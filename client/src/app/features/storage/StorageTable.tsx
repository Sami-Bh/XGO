import { useContext, useState } from "react";
import { StorageFilter, StoredItem } from "../../../lib/types/storage";
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  Pagination,
  TableRow,
  TextField,
} from "@mui/material";
import useStorageItems from "../../../lib/hooks/storage/useStorageItems";
import { DesktopDatePicker } from "@mui/x-date-pickers/DesktopDatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";
import { StoreContext } from "../../../lib/stores/store";
import { reaction } from "mobx";
import { quantityFieldStyle } from "../../shared/actionStyles";

export default function StorageTable() {
  const getDefaultFilter = (): StorageFilter => {
    return {
      oderDirection: "asc",
      orderby: "name",
      pageIndex: 1,
      pageSize: 10,
      productNameSearchText: "",
      StorageId: undefined,
    };
  };
  const [Filter, setFilter] = useState<StorageFilter>(getDefaultFilter());
  const { StoredItemsFromServer, IsStoredItemsFromServerPending } =
    useStorageItems(Filter);

  const updatePageIndex = (value: number) => {
    setFilter((prevFilter) => ({ ...prevFilter, pageIndex: value }));
  };

  const store = useContext(StoreContext);

  reaction(
    () => store.storedItemsFilterStore.getPageFilter,
    (newValue) => {
      setFilter((prevFilter) => ({
        ...prevFilter,
        productNameSearchText: newValue.selectedStoredItemName ?? "",
        StorageId: newValue.selectedStorageId,
      }));
    }
  );

  const handleItemUpdate = (
    item: StoredItem,
    field: keyof StoredItem,
    value: unknown
  ) => {
    console.log(value);
    const tempitem = { ...item, [field]: value };
    console.log(item);

    store.updatedStorageItemsStore.addOrUpdateItem(tempitem);
  };

  // Define columns explicitly for each property of StoredItem
  const columns: {
    id: keyof StoredItem;
    label: string;
    minWidth: number;
    align: "left" | "right" | "center" | "inherit" | "justify";
  }[] = [
    // { id: 'productId', label: 'Product ID', minWidth: 100, align: 'left' },
    { id: "productName", label: "Product Name", minWidth: 100, align: "left" },
    {
      id: "productExpiryDate",
      label: "Expiry Date",
      minWidth: 100,
      align: "center",
    },
    { id: "quantity", label: "Quantity", minWidth: 60, align: "center" },
    // { id: 'id', label: 'ID', minWidth: 100, align: 'right' },
  ];

  const emptyRows =
    Filter.pageIndex > 0
      ? Math.max(
          0,
          Filter.pageSize - (StoredItemsFromServer?.items.length || 0)
        )
      : 0;

  if (IsStoredItemsFromServerPending) return <>Loading...</>;

  return (
    <Paper
      sx={{
        width: "100%",
        p: 2,
        overflow: "hidden",
        display: "flex",
        flexDirection: "column",
        height: 650,
      }}
    >
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
            {StoredItemsFromServer!.items.map(
              (storedItem: StoredItem, rowIndex) => (
                <TableRow hover role="checkbox" tabIndex={-1} key={rowIndex}>
                  {columns.map((column) => {
                    const value = storedItem[column.id];
                    switch (column.id) {
                      case "productExpiryDate":
                        return (
                          <TableCell key={column.id} align={column.align}>
                            <LocalizationProvider dateAdapter={AdapterDayjs}>
                              <DesktopDatePicker
                                defaultValue={value ? dayjs(value) : null}
                                // sx={{ width: 260 }}
                                slotProps={{
                                  field: { clearable: true },
                                }}
                                onChange={(newValue) => {
                                  handleItemUpdate(
                                    storedItem,
                                    "productExpiryDate",
                                    newValue?.toDate()
                                  );
                                }}
                              />
                            </LocalizationProvider>
                          </TableCell>
                        );
                      case "quantity":
                        return (
                          <TableCell key={column.id} align={column.align}>
                            <TextField
                              defaultValue={value}
                              sx={quantityFieldStyle}
                              slotProps={{
                                input: { type: "number" },
                                htmlInput: { min: 0 },
                              }}
                              onChange={(e) => {
                                handleItemUpdate(
                                  storedItem,
                                  "quantity",
                                  parseInt(e.target.value)
                                );
                              }}
                            />
                          </TableCell>
                        );
                      default:
                        return (
                          <TableCell key={column.id} align={column.align}>
                            {String(value ?? "")}
                          </TableCell>
                        );
                    }
                  })}
                </TableRow>
              )
            )}
            {emptyRows > 0 && (
              <TableRow style={{ height: 53 * emptyRows }}>
                <TableCell colSpan={6} />
              </TableRow>
            )}
          </TableBody>
        </Table>
      </TableContainer>
      <Pagination
        sx={{ alignSelf: "center", py: 1 }}
        count={StoredItemsFromServer?.pageCount || 1}
        color="primary"
        page={Filter.pageIndex}
        onChange={(_, value) => updatePageIndex(value)}
      />
    </Paper>
  );
}
