import { Box, Container, Grid2, Pagination } from "@mui/material";
import ProductList from "./ProductList";
import { useState } from "react";
import useProducts from "../../../lib/hooks/useProducts";
import ProducstsFilter from "./ProducstsFilter";
import ProductsActions from "./ProductsActions";
export default function ProductDashboard() {




    //use state
    const [ProductsFilter, setProductsFilter] = useState({ categoryId: -1, subcategoryId: -1, textSearch: "", pageIndex: 1 } as ProductsFilter)

    const { filteredProductsFromServer, isGeFilteredtProductsPending } = useProducts(ProductsFilter);
    const updatePageIndex = (newIndex: number) => {
        setProductsFilter({ ...ProductsFilter, pageIndex: newIndex });
    }
    return (

        <Grid2 container spacing={3}>
            <Grid2 size={8}>
                <Container >
                    <Box sx={{ display: "flex", flexDirection: "column", overflowY: "auto", Height: 700 }}>
                        <ProductList products={filteredProductsFromServer?.items ?? []} isLoading={isGeFilteredtProductsPending} />
                        <Pagination sx={{ alignSelf: "center", py: 1 }}
                            count={filteredProductsFromServer?.pageCount || 1} color="primary"
                            page={ProductsFilter.pageIndex}
                            onChange={(_, value) => updatePageIndex(value)}
                        />

                    </Box>
                </Container>
            </Grid2>
            <Grid2 size={4}>
                <Box sx={{
                    position: "sticky", alignSelf: "flex-start", top: 80
                }}>
                    <ProductsActions />

                    <ProducstsFilter setFilter={setProductsFilter} />
                </Box>
            </Grid2>
        </Grid2 >
    )
}
