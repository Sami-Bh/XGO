import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import { productsUri } from "../../app/routes/routesconsts";

export default function useProducts(categoryId?: number, subCategoryId?: number, searchtext?: string) {
    const { data: productsFromServer, isPending: isGetProductsPending } = useQuery({
        queryKey: ["getProducts"],
        queryFn: async () => {
            const response = await agent.get<Product[]>(`${productsUri}`);
            return response.data;
        },

    });

    const { data: filteredProductsFromServer, isPending: isGeFilteredtProductsPending } = useQuery({
        queryKey: ["getProducts", categoryId, subCategoryId],
        queryFn: async () => {
            const response = await agent.get<Product[]>(`${productsUri}/GetProductsBySubCategoryId`,
                {
                    params: {
                        categoryId: categoryId,
                        subcategoryId: subCategoryId,
                        searchtext: searchtext
                    }
                });
            return response.data;
        },
    });

    return {
        productsFromServer, isGetProductsPending,
        filteredProductsFromServer, isGeFilteredtProductsPending
    }
}

export async function GetFilterdProducts(categoryId?: number, subCategoryId?: number, searchtext?: string) {
    const response = await agent.get<Product[]>(`${productsUri}/GetProductsBySubCategoryId`,
        {
            params: {
                categoryId: categoryId,
                subcategoryId: subCategoryId,
                SearchText: searchtext ?? ""
            }
        });
    return response.data;
}