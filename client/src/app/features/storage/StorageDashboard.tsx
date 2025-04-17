import { Box, Grid2 } from "@mui/material";
import StorageTable from "./StorageTable";
import StorageFilter from "./StorageFilter";

export default function StorageDashboard() {
    return (
        <Grid2 container>
            <Grid2 size={7}>
                <Box sx={{
                    pb: 5, flexDirection: 'column', justifyContent: 'space-between',
                    display: "flex", gap: 2
                }}>
                    <StorageFilter />
                    <StorageTable />

                </Box>
            </Grid2>
        </Grid2>


    )
}
