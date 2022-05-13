param (
    [Parameter()]
    [String]
    $Environment = "DEV",

    [Parameter()]
    [Boolean]
    $DryRun = $false,

    [Parameter()]
    [String]
    $Location = "australiasoutheast"
)

Write-Host "Creating Parameters"
$DeploymentTimestamp = Get-Date -Format "yyMMddHHmmss" -AsUTC
$ResourceGroup = ("TerrariaMate-" + $Environment.ToUpper()) 

# Create parameters
$Parameters = New-Object PSObject -Property @{
    '$schema'      = 'https://schema.management.azure.com/schemas/2019-04-01/deploymentParameters.json#'
    contentVersion = "1.0.0.0"
    parameters     = 
    @{
        environmentName    = @{ value = $Environment }
        lastDeploymentDate = @{ value = $DeploymentTimestamp }
    }
}

# Remove old parameters
if (Test-Path ./azuredeploy.parameters.json) {
    Remove-Item ./azuredeploy.parameters.json
}

# Print parameters to file as JSON
$parameters | ConvertTo-Json -Depth 100 | Out-File ./azuredeploy.parameters.json

# Create Resource Group if not exists
$ResourceGroupExists = az group exists --name $ResourceGroup

if ($ResourceGroupExists -eq $false) {
    az group create --name $ResourceGroup --location $Location
}

Write-Host "Running What-If on bicep"
az deployment group what-if `
    --template-file .\Bicep\Deploy.bicep `
    -g $ResourceGroup `
    --parameters '@azuredeploy.parameters.json'

if ($DryRun -eq $false) {
    Write-Host "Deploying bicep"
    az deployment group create `
        --template-file .\Bicep\Deploy.bicep `
        -g $ResourceGroup `
        --name "azuredeploy-$($deploymentTimestamp)" `
        --mode Incremental `
        --parameters '@azuredeploy.parameters.json'

    if (!$?) {
        Write-Host "Deploying Bicep failed... aborting!"
        exit 1
    }
}

Write-Host "Done"