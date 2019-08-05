﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.Management.ServiceBus.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace Azure.Messaging.ServiceBus.UnitTests
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus.Management;
    using Polly;

    internal static class ServiceBusScope
    {
        private static int randomSeed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Rng = new ThreadLocal<Random>( () => new Random(Interlocked.Increment(ref randomSeed)), false);

        private static readonly ServiceBusManagementClient ManagementClient = new ServiceBusManagementClient(new TokenCredentials(GetToken(
            TestUtility.ClientTenant,
            TestUtility.ClientId,
            TestUtility.ClientSecret).GetAwaiter().GetResult()))
        {
            SubscriptionId = TestUtility.Subscription
        };

        /// <summary>
        ///   Creates a temporary Service Bus queue to be used within a given scope and then removed.
        /// </summary>
        ///
        /// <param name="partitioned">If <c>true</c>, a partitioned queue will be used.</param>
        /// <param name="sessionEnabled">If <c>true</c>, a session will be enabled on the queue.</param>
        /// <param name="configureQueue">If provided, an action that can override the default properties used for queue creation.</param>
        /// <param name="caller">The name of the calling method; this is intended to be populated by the runtime.</param>
        ///
        /// <returns>The queue scope that was created.</returns>
        ///
        public static async Task<QueueScope> CreateQueueAsync(bool partitioned,
                                                              bool sessionEnabled,
                                                              Action<SBQueue> configureQueue = null,
                                                              [CallerMemberName] string caller = "")
        {
            var name = $"{ caller }-{ Guid.NewGuid().ToString("D").Substring(0, 8) }";
            var queueDescription = BuildQueueDescription(name, partitioned, sessionEnabled);

            configureQueue?.Invoke(queueDescription);
            await CreateRetryPolicy<SBQueue>().ExecuteAsync( () => ManagementClient.Queues.CreateOrUpdateAsync(TestUtility.ResourceGroup, TestUtility.Namespace, queueDescription.Id, queueDescription));

            return new QueueScope(name, async () =>
            {
                try
                {
                    await CreateRetryPolicy().ExecuteAsync( () => ManagementClient.Queues.DeleteAsync(TestUtility.ResourceGroup, TestUtility.Namespace, name));
                }
                catch (Exception ex)
                {
                    TestUtility.Log($"There was an issue removing the queue: [{ name }].  This is considered non-fatal, but you should remove this manually from the Service Bus namespace. Exception: [{ ex.Message }]");
                }
            });
        }

        /// <summary>
        ///   Creates a temporary Service Bus topic, with subscription to be used within a given scope and then removed.
        /// </summary>
        ///
        /// <param name="partitioned">If <c>true</c>, a partitioned topic will be used.</param>
        /// <param name="sessionEnabled">If <c>true</c>, a session will be enabled  on the subscription.</param>
        /// <param name="configureTopic">If provided, an action that can override the default properties used for topic creation.</param>
        /// <param name="configureSubscription">If provided, an action that can override the default properties used for topic creation.</param>
        /// <param name="caller">The name of the calling method; this is intended to be populated by the runtime.</param>
        ///
        /// <returns>The topic scope that was created.</returns>
        ///
        public static async Task<TopicScope> CreateTopicAsync(bool partitioned,
                                                              bool sessionEnabled,
                                                              Action<SBTopic> configureTopic = null,
                                                              Action<SBSubscription> configureSubscription = null,
                                                              [CallerMemberName] string caller = "")
        {
            var topicName = $"{ caller }-{ Guid.NewGuid().ToString("D").Substring(0, 8) }";
            var subscripionName = (sessionEnabled) ? TestConstants.SessionSubscriptionName : TestConstants.SubscriptionName;
            var topicDescription = BuildTopicDescription(topicName, partitioned);
            var subscriptionDescription = BuildSubscriptionDescription(subscripionName, topicName, sessionEnabled);

            configureTopic?.Invoke(topicDescription);
            configureSubscription?.Invoke(subscriptionDescription);

            await CreateRetryPolicy<SBTopic>().ExecuteAsync( () => ManagementClient.Topics.CreateOrUpdateAsync(TestUtility.ResourceGroup, TestUtility.Namespace, topicDescription.Id, topicDescription));
            await CreateRetryPolicy<SBSubscription>().ExecuteAsync( () => ManagementClient.Subscriptions.CreateOrUpdateAsync(TestUtility.ResourceGroup, TestUtility.Namespace, topicName, subscriptionDescription.Id, subscriptionDescription));

            return new TopicScope(topicName, subscripionName, async () =>
            {
                try
                {
                    await CreateRetryPolicy().ExecuteAsync( () => ManagementClient.Topics.DeleteAsync(TestUtility.ResourceGroup, TestUtility.Namespace, topicName));
                }
                catch (Exception ex)
                {
                    TestUtility.Log($"There was an issue removing the topic: [{ topicName }].  This is considered non-fatal, but you should remove this manually from the Service Bus namespace. Exception: [{ ex.Message }]");
                }
            });
        }

        /// <summary>
        ///   Performs an operation within the scope of a temporary Service Bus queue.
        /// </summary>
        ///
        /// <param name="partitioned">If <c>true</c>, a partitioned queue will be used.</param>
        /// <param name="sessionEnabled">If <c>true</c>, a session will be required on the queue.</param>
        /// <param name="scopedOperationAsync">The asynchronous operation to be performed; the name of the queue will be passed to the operation.</param>
        /// <param name="configureQueue">If provided, an action that can override the default properties used for queue creation.</param>
        /// <param name="caller">The name of the calling method; this is intended to be populated by the runtime.</param>
        ///
        /// <returns>The task representing the operation being performed</returns>
        ///
        public static async Task UsingQueueAsync(bool partitioned,
                                                 bool sessionEnabled,
                                                 Func<string, Task> scopedOperationAsync,
                                                 Action<SBQueue> configureQueue = null,
                                                 [CallerMemberName] string caller = "")
        {
            if (scopedOperationAsync == null)
            {
                throw new ArgumentNullException(nameof(scopedOperationAsync));
            }

            var scope = default(QueueScope);

            try
            {
                 scope = await CreateQueueAsync(partitioned, sessionEnabled, configureQueue, caller);
                 await scopedOperationAsync(scope.Name);
            }
            finally
            {
                if (scope != null)
                {
                    await scope.CleanupAsync();
                }
            }
        }

        /// <summary>
        ///   Performs an operation within the scope of a temporary Service Bus topic, with subscription.
        /// </summary>
        ///
        /// <param name="partitioned">If <c>true</c>, a partitioned topic will be used.</param>
        /// <param name="sessionEnabled">If <c>true</c>, a session will be required on the subscription.</param>
        /// <param name="scopedOperationAsync">The asynchronous operation to be performed; the name of the topic and subscription will be passed to the operation.</param>
        /// <param name="configureTopic">If provided, an action that can override the default properties used for topic creation.</param>
        /// <param name="configureTSubscription">If provided, an action that can override the default properties used for topic creation.</param>
        /// <param name="caller">The name of the calling method; this is intended to be populated by the runtime.</param>
        ///
        /// <returns>The task representing the operation being performed</returns>
        ///
        public static async Task UsingTopicAsync(bool partitioned,
                                                 bool sessionEnabled,
                                                 Func<string, string, Task> scopedOperationAsync,
                                                 Action<SBTopic> configureTopic = null,
                                                 Action<SBSubscription> configureSubscription = null,
                                                 [CallerMemberName] string caller = "")
        {
            if (scopedOperationAsync == null)
            {
                throw new ArgumentNullException(nameof(scopedOperationAsync));
            }

            var scope = default(TopicScope);

            try
            {
                 scope = await CreateTopicAsync(partitioned, sessionEnabled, configureTopic, configureSubscription, caller);
                 await scopedOperationAsync(scope.TopicName, scope.SubscriptionName);
            }
            finally
            {
                await scope?.CleanupAsync();
            }
        }

        private static IAsyncPolicy<T> CreateRetryPolicy<T>(int maxRetryAttempts = TestConstants.RetryMaxAttempts, double exponentialBackoffSeconds = TestConstants.RetryExponentialBackoffSeconds, double baseJitterSeconds = TestConstants.RetryBaseJitterSeconds) =>
            Policy<T>
                .Handle<ServiceBusCommunicationException>()
                .Or<ServiceBusTimeoutException>()
                .WaitAndRetryAsync(maxRetryAttempts, attempt => CalculateRetryDelay(attempt, exponentialBackoffSeconds, baseJitterSeconds));

        private static IAsyncPolicy CreateRetryPolicy(int maxRetryAttempts = TestConstants.RetryMaxAttempts, double exponentialBackoffSeconds = TestConstants.RetryExponentialBackoffSeconds, double baseJitterSeconds = TestConstants.RetryBaseJitterSeconds) =>
            Policy
                .Handle<ServiceBusCommunicationException>()
                .Or<ServiceBusTimeoutException>()
                .WaitAndRetryAsync(maxRetryAttempts, attempt => CalculateRetryDelay(attempt, exponentialBackoffSeconds, baseJitterSeconds));

        private static TimeSpan CalculateRetryDelay(int attempt, double exponentialBackoffSeconds, double baseJitterSeconds) =>
            TimeSpan.FromSeconds((Math.Pow(2, attempt) * exponentialBackoffSeconds) + (Rng.Value.NextDouble() * baseJitterSeconds));

        private static SBQueue BuildQueueDescription(string name, bool partitioned, bool sessionEnabled) =>
            new SBQueue(
                name,
                defaultMessageTimeToLive: TestConstants.QueueDefaultMessageTimeToLive,
                lockDuration: TestConstants.QueueDefaultLockDuration,
                duplicateDetectionHistoryTimeWindow: TestConstants.QueueDefaultDuplicateDetectionHistory,
                maxSizeInMegabytes: TestConstants.QueueDefaultMaxSizeMegabytes,
                enablePartitioning: partitioned,
                requiresSession: sessionEnabled);

        private static SBTopic BuildTopicDescription(string name, bool partitioned) =>
            new SBTopic(
                name,
                defaultMessageTimeToLive: TestConstants.TopicDefaultMessageTimeToLive,
                duplicateDetectionHistoryTimeWindow: TestConstants.TopicDefaultDuplicateDetectionHistory,
                maxSizeInMegabytes: TestConstants.TopicDefaultMaxSizeMegabytes,
                enablePartitioning: partitioned);

        private static SBSubscription BuildSubscriptionDescription(string subscriptionName, string topicName, bool sessionEnabled) =>
            new SBSubscription(
                topicName,
                subscriptionName,
                defaultMessageTimeToLive: TestConstants.SubscriptionDefaultMessageTimeToLive,
                lockDuration: TestConstants.SubscriptionDefaultLockDuration,
                maxDeliveryCount: TestConstants.SubscriptionMaximumDeliveryCount,
                deadLetteringOnMessageExpiration: TestConstants.SubscriptionDefaultDeadLetterOnExpire,
                deadLetteringOnFilterEvaluationExceptions: TestConstants.SubscriptionDefaultDeadLetterOnException,
                requiresSession: sessionEnabled);

        internal sealed class QueueScope
        {
            public readonly string Name;
            private readonly Func<Task> CleanupAction;

            public QueueScope(string name, Func<Task> cleanupAction)
            {
                Name = name;
                CleanupAction = cleanupAction;
            }

            public Task CleanupAsync() => CleanupAction?.Invoke() ?? Task.CompletedTask;
        }

        internal sealed class TopicScope
        {
            public readonly string TopicName;
            public readonly string SubscriptionName;
            private readonly Func<Task> CleanupAction;

            public TopicScope(string topicName, string subscriptionName, Func<Task> cleanupAction)
            {
                TopicName = topicName;
                SubscriptionName = subscriptionName;
                CleanupAction = cleanupAction;
            }

            public Task CleanupAsync() => CleanupAction?.Invoke() ?? Task.CompletedTask;
        }


        private static async Task<string> GetToken(string tenantId, string clientId, string clientSecret)
        {
            var context = new AuthenticationContext($"https://login.microsoftonline.com/{tenantId}");

            return (await context.AcquireTokenAsync(
                "https://management.core.windows.net/",
                new ClientCredential(clientId, clientSecret)
            )).AccessToken;
        }
    }
}
