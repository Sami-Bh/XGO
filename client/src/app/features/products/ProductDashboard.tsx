import { Box, Grid2 } from "@mui/material";
import ProductList from "./ProductList";
import { useState } from "react";
import useProducts from "../../../lib/hooks/useProducts";
import ProducstsFilter from "./ProducstsFilter";
export default function ProductDashboard() {



    //use state
    const [ProductsFilter, setProductsFilter] = useState({ categoryId: -1, subcategoryId: -1, textSearch: "" } as ProductsFilter)

    const { filteredProductsFromServer, isGeFilteredtProductsPending } = useProducts(ProductsFilter);

    return (
        <Grid2 container spacing={3}>
            <Grid2 size={8}>

                <ProductList products={filteredProductsFromServer ?? []} isLoading={isGeFilteredtProductsPending} />
            </Grid2>
            <Grid2 size={4}>
                <Box sx={{
                    position: "sticky", alignSelf: "flex-start", top: 80
                }}>
                    <ProducstsFilter setFilter={setProductsFilter} />
                </Box>
            </Grid2>
        </Grid2 >
    )
}
