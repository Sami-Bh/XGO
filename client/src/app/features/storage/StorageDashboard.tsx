import { Box, Grid2 } from "@mui/material";
import StorageTable from "./StorageTable";
import StorageFilter from "./StorageFilter";
import { Outlet } from "react-router";
import StorageActions from "./StorageActions";
import UpdatedItemsTable from "./UpdatedItemsTable";

export default function StorageDashboard() {
  return (
    <Grid2 container spacing={3}>
      <Grid2 size={7}>
        <Box
          sx={{
            pb: 5,
            flexDirection: "column",
            justifyContent: "space-between",
            display: "flex",
            gap: 2,
          }}
        >
          <StorageFilter />
          <StorageTable />
        </Box>
      </Grid2>
      <Grid2 size={5}>
        {/* position: "sticky", */}
        <Box sx={{ top: 80, display: "flex", flexDirection: "column", gap: 2 }}>
          <UpdatedItemsTable />
          <StorageActions />
        </Box>
        <Box sx={{ mt: 2 }}>
          <Outlet />
        </Box>
      </Grid2>
    </Grid2>
  );
}
