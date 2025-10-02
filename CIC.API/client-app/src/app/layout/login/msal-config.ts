import { PublicClientApplication, Configuration,InteractionType  } from '@azure/msal-browser';


export const msalConfig: Configuration = {
  auth: {
    clientId: 'fc8572b1-78e6-49e8-8fa9-a78fdbe71f7b',
    authority: 'https://login.microsoftonline.com/4754a0e4-811f-4601-aa07-f3c4c9d8ac75',
    redirectUri: 'http://localhost:4200/',
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: false
  }
};

export const msalInstance = new PublicClientApplication(msalConfig);

// 👇 Must call this before using msalInstance
export const msalInitialized = msalInstance.initialize();

export const protectedResources ={
  graphMe :{
    endpoint: 'https://graph.microsoft.com/v1.0/me',
    scopes: ['User.Read'],
  }
}


