using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Azure.Cosmos.Tables
{
    public class TableClient
    {
        private readonly string _table;
        private readonly ResponseFormat _format;
        private readonly int _operationTimeout;
        private readonly TableOperations _tableOperations;

        internal TableClient(string table, TableOperations tableOperations)
        {
            _tableOperations = tableOperations;
            _table = table;
            _format = ResponseFormat.ApplicationJsonOdataFullmetadata;
            _operationTimeout = 100;
        }

        public async Task<TableItem> CreateAsync(string name)
        {
            var response = await _tableOperations.CreateAsync(null, _format, new TableCreationProperties()
            {
                TableName = name
            }).ConfigureAwait(false);

            return response.Value;
        }

        public async Task<Response> DeleteAsync(string name)
        {
            var response = await _tableOperations.DeleteAsync(null, name).ConfigureAwait(false);

            return response.GetRawResponse();
        }

        public async Task<Response<IDictionary<string, object>>> InsertAsync(TableEntity entity, CancellationToken cancellationToken = default)
        {
            ResponseWithHeaders<IDictionary<string, object>, InsertEntityHeaders> response =
                await _tableOperations.InsertEntityAsync(_operationTimeout, string.Empty, _format, _table, entity.ToValueDictionary(), cancellationToken)
                    .ConfigureAwait(false);

            return response;
        }

        public async Task<Response> UpdateAsync(TableEntity entity, CancellationToken cancellationToken= default)
        {
            ResponseWithHeaders<UpdateEntityHeaders> response = await _tableOperations.UpdateEntityAsync(_operationTimeout, string.Empty, _format, _table, entity.PartitionKey, entity.RowKey, entity.ToValueDictionary(), cancellationToken);

            return response.GetRawResponse();
        }

        public AsyncPageable<TableEntity> QueryAsync(string select = null, string filter = null, int? limit = null, CancellationToken cancellationToken = default)
        {
            return PageableHelpers.CreateAsyncEnumerable(async tableName =>
            {
                var response = await _tableOperations.QueryEntitiesAsync(_operationTimeout, string.Empty, _format, limit, @select, filter, _table, cancellationToken: cancellationToken).ConfigureAwait(false);
                return Page.FromValues(response.Value.Value.Select(row => new TableEntity(row)), null, response.GetRawResponse());
            });
        }
    }
}