import { Paper, Button } from "@mui/material";
import { useNavigate } from "react-router";
import AddIcon from "@mui/icons-material/Add";
import { filterButtonStyle } from "../../shared/actionStyles";

export default function StorageActions() {
  const navigate = useNavigate();

  return (
    <Paper sx={{ p: 1 }}>
      <Button
        sx={filterButtonStyle}
        fullWidth
        variant="contained"
        color="primary"
        endIcon={<AddIcon />}
        onClick={() => navigate("new")}
        // sx={{ justifyContent: "flex-start" }}
      >
        Add Item
      </Button>
    </Paper>
  );
}
