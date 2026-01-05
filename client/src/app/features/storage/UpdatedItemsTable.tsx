import { observer } from "mobx-react-lite";
import { useContext, useState } from "react";
import { StoreContext } from "../../../lib/stores/store";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  Button,
  Box,
  Alert,
  Snackbar,
} from "@mui/material";
import dayjs from "dayjs";
import DeleteIcon from "@mui/icons-material/Delete";
import SaveIcon from "@mui/icons-material/Save";
import ClearIcon from "@mui/icons-material/Clear";
import { StoredItem } from "../../../lib/types/storage";
import useStorageItems from "../../../lib/hooks/storage/useStorageItems";
import { filterButtonStyle } from "../../shared/actionStyles";

const UpdatedItemsTable = observer(() => {
  const { updatedStorageItemsStore } = useContext(StoreContext);
  const items = updatedStorageItemsStore.getUpdatedItems;
  const { updateStoredItems } = useStorageItems();
  const [showSuccess, setShowSuccess] = useState(false);
  const [showError, setShowError] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const columns: {
    id: keyof StoredItem;
    label: string;
    minWidth: number;
    align: "left" | "right" | "center";
  }[] = [
    { id: "productName", label: "Product Name", minWidth: 200, align: "left" },
    {
      id: "productExpiryDate",
      label: "Expiry Date",
      minWidth: 150,
      align: "center",
    },
    { id: "quantity", label: "Quantity", minWidth: 100, align: "center" },
    { id: "id", label: "Actions", minWidth: 100, align: "center" },
  ];

  const handleSubmit = async () => {
    try {
      await updateStoredItems.mutateAsync(items);
      setShowSuccess(true);
      updatedStorageItemsStore.clearItems();
    } catch (error) {
      setErrorMessage(
        error instanceof Error ? error.message : "Failed to update items"
      );
      setShowError(true);
    }
  };

  const handleClear = () => {
    updatedStorageItemsStore.clearItems();
  };

  if (items.length === 0) {
    return (
      <Paper sx={{ p: 3 }}>
        <Typography variant="subtitle1" align="center">
          No updated items
        </Typography>
      </Paper>
    );
  }

  return (
    <Box>
      <Paper sx={{ p: 3 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            flexDirection: "column",
            mb: 2,
          }}
        >
          <Typography variant="h6" color="primary">
            Updated Items List
          </Typography>
          <Box sx={{ display: "flex", gap: 1 }}>
            <Button
              sx={filterButtonStyle}
              variant="contained"
              color="primary"
              startIcon={<SaveIcon />}
              onClick={handleSubmit}
              disabled={updateStoredItems.isPending}
            >
              Save
            </Button>
            <Button
              sx={filterButtonStyle}
              variant="outlined"
              color="error"
              startIcon={<ClearIcon />}
              onClick={handleClear}
            >
              Clear
            </Button>
          </Box>
        </Box>
        <TableContainer sx={{ maxHeight: 440 }}>
          <Table stickyHeader size="small">
            <TableHead>
              <TableRow>
                {columns.map((column) => (
                  <TableCell
                    key={column.id}
                    align={column.align}
                    style={{ minWidth: column.minWidth }}
                  >
                    {column.label}
                  </TableCell>
                ))}
              </TableRow>
            </TableHead>
            <TableBody>
              {items.map((item) => (
                <TableRow hover key={item.id}>
                  <TableCell align="left">{item.productName}</TableCell>
                  <TableCell align="center">
                    {item.productExpiryDate
                      ? dayjs(item.productExpiryDate).format("YYYY-MM-DD")
                      : "-"}
                  </TableCell>
                  <TableCell align="center">{item.quantity}</TableCell>
                  <TableCell align="center">
                    <Button
                      size="small"
                      color="error"
                      onClick={() =>
                        updatedStorageItemsStore.removeItem(item.id)
                      }
                    >
                      <DeleteIcon />
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>

      <Snackbar
        open={showSuccess}
        autoHideDuration={3000}
        onClose={() => setShowSuccess(false)}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert severity="success" sx={{ width: "100%" }}>
          Changes saved successfully!
        </Alert>
      </Snackbar>

      <Snackbar
        open={showError}
        autoHideDuration={3000}
        onClose={() => setShowError(false)}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert severity="error" sx={{ width: "100%" }}>
          {errorMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
});

export default UpdatedItemsTable;
