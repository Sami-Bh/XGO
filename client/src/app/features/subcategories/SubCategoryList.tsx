import { useParams } from 'react-router';
import SubCategoryCard from './SubCategoryCard';
import { Typography, Box } from '@mui/material';
import useSubCategory from '../../../lib/hooks/store/useSubCategory';

export default function SubCategoryList() {
  const { categoryId } = useParams();
  const { isGetSubCategoriesPending, subcategoriesFromServer } = useSubCategory(Number(categoryId));

  if (!subcategoriesFromServer || isGetSubCategoriesPending) return (<Typography variant="h2">Loading</Typography>)

  return (

    <Box sx={{

      display: "flex", flexDirection: "column",
      gap: 2, overflowY: "auto"
    }}>
      {subcategoriesFromServer.map(subcategory => (
        <SubCategoryCard key={subcategory.id} subCategory={subcategory} />
      ))}
    </Box>
  )
}
