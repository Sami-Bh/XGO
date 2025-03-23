import { Box, Typography } from "@mui/material";
import ProductCard from "./ProductCard";

type Props = {
    products: Product[],
    isLoading: boolean,
}
export default function ProductList({ products, isLoading }: Props) {

    if (isLoading || !products) return <Typography>Loading ...</Typography>

    return (
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2, overflowY: "auto" }}>
            {products?.map(product => {
                return <ProductCard key={product.id} product={product} />
            })}
        </Box>
    )
}
