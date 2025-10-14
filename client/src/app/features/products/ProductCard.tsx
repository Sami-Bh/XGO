import { Box, Card, CardContent, CardMedia, Chip, Typography } from "@mui/material";
import { Link } from "react-router";
import { productsUri } from "../../routes/routesconsts";

type Props = {
    product: Product
}

export default function ProductCard({ product }: Props) {
    if (!product) return <Typography>Loading ...</Typography>

    return (
        <Card className="product-card">
            <Box className="product-card-media-container">
                <CardMedia
                    component="img"
                    height="180"
                    image={`https://picsum.photos/300/180?random=${product.id}`}
                    alt={product.name}
                    className="product-card-media"
                />
                <Box className="product-card-badges">
                    {product.isHeavy && (
                        <Chip
                            label="Heavy"
                            size="small"
                            color="error"
                            className="product-card-badge-heavy"
                        />
                    )}
                    {product.isBulky && (
                        <Chip
                            label="Bulky"
                            size="small"
                            color="warning"
                            className="product-card-badge-bulky"
                        />
                    )}
                </Box>
            </Box>

            <CardContent className="product-card-content">
                <Typography
                    component={Link}
                    to={`${productsUri}/${product.id}`}
                    variant="h6"
                    className="product-card-title"
                >
                    {product.name}
                </Typography>
            </CardContent>
        </Card>
    )
}
