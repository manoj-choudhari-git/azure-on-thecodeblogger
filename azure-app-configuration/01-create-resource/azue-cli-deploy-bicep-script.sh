## To create new resource group
az group create --name azure-app-config-demo --location westeurope

## NOTE: Replace <config-store-name> with the actual name
## To deploy bicep file
az deployment group create --name deploy-22-12-23 --resource-group azure-app-config-demo --template-file app-config-store.bicep

## Use deployment name and resource group name to query the outputs of the model
az deployment group show -g azure-app-config-demo -n deploy-22-12-23 --query properties.outputs

## To verify that configuration store resource is deployed
az resource list --resource-group azure-app-config-demo