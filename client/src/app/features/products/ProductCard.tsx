import { Box, Card, CardContent, CardMedia, Chip, Typography } from "@mui/material";
import { Link } from "react-router";
import { productsUri } from "../../routes/routesconsts";
type Props = {
    product: Product
}
export default function ProductCard({ product }: Props) {
    if (!product) return <Typography>Loading ...</Typography>
    return (
        <Card>
            <CardContent >
                <Box sx={{ position: "relative", display: "flex", flexDirection: "row" }}>
                    <CardMedia
                        component="img"
                        height="140"
                        image={`https://picsum.photos/200/300?random=${product.id}`}
                        alt="green iguana"
                    />
                    <Box sx={{
                        display: "flex", flexDirection: "column",
                        position: "absolute", top: 2, right: 2, alignSelf: "end"
                    }}>
                        {product.isHeavy && <Chip label="Heavy" variant="filled" color="error" />}
                        {product.isBulky && <Chip label="Bulky" variant="filled" color="warning" />}

                    </Box>

                </Box>
                <Typography component={Link} to={`${productsUri}/${product.id}`} variant="h5">{product.name}</Typography>
            </CardContent>
        </Card>
    )
}
