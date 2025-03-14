import { Card, CardContent, Divider, Typography } from "@mui/material";

import { Link } from "react-router";
type Props = {
    category: Category
}
export default function CategoryCard({ category }: Props) {

    return (
        <Card elevation={1} >
            {/* <CardActionArea onClick={() => {
                handleSelect();
            }}> */}
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


        </Card >
    )
}
