import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { subcategoriesUri } from "../../app/routes/routesconsts";

function useSubCategory(categoryId?: number, subcategoryId?: number) {
    const queryClient = useQueryClient();
    const { isLoading: isGetSubCategoriesPending, data: subcategoriesFromServer } = useQuery({
        queryKey: ["getSubCategories"],
        queryFn: async () => {
            console.log("get sub for id " + categoryId);

            const response = await agent.get<SubCategory[]>(`${subcategoriesUri}/GetSubcategoriesListByCategoryId/${categoryId}`);
            return response.data;
        },
        enabled: !!categoryId && categoryId > 0,

    });


    const { data: subcategoryFromServer, isLoading: isGetSubCategoryLoading } = useQuery({
        queryKey: ["getSubCategories", { id: subcategoryId }],
        queryFn: async () => {
            const repsponse = await agent.get<SubCategory>(`${subcategoriesUri}/${subcategoryId}`);
            return repsponse.data;
        },
        enabled: !!subcategoryId,
    });


    const createNewSubCategory = useMutation({
        mutationFn: async (subcategory: SubCategory) => {
            const response = await agent.post<SubCategory>(subcategoriesUri, subcategory);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getSubCategories"] });
        }
    })
    const updateSubCategory = useMutation({
        mutationFn: async (subcategory: SubCategory) => {
            await agent.put(subcategoriesUri, subcategory);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getSubCategories"], exact: true });
        }
    })

    const deleteSubCategory = useMutation({
        mutationFn: async (id: number) => {
            await agent.delete(`${subcategoriesUri}/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getSubCategories"], exact: true });
        },

    })

    return {
        isGetSubCategoriesPending, subcategoriesFromServer,
        subcategoryFromServer, isGetSubCategoryLoading,
        createNewSubCategory,
        updateSubCategory,
        deleteSubCategory
    }
}

export default useSubCategory;