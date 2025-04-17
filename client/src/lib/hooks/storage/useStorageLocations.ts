import { useQuery } from "@tanstack/react-query";
import stores from "../../api/agent";
import { StorageLocation } from "../../types/storage";

function useStorageLocations() {
    const storageAgent = stores.storageAgent;
    const { data: StorageLocations, isPending: isStorageLocationsPending } = useQuery({
        queryKey: ["GetStorages"],
        queryFn: async () => {
            const response = await storageAgent.get<StorageLocation[]>("/StorageLocation");
            return response.data;
        }
    });

    return { StorageLocations, isStorageLocationsPending };
}

export default useStorageLocations;