import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { productsUri } from "../../app/routes/routesconsts";

export default function useProducts(productsFilter?: ProductsFilter, productId?: number) {
    const queryClient = useQueryClient();
    const { data: filteredProductsFromServer, isPending: isGeFilteredtProductsPending } = useQuery({
        queryKey: ["getProducts", productsFilter],
        queryFn: async () => {
            const response = await agent.get<ListedItem<Product>>(`${productsUri}/GetProductsBySubCategoryId`,
                {
                    params: {
                        categoryId: productsFilter!.categoryId,
                        subcategoryId: productsFilter!.subcategoryId,
                        searchtext: productsFilter!.textSearch ?? "",
                        pageSize: productsFilter!.pageSize,
                        pageIndex: productsFilter!.pageIndex,
                    }
                });
            return response.data;
        },
        enabled: !!productsFilter
    });

    const { data: ProductFromServer, isPending: IsGetProductPending } = useQuery({
        queryKey: ["getProducts", productId],
        queryFn: async () => {
            const response = await agent.get<Product>(`${productsUri}/${productId}`);
            return response.data;
        },
        enabled: !!productId
    });
    const updateProduct = useMutation({
        mutationKey: ["getProducts", productId],
        mutationFn: async (product: Product) => {
            await agent.put<Product>(`${productsUri}`, product);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getProducts"] });
        }
    });

    const deleteProduct = useMutation({
        mutationKey: ["getProducts"],
        mutationFn: async (id: number) => {
            await agent.delete<Product>(`${productsUri}/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getProducts"] });
        }
    });

    const createProduct = useMutation({
        mutationKey: ["getProducts"],
        mutationFn: async (product: Product) => {
            const response = await agent.post<Product>(`${productsUri}`, product);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getProducts"] });
        }
    })
    return {

        filteredProductsFromServer, isGeFilteredtProductsPending,
        ProductFromServer, IsGetProductPending,
        createProduct,
        updateProduct,
        deleteProduct,
    }
}