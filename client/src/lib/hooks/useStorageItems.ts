import { useQuery } from "@tanstack/react-query";
import storeAgent from "../api/agent";
import { StorageFilter, StoredItem } from "../types/storage";

function useStorageItems(storageFilter?: StorageFilter) {
    const { data: StoredItemsFromServer, isPending: IsStoredItemsFromServerPending } = useQuery({
        queryFn: async () => {
            const response = await storeAgent.get<StoredItem[]>("/StoredItem");
            return response;
        },
        queryKey: ["getStoredItems", storageFilter],
        enabled: !!storageFilter
    });

    return {
        StoredItemsFromServer, IsStoredItemsFromServerPending
    }
}

export default useStorageItems;