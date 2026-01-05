import { useContext, useState } from "react";
import useStorageLocations from "../../../lib/hooks/storage/useStorageLocations";
import SelectInput from "../../shared/components/SelectInput";
import { Box, Paper, Typography } from "@mui/material";
import StoredItemsNameFilter from "./StoredItemsNameFilter";
import { GetNumberOrUndefined } from "../../../lib/Utils/NumberUtils";
import { StoreContext } from "../../../lib/stores/store";
import { filterSelectStyle } from "../../shared/actionStyles";

export default function StorageFilter() {
  const { StorageLocations, isStorageLocationsPending } = useStorageLocations();
  const [SelectedStorageLocationId, setSelectedStorageLocationId] = useState<
    number | ""
  >("");
  const store = useContext(StoreContext);

  if (isStorageLocationsPending) return <>Loading...</>;

  const UpdateStore = (value: "" | number) => {
    store.storedItemsFilterStore.setSelectedStorageId(
      GetNumberOrUndefined(value)
    );
  };
  return (
    <Paper sx={{ display: "flex", flexDirection: "column", gap: 1, p: 2 }}>
      <Typography variant="h5">Filters</Typography>
      <Box sx={{ display: "flex", flexDirection: "row", gap: 1 }}>
        <SelectInput
          name="storageLocationSelect"
          sx={filterSelectStyle}
          label="Storage Location"
          value={SelectedStorageLocationId}
          options={StorageLocations || []}
          showDeleteOption={true} // Enable the delete option
          onChange={(value) => {
            setSelectedStorageLocationId(value);
            UpdateStore(value);
          }}
        />
        <StoredItemsNameFilter />
      </Box>
    </Paper>
  );
}
