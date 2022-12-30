
async function run() {
  
  let retrievedSetting = await client.getConfigurationSetting({
    key: "TestApi:PostEndpoint:Message"
  });

  console.log("Retrieved value:", retrievedSetting.value);
}

const appConfig = require("@azure/app-configuration");
const connection_string = process.env.AZURE_APP_CONFIG_CONNECTION_STRING;
const client = new appConfig.AppConfigurationClient(connection_string);

run().catch((err) => console.log("ERROR:", err));