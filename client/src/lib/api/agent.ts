import axios, { AxiosInstance } from "axios";
import { router } from "../../app/routes/routes";
import { PUBLIC_CLIENT_APPLICATION, TOKEN_REQUEST } from "../../msalConfig";

const storeAgent = axios.create({
    baseURL: import.meta.env.VITE_API_URL + import.meta.env.VITE_STORE_URI,
});
const storageAgent = axios.create({
    baseURL: import.meta.env.VITE_API_URL + import.meta.env.VITE_STORAGE_URI,
});
UpdateInterceptors([storeAgent, storageAgent]);

export default { storeAgent, storageAgent };

function UpdateInterceptors(axiosInstances: AxiosInstance[]) {
    axiosInstances.forEach(axiosInstance => {
        AddAuthenticationHeaderToAxiosInstance(axiosInstance);
        AddRedirectToNotFoundPage(axiosInstance);
    });
}

function AddAuthenticationHeaderToAxiosInstance(axiosInstance: AxiosInstance) {
    axiosInstance.interceptors.request.use(async (conf) => {

        const token = (await PUBLIC_CLIENT_APPLICATION.acquireTokenSilent(TOKEN_REQUEST)).accessToken;

        conf.headers.Authorization = `Bearer ${token}`;

        return conf;
    });
}

function AddRedirectToNotFoundPage(axionsInstance: AxiosInstance) {
    axionsInstance.interceptors.response.use(
        (response) => {
            return response
        },
        (error) => {
            console.log(error);

            if (error.response.status === 404) {
                router.navigate("/not-found");
            } else {
                return Promise.reject(error)
            }
        }
    );
}