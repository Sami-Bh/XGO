import { Box, Card, CardContent, CardMedia, Chip, Typography } from "@mui/material";
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
                        image="https://images.pexels.com/photos/416160/pexels-photo-416160.jpeg"
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
                <Typography variant="h5">{product.name}</Typography>
            </CardContent>
        </Card>
    )
}
