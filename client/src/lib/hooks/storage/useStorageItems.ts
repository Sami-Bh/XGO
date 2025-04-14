import { useQuery } from "@tanstack/react-query";
import stores from "../../api/agent";
import { StorageFilter, StoredItem } from "../../types/storage";

function useStorageItems(storageFilter?: StorageFilter) {
    const storageAgent = stores.storageAgent;

    const { data: StoredItemsFromServer, isPending: IsStoredItemsFromServerPending } = useQuery({
        queryFn: async () => {
            const response = await storageAgent.get<ListedItem<StoredItem>>("/StoredItem/GetFilteredStoredItems", {
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

    return {
        StoredItemsFromServer, IsStoredItemsFromServerPending
    }
}

export default useStorageItems;