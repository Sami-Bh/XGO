import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import stores from "../../api/agent";
import { StorageFilter, StoredItem } from "../../types/storage";
import { storageUri } from "../../../app/routes/routesconsts";

function useStorageItems(storageFilter?: StorageFilter) {
    const storageAgent = stores.storageAgent;
    const queryClient = useQueryClient();

    const { data: StoredItemsFromServer, isPending: IsStoredItemsFromServerPending } = useQuery({
        queryFn: async () => {
            const response = await storageAgent.get<ListedItem<StoredItem>>(`${storageUri}/GetFilteredStoredItems`, {
                params: {
                    orderby: storageFilter?.orderby,
                    oderDirection: storageFilter?.oderDirection,
                    pageIndex: storageFilter?.pageIndex,
                    pageSize: storageFilter?.pageSize,
                    productNameSearchText: storageFilter?.productNameSearchText,
                    StorageId: storageFilter?.StorageId
                }
            });
            return response.data;
        },
        queryKey: ["getStoredItems", storageFilter],
        enabled: !!storageFilter
    });

    const createStoredItem = useMutation({
        mutationFn: async (item: Omit<StoredItem, "id">) => {
            const response = await storageAgent.post<StoredItem>(storageUri, item);
            return response.data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["getStoredItems"] });
        }
    });

    return {
        StoredItemsFromServer,
        IsStoredItemsFromServerPending,
        createStoredItem
    }
}

export default useStorageItems;