import { Card, CardContent, Typography, Divider } from "@mui/material";
import { Link } from "react-router";

type Props = {
  subCategory: SubCategory;
};

export default function SubCategoryCard({ subCategory }: Props) {
  return (
    <Card elevation={1}>
      <CardContent>
        <Typography variant="h5" component="div">
          <Link
            to={`/subcategories/${subCategory.categoryId}/${subCategory.id}`}
          >
            {subCategory.name}
          </Link>
        </Typography>
        <Typography gutterBottom sx={{ color: "text.secondary", fontSize: 14 }}>
          "some description goes here"
        </Typography>
      </CardContent>
      <Divider />
    </Card>
  );
}
