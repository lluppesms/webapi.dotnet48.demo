// --------------------------------------------------------------------------------
// Bicep file that builds all the resource names used by other Bicep templates
// --------------------------------------------------------------------------------
param appName string = ''
@allowed(['azd','gha','azdo','dev','demo','qa','stg','ct','prod'])
param environmentCode string = 'azd'

// --------------------------------------------------------------------------------
var sanitizedEnvironment = toLower(environmentCode)
var sanitizedAppNameWithDashes = replace(replace(toLower(appName), ' ', ''), '_', '')
var sanitizedAppName = replace(replace(replace(toLower(appName), ' ', ''), '-', ''), '_', '')

// pull resource abbreviations from a common JSON file
var resourceAbbreviations = loadJsonContent('./resourceAbbreviations.json')

// --------------------------------------------------------------------------------
output logAnalyticsWorkspaceName string =  toLower('${sanitizedAppNameWithDashes}-${sanitizedEnvironment}-logworkspace')
var webSiteName                         = toLower('${sanitizedAppNameWithDashes}-${sanitizedEnvironment}')
output webSiteName string               = webSiteName
output webSiteAppServicePlanName string = '${webSiteName}-${resourceAbbreviations.appServicePlanSuffix}'
output webSiteAppInsightsName string    = '${webSiteName}-${resourceAbbreviations.appInsightsSuffix}'
output sqlServerName string             = toLower('${sanitizedAppName}sql${sanitizedEnvironment}')

output vnetName string                  = toLower('${resourceAbbreviations.vnetAbbreviation}-${sanitizedAppName}-${sanitizedEnvironment}')
output nsgName string                   = toLower('${resourceAbbreviations.networkSecurityGroupAbbreviation}-${sanitizedAppName}-${sanitizedEnvironment}')
output publicIpNamePrefix string        = toLower('${resourceAbbreviations.publicIpAbbreviation}-${sanitizedAppName}-${sanitizedEnvironment}')
output bastionName string               = toLower('${resourceAbbreviations.bastionAbbreviation}-${sanitizedAppName}-${sanitizedEnvironment}')
output subnetApplicationName string     = 'appsubnet'
output subnetServicesName string        = 'svcsubnet'
output subnetManagementName string      = 'mgtsubnet'
output subnetBastionName string         = 'AzureBastionSubnet'

output sqlServerEndpointName string     = toLower('${resourceAbbreviations.vnetAbbreviation}-${resourceAbbreviations.privateEndpointAbbreviation}-${sanitizedAppName}${resourceAbbreviations.sqlAbbreviation}${sanitizedEnvironment}')
output storageEndpointName string       = toLower('${resourceAbbreviations.vnetAbbreviation}-${resourceAbbreviations.privateEndpointAbbreviation}-${sanitizedAppName}${resourceAbbreviations.storageAccountSuffix}${sanitizedEnvironment}')
output privateEndpointPrefix string     = toLower('${resourceAbbreviations.vnetAbbreviation}-${resourceAbbreviations.privateEndpointAbbreviation}-${sanitizedAppName}${sanitizedEnvironment}')

// Key Vaults and Storage Accounts can only be 24 characters long
output keyVaultName string              = take('${sanitizedAppName}${resourceAbbreviations.keyVaultAbbreviation}${sanitizedEnvironment}', 24)
output storageAccountName string        = take('${sanitizedAppName}${resourceAbbreviations.storageAccountSuffix}${sanitizedEnvironment}', 24)
