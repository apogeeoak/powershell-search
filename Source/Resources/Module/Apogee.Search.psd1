### Module manifest.

@{
    RootModule           = 'Apogee.Search.psm1'
    NestedModules        = @('Apogee.Search.dll')
    ModuleVersion        = '1.0.1'
    CompatiblePSEditions = @('Core')
    GUID                 = '0cc15e8e-6f85-419a-9b1d-273edff31215'
    Author               = 'Apogee'
    Copyright            = 'Copyright (c) Apogee. All rights reserved.'
    Description          = 'Basic file system search functions.'
    VariablesToExport    = @()
    AliasesToExport      = @()
    CmdletsToExport      =
        'Find-Modified'
    FunctionsToExport    =
        'Find-RecentlyModified',
        'Write-RecentlyModifiedToFile'
}
