using System;
using System.Threading;

namespace Azure.Cosmos.Tables
{
    /// <summary>
    /// A <see cref="TablesSharedKeyCredential"/> is a credential backed by
    /// a Storage Account's name and one of its access keys.
    /// </summary>
    public class TablesSharedKeyCredential
    {
        /// <summary>
        /// Gets the name of the Storage Account.
        /// </summary>
        public string AccountName { get; }

        /// <summary>
        /// The value of a Storage Account access key.
        /// </summary>
        private byte[] _accountKeyValue;

        /// <summary>
        /// Gets the value of a Storage Account access key.
        /// </summary>
        internal byte[] AccountKeyValue
        {
            get => Volatile.Read(ref _accountKeyValue);
            set => Volatile.Write(ref _accountKeyValue, value);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TablesSharedKeyCredential"/> class.
        /// </summary>
        /// <param name="accountName">The name of the Storage Account.</param>
        /// <param name="accountKey">A Storage Account access key.</param>
        public TablesSharedKeyCredential(
            string accountName,
            string accountKey)
        {
            AccountName = accountName;
            SetAccountKey(accountKey);
        }

        /// <summary>
        /// Update the Storage Account's access key.  This intended to be used
        /// when you've regenerated your Storage Account's access keys and want
        /// to update long lived clients.
        /// </summary>
        /// <param name="accountKey">A Storage Account access key.</param>
        public void SetAccountKey(string accountKey) =>
            AccountKeyValue = Convert.FromBase64String(accountKey);
    }
}