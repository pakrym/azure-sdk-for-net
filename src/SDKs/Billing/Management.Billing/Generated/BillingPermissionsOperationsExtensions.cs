// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Billing
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for BillingPermissionsOperations.
    /// </summary>
    public static partial class BillingPermissionsOperationsExtensions
    {
            /// <summary>
            /// Lists all billing permissions for the caller under a billing account.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='billingAccountName'>
            /// billing Account Id.
            /// </param>
            public static BillingPermissionsListResult ListByBillingAccount(this IBillingPermissionsOperations operations, string billingAccountName)
            {
                return operations.ListByBillingAccountAsync(billingAccountName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Lists all billing permissions for the caller under a billing account.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='billingAccountName'>
            /// billing Account Id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<BillingPermissionsListResult> ListByBillingAccountAsync(this IBillingPermissionsOperations operations, string billingAccountName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByBillingAccountWithHttpMessagesAsync(billingAccountName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Lists all billing permissions for the caller under invoice section.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='billingAccountName'>
            /// billing Account Id.
            /// </param>
            /// <param name='invoiceSectionName'>
            /// InvoiceSection Id.
            /// </param>
            public static BillingPermissionsListResult ListByInvoiceSections(this IBillingPermissionsOperations operations, string billingAccountName, string invoiceSectionName)
            {
                return operations.ListByInvoiceSectionsAsync(billingAccountName, invoiceSectionName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Lists all billing permissions for the caller under invoice section.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='billingAccountName'>
            /// billing Account Id.
            /// </param>
            /// <param name='invoiceSectionName'>
            /// InvoiceSection Id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<BillingPermissionsListResult> ListByInvoiceSectionsAsync(this IBillingPermissionsOperations operations, string billingAccountName, string invoiceSectionName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByInvoiceSectionsWithHttpMessagesAsync(billingAccountName, invoiceSectionName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Lists all billingPermissions for the caller has for a billing account.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='billingAccountName'>
            /// billing Account Id.
            /// </param>
            /// <param name='billingProfileName'>
            /// Billing Profile Id.
            /// </param>
            public static BillingPermissionsListResult ListByBillingProfile(this IBillingPermissionsOperations operations, string billingAccountName, string billingProfileName)
            {
                return operations.ListByBillingProfileAsync(billingAccountName, billingProfileName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Lists all billingPermissions for the caller has for a billing account.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='billingAccountName'>
            /// billing Account Id.
            /// </param>
            /// <param name='billingProfileName'>
            /// Billing Profile Id.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<BillingPermissionsListResult> ListByBillingProfileAsync(this IBillingPermissionsOperations operations, string billingAccountName, string billingProfileName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByBillingProfileWithHttpMessagesAsync(billingAccountName, billingProfileName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
