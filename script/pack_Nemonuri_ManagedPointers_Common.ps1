$projPath = [System.IO.Path]::Combine($PSScriptRoot, "../src/Nemonuri.ManagedPointers.Common/Nemonuri.ManagedPointers.Common.csproj")
$env:DOTNET_CLI_UI_LANGUAGE = 'en'

dotnet pack $projPath