import { Box, Grid2, Pagination } from "@mui/material";
import ProductList from "./ProductList";
import { useState } from "react";
import useProducts from "../../../lib/hooks/useProducts";
import ProducstsFilter from "./ProducstsFilter";
export default function ProductDashboard() {



    //use state
    const [ProductsFilter, setProductsFilter] = useState({ categoryId: -1, subcategoryId: -1, textSearch: "" } as ProductsFilter)

    const { filteredProductsFromServer, isGeFilteredtProductsPending } = useProducts(ProductsFilter);
    const updatePageIndex = (newIndex: number) => {
        const newFilter = { ...ProductsFilter, pageIndex: newIndex };
        console.log(newFilter);
        setProductsFilter(newFilter);
    }
    return (

        <Grid2 container spacing={3}>
            <Grid2 size={8}>
                <Box sx={{ display: "flex", flexDirection: "column" }}>
                    <ProductList products={filteredProductsFromServer?.items ?? []} isLoading={isGeFilteredtProductsPending} />
                    <Pagination sx={{ alignSelf: "center", py: 1 }}
                        count={filteredProductsFromServer?.pageCount || 1} color="primary"
                        onChange={(e, value) => updatePageIndex(value)}
                    />

                </Box>
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
