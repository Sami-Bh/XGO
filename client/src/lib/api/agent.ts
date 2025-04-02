import axios from "axios";
import { router } from "../../app/routes/routes";
const storeAgent = axios.create({
    baseURL: import.meta.env.VITE_API_URL + import.meta.env.VITE_STORE_URI
});

storeAgent.interceptors.request.use((conf) => {
    return conf;
})

storeAgent.interceptors.response.use(
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
export default storeAgent;