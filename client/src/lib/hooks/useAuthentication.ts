import { LOGIN_REQUEST, TOKEN_REQUEST } from "../../msalConfig";
import { useState } from "react";
import { useAccount, useMsal } from "@azure/msal-react";

function useAuthentication() {
    const { instance, accounts } = useMsal();
    const [Token, setToken] = useState("");
    const LoggedInUser = useAccount(accounts[0])
    const handleSignIn = async () => {
        const loginResponse = await instance.loginPopup(LOGIN_REQUEST);
        if (loginResponse.account) {
            instance.setActiveAccount(loginResponse.account);
        }
        const tokenResponse = await instance.acquireTokenSilent(TOKEN_REQUEST);
        setToken(tokenResponse.accessToken);

    };

    const handleSignOut = async () => {
        // if (!interactionInProgress) {
        //   setInteractionInProgress(true);
        instance.logoutPopup();
        //   setToken(null);
        //   setInteractionInProgress(false);
    };

    const handleRefreshToken = async () => {
        const tokenResponse = await instance.acquireTokenSilent(TOKEN_REQUEST);
        // setToken(tokenResponse.accessToken);
        setToken(tokenResponse.accessToken);
    };


    return {
        handleSignIn,
        handleSignOut,
        handleRefreshToken,
        instance,
        Token,
        LoggedInUser
    }
}
export default useAuthentication;
