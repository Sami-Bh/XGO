import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import storeAgent from "../api/agent";
import { categoriesUri } from "../../app/routes/routesconsts";

function useCategories(id?: number) {
    const queryClient = useQueryClient();
    const { data: categoriesFromServer, isLoading: isGettingCategoriesPending } = useQuery({
        queryKey: ["GetCategories"],
        queryFn: async () => {
            const response = await storeAgent.get<Category[]>('/Categories');
            return response.data;
        },
    });

    const { data: categoryFromServer, isLoading: isGettingCategoryLoading } = useQuery({
        queryKey: ["GetCategories", id],
        queryFn: async () => {
            const response = await storeAgent.get<Category>(`/Categories/${id}`);

            return response.data;
        },
        enabled: !!id,
        // gcTime: 0,
    });

    const updateCategory = useMutation({
        mutationFn: async (category: Category) => {
            await storeAgent.put(categoriesUri, category);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetCategories"] });
        }
    });

    const createCategory = useMutation({
        mutationFn: async (category: Category) => {
            const response = await storeAgent.post(categoriesUri, category);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetCategories"] });
        }
    });

    const deleteCategory = useMutation({
        mutationFn: async (id: number) => {
            await storeAgent.delete(`${categoriesUri}/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetCategories"], exact: true });
        },
        onError: (error) => {
            console.log(error);
        }
    });

    return {
        categoriesFromServer,
        isGettingCategoriesPending,
        categoryFromServer,
        isGettingCategoryLoading,
        updateCategory,
        deleteCategory,
        createCategory
    }
}

export default useCategories;