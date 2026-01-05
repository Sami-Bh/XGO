import { Grid2, Box, Button } from "@mui/material";
import { Outlet, useNavigate, useParams } from "react-router";
import SubCategoryList from "./SubCategoryList";
import AddIcon from "@mui/icons-material/Add";

export default function SubCategoryDashboard() {
  const navigate = useNavigate();
  const { categoryId } = useParams();

  const handleCreate = () => {
    navigate(`/subcategories/${categoryId}/new`);
  };
  return (
    <>
      <Grid2 container spacing={{ xs: 1, sm: 3 }}>
        <Grid2 size={{ xs: 7, sm: 7 }}>
          <Box
            sx={{
              pb: 5,
              flexDirection: "column",
              justifyContent: "space-between",
              display: "flex",
            }}
          >
            <SubCategoryList />
          </Box>
        </Grid2>
        <Grid2 size={{ xs: 5, sm: 5 }}>
          <Box sx={{ position: "sticky", alignSelf: "flex-start", top: 80 }}>
            <Button
              sx={{ width: "fit-content", alignSelf: "end", mb: 2 }}
              color="success"
              variant="contained"
              onClick={() => handleCreate()}
            >
              <AddIcon /> Create Sub Category
            </Button>
            <Outlet />
          </Box>
        </Grid2>
      </Grid2>
    </>
  );
}
