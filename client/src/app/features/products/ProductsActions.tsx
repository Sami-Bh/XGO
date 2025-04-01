import { Box, Button, Card, CardContent, Typography } from '@mui/material'
import { useNavigate } from 'react-router'
import { productsUri } from '../../routes/routesconsts';
import AddToQueueIcon from '@mui/icons-material/AddToQueue';
import AddIcon from '@mui/icons-material/Add';
export default function ProductsActions() {
    const navigate = useNavigate();
    return (
        <Card sx={{ mb: 1 }}>
            <CardContent>
                <Box sx={{ display: "flex", flexDirection: "column", mx: 5, pb: 1, gap: 2 }}>
                    <Typography color="primary" sx={{ alignSelf: "center" }} variant="h5"><AddToQueueIcon /> Actions</Typography>

                    <Button sx={{ py: 2 }} endIcon={<AddIcon />} size='small' variant="contained" color="success" onClick={() => navigate(`${productsUri}/new`)}>Create product</Button>
                </Box>
            </CardContent>

        </Card>
    )
}
