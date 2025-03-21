import { Box, Grid2 } from "@mui/material";
import ProductList from "./ProductList";

export default function ProductDashboard() {
    return (
        <Grid2 container spacing={3}>
            <Grid2 size={8}><ProductList /></Grid2>
            <Grid2 size={4}>filters</Grid2>
        </Grid2>
    )
}
