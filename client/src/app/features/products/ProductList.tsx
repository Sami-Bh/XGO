import { Box, Typography } from "@mui/material";
import ProductCard from "./ProductCard";

type Props = {
    products: Product[],
    isLoading: boolean,
}
export default function ProductList({ products, isLoading }: Props) {
    if (isLoading) return <Typography>Loading...</Typography>

    if (!products || products.length === 0) return <Typography>Nothing found...</Typography>

    return (
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2, overflowY: "auto" }}>
            {products?.map(product => {
                return <ProductCard key={product.id} product={product} />
            })}
        </Box>
    )
}
