// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Sql
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for ManagedInstanceOperations.
    /// </summary>
    public static partial class ManagedInstanceOperationsExtensions
    {
            /// <summary>
            /// Cancels the asynchronous operation on the managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='operationId'>
            /// </param>
            public static void Cancel(this IManagedInstanceOperations operations, string resourceGroupName, string managedInstanceName, System.Guid operationId)
            {
                operations.CancelAsync(resourceGroupName, managedInstanceName, operationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Cancels the asynchronous operation on the managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='operationId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task CancelAsync(this IManagedInstanceOperations operations, string resourceGroupName, string managedInstanceName, System.Guid operationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.CancelWithHttpMessagesAsync(resourceGroupName, managedInstanceName, operationId, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Gets a list of operations performed on the managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            public static IPage<ManagedInstanceOperation> ListByManagedInstance(this IManagedInstanceOperations operations, string resourceGroupName, string managedInstanceName)
            {
                return operations.ListByManagedInstanceAsync(resourceGroupName, managedInstanceName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets a list of operations performed on the managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<ManagedInstanceOperation>> ListByManagedInstanceAsync(this IManagedInstanceOperations operations, string resourceGroupName, string managedInstanceName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByManagedInstanceWithHttpMessagesAsync(resourceGroupName, managedInstanceName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Gets a management operation on a managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='operationId'>
            /// </param>
            public static ManagedInstanceOperation Get(this IManagedInstanceOperations operations, string resourceGroupName, string managedInstanceName, System.Guid operationId)
            {
                return operations.GetAsync(resourceGroupName, managedInstanceName, operationId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets a management operation on a managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='operationId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ManagedInstanceOperation> GetAsync(this IManagedInstanceOperations operations, string resourceGroupName, string managedInstanceName, System.Guid operationId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(resourceGroupName, managedInstanceName, operationId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Gets a list of operations performed on the managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            public static IPage<ManagedInstanceOperation> ListByManagedInstanceNext(this IManagedInstanceOperations operations, string nextPageLink)
            {
                return operations.ListByManagedInstanceNextAsync(nextPageLink).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets a list of operations performed on the managed instance.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='nextPageLink'>
            /// The NextLink from the previous successful call to List operation.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IPage<ManagedInstanceOperation>> ListByManagedInstanceNextAsync(this IManagedInstanceOperations operations, string nextPageLink, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByManagedInstanceNextWithHttpMessagesAsync(nextPageLink, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
