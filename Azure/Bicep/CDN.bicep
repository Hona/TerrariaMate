param appName string
param environmentName string
param location string

@description('Resource tags for organizing / cost monitoring')
param tags object

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

var webappName = '${appName}-${environmentName}'

resource servicePlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: webappName
  location: location
  kind: 'linux'
  tags: tags
  sku: {
    name: webAppSkuName
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: webappName
  location: location
  kind: 'web'
  tags: tags
  properties: {
    Application_Type: 'web'
  }
}

resource webapp 'Microsoft.Web/sites@2021-01-01' = {
  name: webappName
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  //  "api", "app", "app,linux", "functionapp", "functionapp,linux"
  kind: 'app,linux'
  properties: {
    serverFarmId: servicePlan.id
    httpsOnly: true
    siteConfig: {
      alwaysOn: false
      minTlsVersion: '1.2'
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
      ]
    }
  }
}

output cdnUrl string = 'https://${webapp.properties.defaultHostName}'
