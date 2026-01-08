import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import stores from "../../api/agent";
import { locationUri } from "../../../app/routes/routesconsts";
import { StorageLocation } from "../../types/storage";

function useLocations() {
    const storageAgent = stores.storageAgent;
    const queryClient = useQueryClient();

    const { data: locationsFromServer, isLoading: isGettingLocationsPending } = useQuery({
        queryKey: ["GetLocations"],
        queryFn: async () => {
            const response = await storageAgent.get<StorageLocation[]>(locationUri);
            return response.data;
        }
    });

    const addLocation = useMutation({
        mutationFn: async (location: StorageLocation) => {
            await storageAgent.post(locationUri, location);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetLocations"] })
        }
    });
    const updateLocation = useMutation({
        mutationFn: async (location: StorageLocation) => {
            await storageAgent.put(locationUri, location);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetLocations"] });
        }
    });

    const deleteLocation = useMutation({
        mutationFn: async (id: number) => {
            await storageAgent.delete(`${locationUri}/${id}`);
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["GetLocations"], exact: true });
        },
    });

    return {
        locationsFromServer,
        isGettingLocationsPending,
        updateLocation,
        deleteLocation,
        addLocation
    }
}

export default useLocations;