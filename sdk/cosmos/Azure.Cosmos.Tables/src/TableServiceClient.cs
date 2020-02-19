using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Cosmos.Tables.Models;

namespace Azure.Cosmos.Tables
{
    public class TableServiceClient
    {
        private readonly TableOperations _tableOperations;
        private readonly ResponseFormat _format = ResponseFormat.ApplicationJsonOdataFullmetadata;

        public TableServiceClient(Uri endpoint, TablesSharedKeyCredential credential, TableClientOptions options = null)
        {
            options ??= new TableClientOptions();

            var endpoint1 = endpoint.ToString();
            var pipeline = HttpPipelineBuilder.Build(options, new TablesSharedKeyPipelinePolicy(credential));
            var diagnostics = new ClientDiagnostics(options);
            _tableOperations = new TableOperations(diagnostics, pipeline, endpoint1, "2019-02-02");
        }

        public TableClient GetTable(string name)
        {
            return new TableClient(name, _tableOperations);
        }

        public AsyncPageable<TableItem> GetTablesAsync(CancellationToken cancellationToken = default)
        {
            return PageableHelpers.CreateAsyncEnumerable(async _ =>
            {
                var response = await _tableOperations.QueryAsync(null, _format, null, null, null, cancellationToken).ConfigureAwait(false);
                return Page.FromValues(response.Value.Value, response.Headers.XMsContinuationNextTableName, response.GetRawResponse());
            });
        }
    }
}