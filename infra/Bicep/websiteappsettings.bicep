// --------------------------------------------------------------------------------
// This BICEP file will add unique Configuration settings to a web app
// --------------------------------------------------------------------------------
// NOTE: See https://learn.microsoft.com/en-us/azure/app-service/configure-common?tabs=portal  
// In a Linux app service, any nested JSON app key like AppSettings:MyKey needs to be 
// configured in App Service as AppSettings__MyKey for the key name. 
// In other words, any : should be replaced by __ (double underscore).
// --------------------------------------------------------------------------------
param webAppName string = ''
param appInsightsKey string = 'myKey'
param customAppSettings object = {}
param customConnectionStrings object = {}

// --------------------------------------------------------------------------------
var BASE_SLOT_APPSETTINGS = {
  APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsKey
  APPLICATIONINSIGHTS_CONNECTION_STRING: 'InstrumentationKey=${appInsightsKey}'
  ApplicationInsightsAgent_EXTENSION_VERSION: '~2'
}

// --------------------------------------------------------------------------------
resource webSiteResource 'Microsoft.Web/sites@2020-06-01' existing = {
  name: webAppName
} 

resource siteAppSettings 'Microsoft.Web/sites/config@2022-09-01' = if (customAppSettings != {}) {
  name: 'appsettings'
  parent: webSiteResource 
  properties: union(BASE_SLOT_APPSETTINGS, customAppSettings)
}

resource siteConnectionStringDatabaseEntities 'Microsoft.Web/sites/config@2022-09-01' = if (customConnectionStrings != {}) {
  name: 'connectionstrings'
  parent: webSiteResource 
  properties: customConnectionStrings
}
