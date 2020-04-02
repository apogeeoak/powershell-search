# PowerShell Search

Search for modified files and folders.

## Building from source

To build from source use:

``` shell
dotnet build
```

To publish for release:

``` shell
dotnet publish -c Release -o Builds
```

## Example use case

``` shell
$date = Get-Date
[Perform operation that modifies file system.]
Write-RecentlyModifiedToFile -After $date
```
