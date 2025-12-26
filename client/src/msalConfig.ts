import { PublicClientApplication } from "@azure/msal-browser";
import { MSAL_API_SCOPE, MSAL_Authority, MSAL_ClientId, MSAL_REDIRECT_URI } from "./msalKeys";

const MSAL_CONFIG = {
    auth: {
        clientId: MSAL_ClientId,
        authority: MSAL_Authority,
        redirectUri: MSAL_REDIRECT_URI,
    },
    cache: {
        cacheLocation: "localStorage",
        storeAuthStateInCookie: false,
    },
}

const LOGIN_REQUEST = {
    scopes: ["openid", "offline_access"]
};

const TOKEN_REQUEST = {
    scopes: [MSAL_API_SCOPE]
};



const PUBLIC_CLIENT_APPLICATION = new PublicClientApplication(MSAL_CONFIG);

async function initializeMsal() {
    await PUBLIC_CLIENT_APPLICATION.initialize();
}

initializeMsal();

export {
    PUBLIC_CLIENT_APPLICATION,
    MSAL_CONFIG,
    LOGIN_REQUEST,
    TOKEN_REQUEST,
}