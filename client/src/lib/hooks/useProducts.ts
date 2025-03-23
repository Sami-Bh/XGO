import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import { productsUri } from "../../app/routes/routesconsts";

export default function useProducts(subCategoryId?: number) {
    const { data: productsFromServer, isPending: isGetProductsPending } = useQuery({
        queryKey: ["getProducts"],
        queryFn: async () => {
            const response = await agent.get<Product[]>(`${productsUri}`);
            return response.data;
        }
    });

    return { productsFromServer, isGetProductsPending }
}