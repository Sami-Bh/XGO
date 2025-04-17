import { useState } from "react";
import useStorageLocations from "../../../lib/hooks/storage/useStorageLocations"
import SelectInput from "../../shared/components/SelectInput";
import { Box, Paper, Typography } from "@mui/material";
import StoredItemsNameFilter from "./StoredItemsNameFilter";

export default function StorageFilter() {
    const { StorageLocations, isStorageLocationsPending } = useStorageLocations();
    const [SelectedStorageLocationId, setSelectedStorageLocationId] = useState<number | "">("");

    if (isStorageLocationsPending) return <>Loading...</>

    return (
        <Paper sx={{ display: "flex", flexDirection: "column", gap: 1 }}>
            <Typography variant="h5">Filters</Typography>
            <Box sx={{ display: "flex", flexDirection: "row", gap: 1 }}>
                <SelectInput
                    name="storageLocationSelect"

                    label="Storage Location"
                    value={SelectedStorageLocationId}
                    options={StorageLocations || []}
                    onChange={(value) => setSelectedStorageLocationId(Number(value))}
                />
                <StoredItemsNameFilter />
            </Box>
        </Paper>
    )
}
