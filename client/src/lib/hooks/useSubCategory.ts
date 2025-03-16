import { useQuery } from "@tanstack/react-query";
import agent from "../api/agent";

function useSubCategory(categoryId?: number, subcategoryId?: number) {
    const { isPending: isGetSubCategoriesPending, data: subcategoriesFromServer } = useQuery({
        queryKey: ["getSubCategories"],
        queryFn: async () => {
            const response = await agent.get<SubCategory[]>(`/SubCategories/GetSubcategoriesListByCategoryId/${categoryId}`);
            return response.data;
        },
        enabled: !!categoryId
    });
    return { isGetSubCategoriesPending, subcategoriesFromServer }
}

export default useSubCategory;