import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import { productsUri } from "../../app/routes/routesconsts";

export default function useProducts(productsFilter: ProductsFilter) {

    const { data: filteredProductsFromServer, isPending: isGeFilteredtProductsPending } = useQuery({
        queryKey: ["getProducts", productsFilter],
        queryFn: async () => {
            const response = await agent.get<Product[]>(`${productsUri}/GetProductsBySubCategoryId`,
                {
                    params: {
                        categoryId: productsFilter.categoryId,
                        subcategoryId: productsFilter.subcategoryId,
                        searchtext: productsFilter.textSearch ?? ""
                    }
                });
            return response.data;
        },
    });

    return {

        filteredProductsFromServer, isGeFilteredtProductsPending
    }
}