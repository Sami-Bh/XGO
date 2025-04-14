import { useState } from "react"
import { StorageFilter, StoredItem } from "../../../lib/types/storage"
import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, Pagination, TableRow, TextField } from "@mui/material";
import useStorageItems from "../../../lib/hooks/storage/useStorageItems";
import { DesktopDatePicker } from '@mui/x-date-pickers/DesktopDatePicker';
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";


export default function StorageTable() {
    const getDefaultFilter = (): StorageFilter => { return { oderDirection: "asc", orderby: "name", pageIndex: 1, pageSize: 10, productNameSearchText: "", StorageId: 1 } };
    const [Filter, setFilter] = useState<StorageFilter>(getDefaultFilter());
    const { StoredItemsFromServer, IsStoredItemsFromServerPending } = useStorageItems(Filter);

    const updatePageIndex = (value: number) => {
        setFilter((prevFilter) => ({ ...prevFilter, pageIndex: value }));
    };

    // Define columns explicitly for each property of StoredItem
    const columns: { id: keyof StoredItem; label: string; minWidth: number; align: "left" | "right" | "center" | "inherit" | "justify" }[] = [
        // { id: 'productId', label: 'Product ID', minWidth: 100, align: 'left' },
        { id: 'productName', label: 'Product Name', minWidth: 150, align: 'left' },
        { id: 'productExpiryDate', label: 'Expiry Date', minWidth: 150, align: 'left' },
        { id: 'quantity', label: 'Quantity', minWidth: 100, align: 'right' },
        // { id: 'id', label: 'ID', minWidth: 100, align: 'right' },
    ];

    const emptyRows = Filter.pageIndex > 0 ? Math.max(0, Filter.pageSize - (StoredItemsFromServer?.items.length || 0)) : 0;

    if (IsStoredItemsFromServerPending) return <>Loading...</>;

    return (
        <Paper sx={{ width: '100%', overflow: 'hidden', display: "flex", flexDirection: "column", height: 650 }}>
            <TableContainer sx={{ maxHeight: 600 }}>
                <Table stickyHeader aria-label="sticky table">
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
                        {StoredItemsFromServer!.items
                            .map((storedItem: StoredItem, rowIndex) => (
                                <TableRow hover role="checkbox" tabIndex={-1} key={rowIndex}>
                                    {columns.map((column) => {
                                        const value = storedItem[column.id as keyof StoredItem];
                                        switch (column.id) {
                                            case 'productExpiryDate':

                                                return (
                                                    <TableCell key={column.id} align={column.align}>
                                                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                                                            <DesktopDatePicker
                                                                defaultValue={value ? dayjs(value) : null}
                                                                sx={{ width: 260 }}
                                                                slotProps={{
                                                                    field: { clearable: true, },
                                                                }}
                                                            />
                                                        </LocalizationProvider>
                                                    </TableCell>
                                                );
                                            case 'quantity':
                                                return (
                                                    <TableCell key={column.id} align={column.align}>
                                                        <TextField defaultValue={value} slotProps={{ input: { type: "number" } }} />
                                                    </TableCell>
                                                );
                                            default:
                                                return (<TableCell key={column.id} align={column.align}>
                                                    {String(value ?? '')}
                                                </TableCell>);
                                        }

                                    })}
                                </TableRow>
                            ))}
                        {emptyRows > 0 && (
                            <TableRow style={{ height: 53 * emptyRows }}>
                                <TableCell colSpan={6} />
                            </TableRow>
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
            <Pagination sx={{ alignSelf: "center", py: 1 }}
                count={StoredItemsFromServer?.pageCount || 1} color="primary"
                page={Filter.pageIndex}
                onChange={(_, value) => updatePageIndex(value)}
            />
        </Paper>
    );
}
