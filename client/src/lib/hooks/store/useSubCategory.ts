import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import stores from "../../api/agent";
import { subcategoriesUri } from "../../../app/routes/routesconsts";

function useSubCategory(categoryId?: number, subcategoryId?: number) {
    const storeAgent = stores.storeAgent;

    const queryClient = useQueryClient();
    const { isLoading: isGetSubCategoriesPending, data: subcategoriesFromServer } = useQuery({
        queryKey: ["getSubCategories", categoryId],
        queryFn: async () => {
            const response = await storeAgent.get<SubCategory[]>(`${subcategoriesUri}/GetSubcategoriesListByCategoryId/${categoryId}`);
            return response.data;
        },
        enabled: !!categoryId,

    });


    const { data: subcategoryFromServer, isLoading: isGetSubCategoryLoading } = useQuery({
        queryKey: ["getSubCategories", { id: subcategoryId }],
        queryFn: async () => {
            const repsponse = await storeAgent.get<SubCategory>(`${subcategoriesUri}/${subcategoryId}`);
            return repsponse.data;
        },
        enabled: !!subcategoryId,
    });


    const createNewSubCategory = useMutation({
        mutationFn: async (subcategory: SubCategory) => {
            const response = await storeAgent.post<SubCategory>(subcategoriesUri, subcategory);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getSubCategories"] });
        }
    })
    const updateSubCategory = useMutation({
        mutationFn: async (subcategory: SubCategory) => {
            await storeAgent.put(subcategoriesUri, subcategory);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getSubCategories"], exact: true });
        }
    })

    const deleteSubCategory = useMutation({
        mutationFn: async (id: number) => {
            await storeAgent.delete(`${subcategoriesUri}/${id}`);
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