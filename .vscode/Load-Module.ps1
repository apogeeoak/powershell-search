# Load-Module

$workspaceFolder = Split-Path $PSScriptRoot
Import-Module $workspaceFolder\bin\Debug\netstandard2.0\Apogee.Search.psd1

$path = "~/Documents"

# Find-Modified $path -Recurse
Find-RecentlyModified $path -Recurse
# Write-RecentlyModifiedToFile $path
