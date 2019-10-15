dotnet pack .\eng\services.proj
$dir = "$pwd\artifacts\packages\Debug\"
$updates = gci $dir -Filter *.nupkg | `
    %{ [regex]::match($_, '([A-Za-z\.]+)\.(.*)\.nupkg') } | `
    %{ "<PackageReference Update=`"$($_.Groups[1].Value)`" Version=`"$($_.Groups[2].Value)`" />`n" }

$updates = "<Project>`n<ItemGroup>`n$updates</ItemGroup>`n</Project>"
$updates | Out-File $pwd\PackageOverride.props
dotnet publish .\samples\SmokeTest\ -restore -f netcoreapp2.1 -o smokeTestApp -p:OverridePackagesProps=$pwd\PackageOverride.props -p:AdditionalRestoreSources=$pwd\artifacts\packages\Debug\