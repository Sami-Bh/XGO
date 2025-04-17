import { useQuery } from "@tanstack/react-query";
import stores from "../../api/agent";
import { storageUri } from "../../../app/routes/routesconsts";
import { StoredItemName } from "../../types/storage";

function useSearchStoredItemsNames(searchText: string) {
    const storageAgent = stores.storageAgent;
    const { data: ProductNames, isPending } = useQuery({
        queryFn: async () => {
            const response = await storageAgent.get<StoredItemName[]>(`${storageUri}/GetStoredItemsNames`, {
                params: {
                    searchText: searchText
                }
            });
            return response.data;
        },
        queryKey: ["GetStoredItemsNames", searchText],
        enabled: !!searchText
    });

    return { ProductNames, isPending }
};
export default useSearchStoredItemsNames;