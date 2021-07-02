// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.HDInsight
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for ExtensionsOperations.
    /// </summary>
    public static partial class ExtensionsOperationsExtensions
    {
            /// <summary>
            /// Enables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Operations Management Suite (OMS) workspace parameters.
            /// </param>
            public static void EnableMonitoring(this IExtensionsOperations operations, string resourceGroupName, string clusterName, ClusterMonitoringRequest parameters)
            {
                operations.EnableMonitoringAsync(resourceGroupName, clusterName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Enables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Operations Management Suite (OMS) workspace parameters.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task EnableMonitoringAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, ClusterMonitoringRequest parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.EnableMonitoringWithHttpMessagesAsync(resourceGroupName, clusterName, parameters, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets the status of Operations Management Suite (OMS) on the HDInsight
            /// cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            public static ClusterMonitoringResponse GetMonitoringStatus(this IExtensionsOperations operations, string resourceGroupName, string clusterName)
            {
                return operations.GetMonitoringStatusAsync(resourceGroupName, clusterName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the status of Operations Management Suite (OMS) on the HDInsight
            /// cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ClusterMonitoringResponse> GetMonitoringStatusAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetMonitoringStatusWithHttpMessagesAsync(resourceGroupName, clusterName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Disables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            public static void DisableMonitoring(this IExtensionsOperations operations, string resourceGroupName, string clusterName)
            {
                operations.DisableMonitoringAsync(resourceGroupName, clusterName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Disables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DisableMonitoringAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DisableMonitoringWithHttpMessagesAsync(resourceGroupName, clusterName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Enables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Log Analytics workspace parameters.
            /// </param>
            public static void EnableAzureMonitor(this IExtensionsOperations operations, string resourceGroupName, string clusterName, AzureMonitorRequest parameters)
            {
                operations.EnableAzureMonitorAsync(resourceGroupName, clusterName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Enables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Log Analytics workspace parameters.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task EnableAzureMonitorAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, AzureMonitorRequest parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.EnableAzureMonitorWithHttpMessagesAsync(resourceGroupName, clusterName, parameters, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets the status of Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            public static AzureMonitorResponse GetAzureMonitorStatus(this IExtensionsOperations operations, string resourceGroupName, string clusterName)
            {
                return operations.GetAzureMonitorStatusAsync(resourceGroupName, clusterName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the status of Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AzureMonitorResponse> GetAzureMonitorStatusAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAzureMonitorStatusWithHttpMessagesAsync(resourceGroupName, clusterName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Disables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            public static void DisableAzureMonitor(this IExtensionsOperations operations, string resourceGroupName, string clusterName)
            {
                operations.DisableAzureMonitorAsync(resourceGroupName, clusterName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Disables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DisableAzureMonitorAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DisableAzureMonitorWithHttpMessagesAsync(resourceGroupName, clusterName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Creates an HDInsight cluster extension.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='parameters'>
            /// The cluster extensions create request.
            /// </param>
            public static void Create(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, Extension parameters)
            {
                operations.CreateAsync(resourceGroupName, clusterName, extensionName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates an HDInsight cluster extension.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='parameters'>
            /// The cluster extensions create request.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task CreateAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, Extension parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.CreateWithHttpMessagesAsync(resourceGroupName, clusterName, extensionName, parameters, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets the extension properties for the specified HDInsight cluster
            /// extension.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            public static ClusterMonitoringResponse Get(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName)
            {
                return operations.GetAsync(resourceGroupName, clusterName, extensionName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the extension properties for the specified HDInsight cluster
            /// extension.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ClusterMonitoringResponse> GetAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(resourceGroupName, clusterName, extensionName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes the specified extension for HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            public static void Delete(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName)
            {
                operations.DeleteAsync(resourceGroupName, clusterName, extensionName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the specified extension for HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteWithHttpMessagesAsync(resourceGroupName, clusterName, extensionName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets the async operation status.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='operationId'>
            /// The long running operation id.
            /// </param>
            public static AsyncOperationResult GetAzureAsyncOperationStatus(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, string operationId)
            {
                return operations.GetAzureAsyncOperationStatusAsync(resourceGroupName, clusterName, extensionName, operationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the async operation status.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='operationId'>
            /// The long running operation id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AsyncOperationResult> GetAzureAsyncOperationStatusAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, string operationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAzureAsyncOperationStatusWithHttpMessagesAsync(resourceGroupName, clusterName, extensionName, operationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Enables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Operations Management Suite (OMS) workspace parameters.
            /// </param>
            public static void BeginEnableMonitoring(this IExtensionsOperations operations, string resourceGroupName, string clusterName, ClusterMonitoringRequest parameters)
            {
                operations.BeginEnableMonitoringAsync(resourceGroupName, clusterName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Enables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Operations Management Suite (OMS) workspace parameters.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginEnableMonitoringAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, ClusterMonitoringRequest parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginEnableMonitoringWithHttpMessagesAsync(resourceGroupName, clusterName, parameters, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Disables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            public static void BeginDisableMonitoring(this IExtensionsOperations operations, string resourceGroupName, string clusterName)
            {
                operations.BeginDisableMonitoringAsync(resourceGroupName, clusterName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Disables the Operations Management Suite (OMS) on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginDisableMonitoringAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginDisableMonitoringWithHttpMessagesAsync(resourceGroupName, clusterName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Enables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Log Analytics workspace parameters.
            /// </param>
            public static void BeginEnableAzureMonitor(this IExtensionsOperations operations, string resourceGroupName, string clusterName, AzureMonitorRequest parameters)
            {
                operations.BeginEnableAzureMonitorAsync(resourceGroupName, clusterName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Enables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='parameters'>
            /// The Log Analytics workspace parameters.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginEnableAzureMonitorAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, AzureMonitorRequest parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginEnableAzureMonitorWithHttpMessagesAsync(resourceGroupName, clusterName, parameters, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Disables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            public static void BeginDisableAzureMonitor(this IExtensionsOperations operations, string resourceGroupName, string clusterName)
            {
                operations.BeginDisableAzureMonitorAsync(resourceGroupName, clusterName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Disables the Azure Monitor on the HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginDisableAzureMonitorAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginDisableAzureMonitorWithHttpMessagesAsync(resourceGroupName, clusterName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Creates an HDInsight cluster extension.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='parameters'>
            /// The cluster extensions create request.
            /// </param>
            public static void BeginCreate(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, Extension parameters)
            {
                operations.BeginCreateAsync(resourceGroupName, clusterName, extensionName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates an HDInsight cluster extension.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='parameters'>
            /// The cluster extensions create request.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginCreateAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, Extension parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginCreateWithHttpMessagesAsync(resourceGroupName, clusterName, extensionName, parameters, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Deletes the specified extension for HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            public static void BeginDelete(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName)
            {
                operations.BeginDeleteAsync(resourceGroupName, clusterName, extensionName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes the specified extension for HDInsight cluster.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group.
            /// </param>
            /// <param name='clusterName'>
            /// The name of the cluster.
            /// </param>
            /// <param name='extensionName'>
            /// The name of the cluster extension.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task BeginDeleteAsync(this IExtensionsOperations operations, string resourceGroupName, string clusterName, string extensionName, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.BeginDeleteWithHttpMessagesAsync(resourceGroupName, clusterName, extensionName, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

    }
}
