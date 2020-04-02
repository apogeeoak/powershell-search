# Launch

$workspaceFolder = $PSScriptRoot
Import-Module $workspaceFolder\bin\Debug\netstandard2.0\Apogee.Search.psd1

$path = "~/Documents"

Find-Modified $path -Recurse -Exclude Additional
# Find-RecentlyModified $path -Recurse
# Write-RecentlyModifiedToFile

Remove-Module Apogee.Search
