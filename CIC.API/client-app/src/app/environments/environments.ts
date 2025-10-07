import { EnvironmentConfiguration } from "../models/environment-configuration";

const serverUrl = 'https://localhost:44351/api';
export const environment: EnvironmentConfiguration = {
  env_name: 'dev',
  production: true,
  apiUrl: serverUrl,
  apiEndpoints: {
    userProfile: 'user-profiles'
  },
  adConfig: {
    clientId: 'fc8572b1-78e6-49e8-8fa9-a78fdbe71f7b ',
    readScopeUrl: 'api://ae208836-1c5c-4d13-8dfb-76c177208bc7/CIC.API',
    writeScopeUrl: 'api://ae208836-1c5c-4d13-8dfb-76c177208bc7/CIC.API.Write',
    scopeUrls: [
      'api://ae208836-1c5c-4d13-8dfb-76c177208bc7/CIC.API',
      'api://ae208836-1c5c-4d13-8dfb-76c177208bc7/CIC.API.Write'
    ],
    apiEndpointUrl: 'https://localhost:44351/api',
    tenantId: "4754a0e4-811f-4601-aa07-f3c4c9d8ac75"
  },
  cacheTimeInMinutes: 30,
};