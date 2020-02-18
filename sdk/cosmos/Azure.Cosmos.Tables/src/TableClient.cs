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

        public async Task<Response<IDictionary<string, object>>> InsertAsync(TableRow row, CancellationToken cancellationToken = default)
        {
            ResponseWithHeaders<IDictionary<string, object>, InsertEntityHeaders> response =
                await _tableOperations.InsertEntityAsync(_operationTimeout, string.Empty, _format, _table, row.ToValueDictionary(), cancellationToken)
                    .ConfigureAwait(false);

            return response;
        }

        public async Task<Response> UpdateAsync(TableRow row, CancellationToken cancellationToken= default)
        {
            ResponseWithHeaders<UpdateEntityHeaders> response = await _tableOperations.UpdateEntityAsync(_operationTimeout, string.Empty, _format, _table, row.PartitionKey, row.RowKey, row.ToValueDictionary(), cancellationToken);

            return response.GetRawResponse();
        }

        public AsyncPageable<TableRow> QueryAsync(string select = null, string filter = null, int? limit = null, CancellationToken cancellationToken = default)
        {
            return PageableHelpers.CreateAsyncEnumerable(async tableName =>
            {
                var response = await _tableOperations.QueryEntitiesAsync(_operationTimeout, string.Empty, _format, limit, @select, filter, _table, cancellationToken: cancellationToken).ConfigureAwait(false);
                return Page.FromValues(response.Value.Value.Select(row => new TableRow(row)), null, response.GetRawResponse());
            });
        }
    }
}