import { Box, Button, Grid2 } from '@mui/material'
import CategoriesList from './CategoriesList'
import AddIcon from '@mui/icons-material/Add';
import { Outlet, useNavigate } from 'react-router';
export default function CategoriesDashboard() {
    const navigate = useNavigate();

    const handleCreate = () => {
        navigate("/categories/new");
    }
    return (
        <>
            <Grid2 container spacing={{ xs: 2, sm: 3 }}>
                <Grid2 size={{ xs: 7, sm: 7 }}>
                    <Box sx={{ pb: 5, flexDirection: 'column', justifyContent: 'space-between', display: "flex" }}>
                        <CategoriesList />
                    </Box>
                </Grid2>
                <Grid2 size={{ xs: 5, sm: 5 }}>
                    <Box sx={{ position: "sticky", alignSelf: "flex-start", top: 80 }}>
                        <Button
                            sx={{ width: "fit-content", alignSelf: "end", mb: 2 }}
                            color='success' variant='contained'
                            onClick={() => handleCreate()} ><AddIcon /> Create Category</Button>
                        <Outlet />
                    </Box>
                </Grid2>
            </Grid2>
        </>)
}
