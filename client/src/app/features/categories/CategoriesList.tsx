import { Box, Typography } from "@mui/material";
import CategoryCard from "./CategoryCard";
import useCategories from "../../../lib/hooks/store/useCategories";

export default function CategoriesList() {
    const { categoriesFromServer, isGettingCategoriesPending } = useCategories();


    if (!categoriesFromServer || isGettingCategoriesPending) return (<Typography variant="h2">Loading</Typography>)

    return (

        <Box sx={{

            display: "flex", flexDirection: "column",
            gap: 2, overflowY: "auto"
        }}>
            {categoriesFromServer.map(category => (
                <CategoryCard key={category.id} category={category} />
            ))}
        </Box>
    )
}
