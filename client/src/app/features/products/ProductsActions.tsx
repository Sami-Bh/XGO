import { Box, Button, Paper, Typography } from "@mui/material";
import { useNavigate } from "react-router";
import { productsUri } from "../../routes/routesconsts";
import AddToQueueIcon from "@mui/icons-material/AddToQueue";
import AddIcon from "@mui/icons-material/Add";
import {
  actionIconFontSize,
  actionTitleStyle,
  actionsBoxStyle,
  filterButtonStyle,
} from "../../shared/actionStyles";
export default function ProductsActions() {
  const navigate = useNavigate();
  return (
    <Paper sx={{ gap: 2, my: { xs: 1, sm: 2, md: 5 } }}>
      <Box sx={actionsBoxStyle}>
        <Typography color="primary" sx={actionTitleStyle} variant="h5">
          <AddToQueueIcon sx={{ fontSize: actionIconFontSize }} />
          Actions
        </Typography>

        <Button
          sx={filterButtonStyle}
          endIcon={<AddIcon />}
          variant="contained"
          color="success"
          onClick={() => navigate(`${productsUri}/new`)}
        >
          Create product
        </Button>
      </Box>
    </Paper>
  );
}
