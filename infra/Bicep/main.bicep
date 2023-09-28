// --------------------------------------------------------------------------------
// Main Bicep file that creates all of the Azure Resources for one environment
// --------------------------------------------------------------------------------
// To deploy this Bicep manually:
// 	 az login
//   az account set --subscription <subscriptionId>
//   az deployment group create -n main-deploy-20221129T150000Z --resource-group rg_myapp_test --template-file 'main.bicep' --parameters appName=xxx-myapp-test environmentCode=demo keyVaultOwnerUserId=xxxxxxxx-xxxx-xxxx
// --------------------------------------------------------------------------------
param appName string = ''
@allowed(['azd','gha','azdo','dev','demo','qa','stg','ct','prod'])
param environmentCode string = 'azd'
param location string = resourceGroup().location
param keyVaultOwnerUserId string = ''
param databaseName string = 'myapp'
@allowed(['Basic','Standard','Premium','BusinessCritical','GeneralPurpose'])
param sqlSkuTier string = 'GeneralPurpose'
param sqlSkuFamily string = 'Gen5'
param sqlSkuName string = 'GP_S_Gen5'
param adminLoginUserId string = ''
param adminLoginUserSid string = ''
param adminLoginTenantId string = ''

param storageSku string = 'Standard_LRS'
param runDateTime string = utcNow()
param webSiteSku string = 'B1'

param appSwaggerEnabled string = 'true'

param adDomain string = ''
param adTenantId string = ''
param adClientId string = ''
param adInstance string = environment().authentication.loginEndpoint // 'https://login.microsoftonline.com/'

// --------------------------------------------------------------------------------
var deploymentSuffix = '-${runDateTime}'
var insightKeyName = 'AppInsightsKey' 
var commonTags = {         
  LastDeployed: runDateTime
  Application: appName
  Environment: environmentCode
}

// --------------------------------------------------------------------------------
module resourceNames 'resourcenames.bicep' = {
  name: 'resourcenames${deploymentSuffix}'
  params: {
    appName: appName
    environmentCode: environmentCode
  }
}
// --------------------------------------------------------------------------------
module logAnalyticsWorkspaceModule 'loganalyticsworkspace.bicep' = {
  name: 'logAnalytics${deploymentSuffix}'
  params: {
    logAnalyticsWorkspaceName: resourceNames.outputs.logAnalyticsWorkspaceName
    location: location
    commonTags: commonTags
  }
}

// --------------------------------------------------------------------------------
module storageModule 'storageaccount.bicep' = {
  name: 'storage${deploymentSuffix}'
  params: {
    storageSku: storageSku
    storageAccountName: resourceNames.outputs.storageAccountName
    location: location
    commonTags: commonTags
  }
}

module sqlDbModule 'sqlserver.bicep' = {
  name: 'sql-server${deploymentSuffix}'
  params: {
    sqlServerName: resourceNames.outputs.sqlServerName
    sqlDBName: databaseName
    sqlSkuTier: sqlSkuTier
    sqlSkuName: sqlSkuName
    sqlSkuFamily: sqlSkuFamily
    mincores: 1
    autopause: 60
    location: location
    commonTags: commonTags
    adAdminUserId: adminLoginUserId
    adAdminUserSid: adminLoginUserSid
    adAdminTenantId: adminLoginTenantId
    // sqldbAdminUserId: sqldbAdminUserId
    // sqldbAdminPassword: sqldbAdminPassword
  }
}

module webSiteModule 'website.bicep' = {
  name: 'webSite${deploymentSuffix}'
  params: {
    webSiteName: resourceNames.outputs.webSiteName
    location: location
    appInsightsLocation: location
    commonTags: commonTags
    sku: webSiteSku
    environmentCode: environmentCode
    workspaceId: logAnalyticsWorkspaceModule.outputs.id
  }
}

