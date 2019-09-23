﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Azure.Messaging.ServiceBus.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Xunit;

    public abstract class SenderReceiverClientTestBase
    {
        internal async Task PeekLockTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            await TestUtility.SendMessagesAsync(messageSender, messageCount);
            var receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount);
            await TestUtility.CompleteMessagesAsync(messageReceiver, receivedMessages);
        }

        internal async Task ReceiveDeleteTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            await TestUtility.SendMessagesAsync(messageSender, messageCount);
            var receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount, TimeSpan.FromSeconds(10));
            Assert.Equal(receivedMessages.Count, messageCount);
        }

        internal async Task PeekLockWithAbandonTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            // Send messages
            await TestUtility.SendMessagesAsync(messageSender, messageCount);

            // Receive 5 messages and Abandon them
            var abandonMessagesCount = 5;
            var receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, abandonMessagesCount);
            Assert.True(receivedMessages.Count == abandonMessagesCount);

            await TestUtility.AbandonMessagesAsync(messageReceiver, receivedMessages);

            // Receive all 10 messages
            receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount);
            Assert.True(receivedMessages.Count == messageCount);

            // TODO: Some reason for partitioned entities the delivery count is incorrect. Investigate and enable
            // 5 of these messages should have deliveryCount = 2
            var messagesWithDeliveryCount2 = receivedMessages.Where(message => message.DeliveryCount == 2).Count();
            TestUtility.Log($"Messages with Delivery Count 2: {messagesWithDeliveryCount2}");
            Assert.True(messagesWithDeliveryCount2 == abandonMessagesCount);

            // Complete Messages
            await TestUtility.CompleteMessagesAsync(messageReceiver, receivedMessages);
        }

        internal async Task PeekLockWithDeadLetterTestCase(MessageSender messageSender, MessageReceiver messageReceiver, MessageReceiver deadLetterReceiver, int messageCount)
        {
            // Send messages
            await TestUtility.SendMessagesAsync(messageSender, messageCount);

            // Receive 5 messages and Deadletter them
            var deadLetterMessageCount = 5;
            var receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, deadLetterMessageCount);
            Assert.True(receivedMessages.Count == deadLetterMessageCount);

            await TestUtility.DeadLetterMessagesAsync(messageReceiver, receivedMessages);

            // Receive and Complete 5 other regular messages
            receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount - deadLetterMessageCount);
            await TestUtility.CompleteMessagesAsync(messageReceiver, receivedMessages);

            // TODO: After implementing Receive(WithTimeSpan), Add Try another Receive, We should not get anything.
            // IEnumerable<Message> dummyMessages = await this.ReceiveMessagesAsync(queueClient, 10);
            // Assert.True(dummyMessages == null);

            // Receive 5 DLQ messages and Complete them
            receivedMessages = await TestUtility.ReceiveMessagesAsync(deadLetterReceiver, deadLetterMessageCount);
            Assert.True(receivedMessages.Count == deadLetterMessageCount);
            await TestUtility.CompleteMessagesAsync(deadLetterReceiver, receivedMessages);
        }

        internal async Task PeekLockDeferTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            // Send messages
            await TestUtility.SendMessagesAsync(messageSender, messageCount);

            // Receive 5 messages And Defer them
            var deferMessagesCount = 5;
            var receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, deferMessagesCount);
            Assert.True(receivedMessages.Count == deferMessagesCount);
            var sequenceNumbers = receivedMessages.Select(receivedMessage => receivedMessage.SequenceNumber).ToList();
            await TestUtility.DeferMessagesAsync(messageReceiver, receivedMessages);

            // Receive and Complete 5 other regular messages
            receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount - deferMessagesCount);
            await TestUtility.CompleteMessagesAsync(messageReceiver, receivedMessages);
            Assert.True(receivedMessages.Count == messageCount - deferMessagesCount);

            // Receive / Abandon deferred messages
            receivedMessages = await TestUtility.ReceiveDeferredMessagesAsync(messageReceiver, sequenceNumbers);
            Assert.True(receivedMessages.Count == 5);
            await TestUtility.DeferMessagesAsync(messageReceiver, receivedMessages);

            // Receive Again and Check delivery count
            receivedMessages = await TestUtility.ReceiveDeferredMessagesAsync(messageReceiver, sequenceNumbers);
            var count = receivedMessages.Count(message => message.DeliveryCount == 3);
            Assert.True(count == receivedMessages.Count);

            // Complete messages
            await TestUtility.CompleteMessagesAsync(messageReceiver, receivedMessages);
        }

        internal async Task RenewLockTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            // Send messages
            await TestUtility.SendMessagesAsync(messageSender, messageCount);

            // Receive messages
            var receivedMessages = await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount);

            var message = receivedMessages.First();
            var firstLockedUntilUtcTime = message.LockedUntilUtc;
            TestUtility.Log($"MessageLockedUntil: {firstLockedUntilUtcTime}");

            TestUtility.Log("Sleeping 10 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(10));


            await messageReceiver.RenewLockAsync(message);
            TestUtility.Log($"After First Renewal: {message.LockedUntilUtc}");
            Assert.True(message.LockedUntilUtc >= firstLockedUntilUtcTime + TimeSpan.FromSeconds(10));

            TestUtility.Log("Sleeping 5 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(5));

            await messageReceiver.RenewLockAsync(message.LockToken);
            TestUtility.Log($"After Second Renewal: {message.LockedUntilUtc}");
            Assert.True(message.LockedUntilUtc >= firstLockedUntilUtcTime + TimeSpan.FromSeconds(5));

            // Complete Messages
            await TestUtility.CompleteMessagesAsync(messageReceiver, receivedMessages);

            Assert.True(receivedMessages.Count == messageCount);
        }

        internal async Task PeekAsyncTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            await TestUtility.SendMessagesAsync(messageSender, messageCount);
            var peekedMessages = new List<ReceivedMessage>();
            peekedMessages.Add(await TestUtility.PeekMessageAsync(messageReceiver));
            peekedMessages.AddRange(await TestUtility.PeekMessagesAsync(messageReceiver, messageCount - 1));

            Assert.True(messageCount == peekedMessages.Count);
            long lastSequenceNumber = -1;
            foreach (var message in peekedMessages)
            {
                Assert.True(message.SequenceNumber != lastSequenceNumber);
                lastSequenceNumber = message.SequenceNumber;
            }

            await TestUtility.ReceiveMessagesAsync(messageReceiver, messageCount);
        }

        internal async Task ReceiveShouldReturnNoLaterThanServerWaitTimeTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            var timer = Stopwatch.StartNew();
            TestUtility.Log($"starting Receive");
            var message = await messageReceiver.ReceiveAsync(TimeSpan.FromSeconds(2));
            timer.Stop();

            // If message is not null, then the queue needs to be cleaned up before running the timeout test.
            Assert.Null(message);

            // Ensuring total time taken is less than 60 seconds, which is the default timeout for receive.
            // Keeping the value of 40 to avoid flakiness in test infrastructure which may lead to extended time taken.
            // Todo: Change this value to a lower number once test infra is performant.
            TestUtility.Log($"Elapsed Milliseconds: {timer.Elapsed.TotalMilliseconds}");
            Assert.True(timer.Elapsed.TotalSeconds < 40);
        }

        internal async Task ReceiveShouldThrowForServerTimeoutZero(MessageReceiver messageReceiver)
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => messageReceiver.ReceiveAsync(TimeSpan.Zero));
        }

        internal async Task ScheduleMessagesAppearAfterScheduledTimeAsyncTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            var startTime = DateTime.UtcNow;
            var scheduleTime = new DateTimeOffset(DateTime.UtcNow).AddSeconds(5);
            TestUtility.Log($"Sending message with schedule time: {scheduleTime.UtcDateTime}");

            var sequenceNumber =
                await
                    messageSender.ScheduleMessageAsync(
                        new Message(Encoding.UTF8.GetBytes("Test")) { MessageId = "randomId", Label = "randomLabel" }, scheduleTime);
            TestUtility.Log($"Received sequence number: {sequenceNumber}");
            Assert.True(sequenceNumber > 0);

            TestUtility.Log("Sleeping for 5 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(5));

            var message = await messageReceiver.ReceiveAsync();

            // Asserting using Math.Ceiling since TotalSeconds usually ends up being around 4.999 due to precision of
            // the scheduleTime in requestMessage and responseMessage.
            Assert.True(Math.Ceiling(message.ScheduledEnqueueTimeUtc.Subtract(startTime).TotalSeconds) >= 5);
        }

        internal async Task CancelScheduledMessagesAsyncTestCase(MessageSender messageSender, MessageReceiver messageReceiver, int messageCount)
        {
            var scheduleTime = new DateTimeOffset(DateTime.UtcNow).AddSeconds(30);
            var brokeredMessage = new Message(Encoding.UTF8.GetBytes("Test1")) { MessageId = Guid.NewGuid().ToString() };
            TestUtility.Log(
                $"Sending message with schedule time: {scheduleTime.UtcDateTime} and messageID {brokeredMessage.MessageId}");

            var sequenceNumber = await messageSender.ScheduleMessageAsync(brokeredMessage, scheduleTime);
            TestUtility.Log($"Received sequence number: {sequenceNumber}");
            Assert.True(sequenceNumber > 0);

            TestUtility.Log("Cancelling scheduled message");
            await messageSender.CancelScheduledMessageAsync(sequenceNumber);

            TestUtility.Log("Sleeping for 30 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(30));

            // Sending a dummy message so that ReceiveAsync(2) returns immediately after getting 1 message
            // instead of waiting for connection timeout on a single message.
            await messageSender.SendAsync(new Message(Encoding.UTF8.GetBytes(("Dummy"))) { MessageId = "Dummy" });
            IList<ReceivedMessage> messages = null;
            var retryCount = 5;
            while (messages == null && --retryCount > 0)
            {
                messages = await messageReceiver.ReceiveAsync(2);
            }

            Assert.NotNull(messages);
            Assert.True(messages.Count == 1);
            Assert.True(messages.First().MessageId == "Dummy");
        }

        internal async Task OnMessageAsyncTestCase(
            MessageSender messageSender,
            MessageReceiver messageReceiver,
            int maxConcurrentCalls,
            bool autoComplete,
            int messageCount)
        {
            var count = 0;
            await TestUtility.SendMessagesAsync(messageSender, messageCount);
            messageReceiver.RegisterMessageHandler(
                async (message, token) =>
                {
                    TestUtility.Log($"Received message: SequenceNumber: {message.SequenceNumber}");
                    Interlocked.Increment(ref count);
                    if (messageReceiver.ReceiveMode == ReceiveMode.PeekLock && !autoComplete)
                    {
                        await messageReceiver.CompleteAsync(message.LockToken);
                    }
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = maxConcurrentCalls, AutoComplete = autoComplete });

            // Wait for the OnMessage Tasks to finish
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.TotalSeconds <= 60)
            {
                if (count == messageCount)
                {
                    TestUtility.Log($"All '{messageCount}' messages Received.");
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
            Assert.True(count == messageCount);
        }

        internal async Task OnMessageRegistrationWithoutPendingMessagesTestCase(
            MessageSender messageSender,
            MessageReceiver messageReceiver,
            int maxConcurrentCalls,
            bool autoComplete)
        {
            var count = 0;
            messageReceiver.RegisterMessageHandler(
                async (message, token) =>
                {
                    TestUtility.Log($"Received message: SequenceNumber: {message.SequenceNumber}");
                    Interlocked.Increment(ref count);
                    await Task.CompletedTask;
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = maxConcurrentCalls, AutoComplete = autoComplete });

            await TestUtility.SendMessagesAsync(messageSender, 1);

            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.TotalSeconds <= 20)
            {
                if (count == 1)
                {
                    TestUtility.Log($"All messages Received.");
                    break;
                }
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            TestUtility.Log($"{DateTime.Now}: MessagesReceived: {count}");
            Assert.True(count == 1);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs eventArgs)
        {
            TestUtility.Log($"Exception Received: ClientId: {eventArgs.ExceptionReceivedContext.ClientId}, EntityPath: {eventArgs.ExceptionReceivedContext.EntityPath}, Exception: {eventArgs.Exception.Message}");
            return Task.CompletedTask;
        }
    }
}