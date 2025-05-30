import { Paper, Button } from "@mui/material";
import { useNavigate } from "react-router";
import AddIcon from '@mui/icons-material/Add';

export default function StorageActions() {
    const navigate = useNavigate();

    return (
        <Paper sx={{ p: 2 }}>
            <Button
                fullWidth
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                onClick={() => navigate('new')}
                sx={{ justifyContent: "flex-start" }}
            >
                Add Item
            </Button>
        </Paper>
    );
} 