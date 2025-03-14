import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";

function useCategories(id?: number) {
    const queryClient = useQueryClient();
    const { data: categoriesFromServer, isPending: isGettingCategoriesPending } = useQuery({
        queryKey: ["GetCategories"],
        queryFn: async () => {
            const response = await agent.get<Category[]>('/Categories');
            return response.data;
        },
    });

    const { data: categoryFromServer, isLoading: isGettingCategoryLoading } = useQuery({
        queryKey: ["GetCategories", id],
        queryFn: async () => {
            const response = await agent.get<Category>(`/Categories/${id}`);

            return response.data;
        },
        enabled: !!id,
        // gcTime: 0,
    });

    const updateCategory = useMutation({
        mutationFn: async (category: Category) => {
            await agent.put("/Categories", category);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetCategories"] });
        }
    });

    const createCategory = useMutation({
        mutationFn: async (category: Category) => {
            await agent.post("/Categories", category);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetCategories"] });
        }
    });

    const deleteCategory = useMutation({
        mutationFn: async (id: number) => {
            await agent.delete(`/Categories/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetCategories"] });
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