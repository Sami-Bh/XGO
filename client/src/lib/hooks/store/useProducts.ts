import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import stores from "../../api/agent";
import { productsUri } from "../../../app/routes/routesconsts";
import { Product, ProductInfo, ListedItem, ProductsFilter } from "../../../types/product.types";

export default function useProducts(productsFilter?: ProductsFilter, productId?: number, subCategoryId?: number) {
    const storeAgent = stores.storeAgent;
    const queryClient = useQueryClient();

    const { data: productNamesFromServer, isPending: isGetProductNamesPending } = useQuery({
        queryKey: ["getProductNames", subCategoryId],
        queryFn: async () => {
            const response = await storeAgent.get<ProductInfo[]>(`${productsUri}/GetProductNamesBySubCategory/${subCategoryId}`);
            return response.data;
        },
        enabled: !!subCategoryId
    });

    const { data: filteredProductsFromServer, isPending: isGetFilteredProductsPending } = useQuery({
        queryKey: ["getFilteredProducts", productsFilter],
        queryFn: async () => {
            const response = await storeAgent.get<ListedItem<Product>>(`${productsUri}/GetProductsBySubCategoryId`, {
                params: {
                    pageIndex: productsFilter?.pageIndex,
                    pageSize: productsFilter?.pageSize,
                    searchText: productsFilter?.textSearch,
                    categoryId: productsFilter?.categoryId,
                    subcategoryId: productsFilter?.subcategoryId
                }
            });
            return response.data;
        },
        enabled: !!productsFilter
    });

    const { data: ProductFromServer, isPending: IsGetProductPending } = useQuery({
        queryKey: ["getProducts", productId],
        queryFn: async () => {
            const response = await storeAgent.get<Product>(`${productsUri}/${productId}`);
            return response.data;
        },
        enabled: !!productId
    });
    const updateProduct = useMutation({
        mutationKey: ["getProducts", productId],
        mutationFn: async (product: Product) => {
            await storeAgent.put<Product>(`${productsUri}`, product);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getProducts"] });
        }
    });

    const deleteProduct = useMutation({
        mutationKey: ["getProducts"],
        mutationFn: async (id: number) => {
            await storeAgent.delete<Product>(`${productsUri}/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getProducts", productsFilter] });
        }
    });

    const createProduct = useMutation({
        mutationKey: ["getProducts"],
        mutationFn: async (product: Product) => {
            const response = await storeAgent.post(`${productsUri}`, product);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getProducts"] });
        }
    })
    return {
        productNamesFromServer, isGetProductNamesPending,
        filteredProductsFromServer, isGetFilteredProductsPending,
        ProductFromServer, IsGetProductPending,
        createProduct,
        updateProduct,
        deleteProduct,
    }
}