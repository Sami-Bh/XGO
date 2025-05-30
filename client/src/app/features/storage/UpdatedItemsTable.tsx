import { observer } from "mobx-react-lite";
import { useContext } from "react";
import { StoreContext } from "../../../lib/stores/store";
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, Button, Box } from "@mui/material";
import dayjs from "dayjs";
import DeleteIcon from '@mui/icons-material/Delete';
import { StoredItem } from "../../../lib/types/storage";

const UpdatedItemsTable = observer(() => {
    const { updatedStorageItemsStore } = useContext(StoreContext);
    const items = updatedStorageItemsStore.getUpdatedItems;

    const columns: { id: keyof StoredItem; label: string; minWidth: number; align: "left" | "right" | "center" }[] = [
        { id: 'productName', label: 'Product Name', minWidth: 200, align: 'left' },
        { id: 'productExpiryDate', label: 'Expiry Date', minWidth: 150, align: 'center' },
        { id: 'quantity', label: 'Quantity', minWidth: 100, align: 'center' },
        { id: 'id', label: 'Actions', minWidth: 100, align: 'center' },
    ];

    if (items.length === 0) {
        return (
            <Paper sx={{ p: 3 }}>
                <Typography variant="subtitle1" align="center">No updated items</Typography>
            </Paper>
        );
    }

    return (
        <Box>
            <Paper sx={{ p: 3 }}>
                <Typography variant="h6" gutterBottom color="primary">
                    Updated Items List
                </Typography>
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
                                        {item.productExpiryDate ? dayjs(item.productExpiryDate).format('YYYY-MM-DD') : '-'}
                                    </TableCell>
                                    <TableCell align="center">{item.quantity}</TableCell>
                                    <TableCell align="center">
                                        <Button
                                            size="small"
                                            color="error"
                                            onClick={() => updatedStorageItemsStore.removeItem(item.id)}
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
        </Box>
    );
});

export default UpdatedItemsTable; 