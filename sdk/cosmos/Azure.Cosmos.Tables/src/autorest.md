## AutoRest Configuration

Run `dotnet msbuild /t:GenerateCode` to generate code.

> see https://aka.ms/autorest

``` yaml
input-file:
    -  https://raw.githubusercontent.com/Azure/azure-rest-api-specs/f5cb6fb416ae0a06329599db9dc17c8fdd7f95c7/specification/cosmos-db/data-plane/Microsoft.TablesStorage/preview/2018-10-10/table.json
```