module keyVaultModule 'keyvault.bicep' = {
  name: 'keyvault${deploymentSuffix}'
  dependsOn: [ webSiteModule ]
  params: {
    keyVaultName: resourceNames.outputs.keyVaultName
    location: location
    commonTags: commonTags
    adminUserObjectIds: [ keyVaultOwnerUserId ]
    applicationUserObjectIds: [ webSiteModule.outputs.principalId ]
    workspaceId: logAnalyticsWorkspaceModule.outputs.id
  }
}

module keyVaultSecretList 'keyvaultlistsecretnames.bicep' = {
  name: 'keyVault-Secret-List-Names${deploymentSuffix}'
  dependsOn: [ keyVaultModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    location: location
    userManagedIdentityId: keyVaultModule.outputs.userManagedIdentityId
  }
}

module keyVaultSecret1 'keyvaultsecret.bicep' = {
  name: 'keyVaultSecret1${deploymentSuffix}'
  dependsOn: [ keyVaultModule, webSiteModule ]
  params: {
    keyVaultName: keyVaultModule.outputs.name
    secretName: insightKeyName
    secretValue: webSiteModule.outputs.appInsightsKey
    existingSecretNames: keyVaultSecretList.outputs.secretNameList
  }
}  

// With a Managed Identity, this is no longer needed...
// module keyVaultSecret2 'keyvaultsecretsqlserver.bicep' = {
//   name: 'keyVaultSecret2${deploymentSuffix}'
//   dependsOn: [ keyVaultModule, sqlDbModule ]
//   params: {
//     keyVaultName: keyVaultModule.outputs.name
//     secretName: 'sqlConnectionString'
//     sqlServerName: sqlDbModule.outputs.serverName
//     sqlDatabaseName: sqlDbModule.outputs.databaseName
//     existingSecretNames: keyVaultSecretList.outputs.secretNameList
//   }
// }

// In a Linux app service, any nested JSON app key like AppSettings:MyKey needs to be 
// configured in App Service as AppSettings__MyKey for the key name. 
// In other words, any : should be replaced by __ (double underscore).
// NOTE: See https://learn.microsoft.com/en-us/azure/app-service/configure-common?tabs=portal  
module webSiteAppSettingsModule 'websiteappsettings.bicep' = {
  name: 'webSiteAppSettings${deploymentSuffix}'
  dependsOn: [ keyVaultSecret1 ]
  params: {
    webAppName: webSiteModule.outputs.name
    appInsightsKey: webSiteModule.outputs.appInsightsKey
    customAppSettings: {
      AppSettings__AppInsights_InstrumentationKey: webSiteModule.outputs.appInsightsKey
      AppSettings__EnvironmentName: environmentCode
      AppSettings__EnableSwagger: appSwaggerEnabled
      AppSettings__DefaultConnection: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=sqlConnectionString)'
      AzureAD__AADInstance: adInstance
      AzureAD__Domain: adDomain
      AzureAD__TenantId: adTenantId
      AzureAD__ClientId: adClientId
      AzureAD__PostLogoutRedirectUri: 'https://${webSiteModule.outputs.name}.azurewebsites.net/signin-oidc'
    }
    
    // With a Managed Identity, you only need a very simple (not secret) connection string...
    customConnectionStrings: {
      DatabaseEntities: {
        value: 'data source=tcp:${sqlDbModule.outputs.serverName}.database.windows.net,1433;initial catalog=${sqlDbModule.outputs.databaseName};'
        type: 'SQLServer'
      }
    }
    // // With a Managed Identity, the key vault is no longer needed...
    // customConnectionStrings: {
    //   DatabaseEntities: {
    //     value: '@Microsoft.KeyVault(VaultName=${keyVaultModule.outputs.name};SecretName=sqlConnectionString)'
    //     type: 'SQLServer'
    //   }
    // }
  }
}

// --------------------------------------------------------------------------------
var fqdnSqlServer = '${sqlDbModule.outputs.serverName}${environment().suffixes.sqlServerHostname}'
output fqdnSqlServerName string = fqdnSqlServer
output sqlDatabaseName string = databaseName
