import { Box, Button, Card, CardContent, Divider, Typography } from "@mui/material";

import { Link, useNavigate } from "react-router";
type Props = {
    category: Category
}
export default function CategoryCard({ category }: Props) {
    const navigate = useNavigate();
    const handleSeeSubCategories = () => {
        navigate(`/subcategories/${category.id}`);
    }
    return (
        <Card elevation={1} >
            <CardContent>
                <Typography variant="h4" component="div"><Link
                    to={`/categories/${category.id}`}>{category.name}
                </Link>
                </Typography>
                <Typography gutterBottom sx={{ color: 'text.secondary', fontSize: 14 }}>
                    "some description goes here"
                </Typography>

            </CardContent>

            <Divider />
            <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "flex-end" }}>
                <Button onClick={() => handleSeeSubCategories()} sx={{ my: 1, mx: 1 }} variant="contained">See sub categories</Button>
            </Box>
        </Card >
    )
}
