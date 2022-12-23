@description('Specifies the name of the App Configuration Services store.')
param appConfigStoreName string = 'app-config-store-${uniqueString(resourceGroup().id)}'

@description('Specifies the Azure location where the app configuration store should be created.')
param location string = resourceGroup().location

// Load pairs from file
var keyValuePairs = loadJsonContent('key-value-pairs.json')

// to create Azure App Configuration Service 
resource configStore 'Microsoft.AppConfiguration/configurationStores@2021-10-01-preview' = {
  name: appConfigStoreName
  location: location
  sku: {
    name: 'standard'
  }
}

// to insert Key Value Pairs
resource configStoreKeyValue 'Microsoft.AppConfiguration/configurationStores/keyValues@2021-10-01-preview' = [for keyValuePair in keyValuePairs: {
  parent: configStore
  name: keyValuePair.key					// key
  properties: {
    value: keyValuePair.value				// value of the key
    contentType: keyValuePair.contentType	// string representing content type of value
    tags: keyValuePair.tags					// object: Dictionary of tags 
  }
}]

// Select primary readonly access keys
var readonlyKeys = filter(configStore.listKeys().value, k => k.name == 'Primary Read Only')[0]

// Output the connection string
output readonly string = readonlyKeys.connectionString
