@minLength(3)
@maxLength(21)
@description('Name of this project')
param projectName string = 'TerrariaMate'

@allowed([
  'DEV'
  'PROD'
])
@description('The environment that this project is being deployed to. (eg. Test, Preprod, Prod)')
param environmentName string

@description('Date timestamp of when this deployment was run - defaults to UtcNow()')
param lastDeploymentDate string = utcNow('yyMMddHHmmss')

@description('Resource tags for organizing / cost monitoring')
param tags object = {
  project: projectName
  environment: environmentName
  lastDeploymentDate: lastDeploymentDate
}

@allowed([
  'B1'
  'B2'
  'B3'
  'D1'
  'F1'
  'FREE'
  'I1'
  'I1v2'
  'I2'
  'I2v2'
  'I3'
  'I3v2'
  'P1V2'
  'P1V3'
  'P2V2'
  'P2V3'
  'P3V2'
  'P3V3'
  'PC2'
  'PC3'
  'PC4'
  'S1'
  'S2'
  'S3'
])
param webAppSkuName string = 'F1'

param location string = resourceGroup().location

module CDN 'CDN.bicep' = {
  name: '${projectName}-CDN-${lastDeploymentDate}'
  scope: resourceGroup()
  params: {
    appName: '${projectName}-CDN'
    tags: tags
    webAppSkuName: webAppSkuName
    environmentName: environmentName
    location: location
  }
}
