import axios from "axios";

const agent = axios.create({
    baseURL: import.meta.env.VITE_API_URL
});

agent.interceptors.request.use((conf) => {
    return conf;
})
export default agent;