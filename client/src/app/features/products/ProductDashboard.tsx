import { Box, Grid2 } from "@mui/material";
import ProductList from "./ProductList";
import { useEffect, useState } from "react";
import { GetFilterdProducts } from "../../../lib/hooks/useProducts";
import ProducstsFilter from "./ProducstsFilter";
export default function ProductDashboard() {



    //use state
    const [ProductsFilter, setProductsFilter] = useState({ categoryId: -1, subcategoryId: -1, textSearch: "" } as ProductsFilter)
    const [Products, setProducts] = useState<Product[] | undefined>([]);


    // const { filteredProductsFromServer, isGeFilteredtProductsPending } = useProducts(SelectedCategoryId || undefined, SelectedSubcategoryId, SearchValue, EnableSearchProduct);






    async function GetProductsDataAsync(productsFilter: ProductsFilter) {

        const products = await GetFilterdProducts(productsFilter.categoryId || undefined, productsFilter.subcategoryId, productsFilter.textSearch);
        setProducts(products);
    }

    useEffect(() => {
        GetProductsDataAsync(ProductsFilter);

    }, [ProductsFilter]);


    return (
        <Grid2 container spacing={3}>
            <Grid2 size={8}>

                <ProductList products={Products ?? []} />
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
