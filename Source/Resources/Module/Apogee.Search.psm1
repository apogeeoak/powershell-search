### Apogee.Search

<#
.Synopsis
    Finds items modified after a given time.
.Description
    Finds files and folders modified after a given time.
.Parameter Path
    The paths to begin the search from. Default value: (Current directory).
.Parameter Recurse
    Switch to determine if child items are searched. Default value: (False).
.Parameter Depth
    The recursion depth. Default value: (None).
.Parameter After
    The datetime after which to search for modified items. Default value: (30 minutes).
.Parameter ExcludeLevel
    The exclusion level for items. Default value: (None).
.Parameter DisplayProgress
    Switch to determine if incremental progress is displayed. Default value: (False).
.Example
    Find-RecentlyModified
    Finds the files and folders in the current directory modified within the default time frame.
.Outputs
    System.String
    Returns the full path of the modified items.
#>
function Find-RecentlyModified (
    [string[]] $Path,
    [switch] $Recurse,
    [uint] $Depth,
    [datetime] $After = (Get-Date).AddMinutes(-30),
    [Nullable[Apogee.Search.Model.ExcludeLevel]] $ExcludeLevel,
    [switch] $DisplayProgress)
{
    $parameters = @{
        Path            = $Path
        Recurse         = $Recuse
        After           = $After
        DisplayProgress = $DisplayProgress
    }

    if ($Depth) { $parameters.Depth = $Depth }
    if ($ExcludeLevel) { $parameters.ExcludeLevel = $ExcludeLevel }

    Find-Modified @parameters
}

<#
.Synopsis
    Writes items modified after a given time to a file.
.Description
    Writes files and folders modified after a given time to a file. Uses a recursive search.
.Parameter Path
    The paths to begin the search from. Default value: ('Root directory: /').
.Parameter NoRecurse
    Search only the given items. By default child items are also searched. Default value: (False).
.Parameter Depth
    The recursion depth. Default value: (None).
.Parameter After
    The datetime after which to search for modified items. Default value: (30 minutes).
.Parameter ExcludeLevel
    The exclusion level for items. Default value: (None).
.Parameter NoProgress
    Do not display incremental progress. Default value: (False).
.Parameter File
    The file to write the search results to. Default value: ('found.txt').
.Example
    Write-RecentlyModifiedToFile
    Writes the files and folders within the root '/' directory modified within the default time frame to the default file.
.Outputs
    None
#>
function Write-RecentlyModifiedToFile (
    [string[]] $Path = '/',
    [switch] $NoRecurse,
    [uint] $Depth,
    [datetime] $After = (Get-Date).AddMinutes(-30),
    [Nullable[Apogee.Search.Model.ExcludeLevel]] $ExcludeLevel,
    [switch] $NoProgress,
    [string] $File = 'found.txt',
    [string] $ErrorFile = 'error.txt')
{
    $parameters = @{
        Path            = $Path
        Recurse         = !$NoRecuse
        After           = $After
        DisplayProgress = !$NoProgress
        ErrorVariable   = 'errorFinder'
        ErrorAction     = 'SilentlyContinue'
    }

    if ($Depth) { $parameters.Depth = $Depth }
    if ($ExcludeLevel) { $parameters.ExcludeLevel = $ExcludeLevel }

    Add-Content $File ("`nModified Files for $Path`n`n`t{0, -15} $(Get-Date)`n`t{1, -15} $After`n" -f 'Timestamp:', 'Modified after:')

    Find-Modified @parameters | Add-Content $File

    # Error logging.
    if ($errorFinder)
    {
        Add-Content $ErrorFile ("`nErrors for $Path`n`n`t{0, -15} $(Get-Date)`n`t{1, -15} $After`n" -f 'Timestamp:', 'Modified after:')
        $errorFinder | Select-Object $PSItem.Exception.Message | Add-Content $ErrorFile
    }
}
