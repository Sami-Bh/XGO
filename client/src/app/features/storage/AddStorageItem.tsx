import { Box, Card, CardContent, Typography, CircularProgress, TextField, Button, Alert, Snackbar } from "@mui/material";
import { useState } from "react";
import SelectInput from "../../shared/components/SelectInput";
import useCategories from "../../../lib/hooks/store/useCategories";
import useSubCategory from "../../../lib/hooks/store/useSubCategory";
import useProducts from "../../../lib/hooks/store/useProducts";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DesktopDatePicker } from "@mui/x-date-pickers/DesktopDatePicker";
import dayjs from "dayjs";
import useStorageItems from "../../../lib/hooks/storage/useStorageItems";
import useStorageLocations from "../../../lib/hooks/storage/useStorageLocations";
import { useNavigate } from "react-router";
import { storageUri } from "../../routes/routesconsts";

export default function AddStorageItem() {
    const navigate = useNavigate();
    const [CategoryId, setCategoryId] = useState<number | null>(null);
    const [SubCategoryId, setSubCategoryId] = useState<number | null>(null);
    const [selectedProductId, setSelectedProductId] = useState<number | null>(null);
    const [quantity, setQuantity] = useState<number>(1);
    const [expiryDate, setExpiryDate] = useState<Date | undefined>(undefined);
    const [storageLocationId, setStorageLocationId] = useState<number | null>(null);
    const [showSuccess, setShowSuccess] = useState(false);

    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();
    const { isGetSubCategoriesPending, subcategoriesFromServer } = useSubCategory(CategoryId || undefined);
    const { isGetProductNamesPending, productNamesFromServer } = useProducts(undefined, undefined, SubCategoryId || undefined);
    const { StorageLocations, isStorageLocationsPending } = useStorageLocations();
    const { createStoredItem } = useStorageItems();

    const productOptions = [
        { id: -1, name: "---Select---" },
        ...(productNamesFromServer ?? []).map(product => ({
            id: product.id,
            name: product.name
        }))
    ];

    const selectedProduct = selectedProductId !== null && selectedProductId >= 0
        ? productNamesFromServer?.find(p => p.id === selectedProductId)
        : null;

    const handleSubmit = async () => {
        if (!selectedProduct || !storageLocationId || quantity < 1) return;

        try {
            await createStoredItem.mutateAsync({
                productId: selectedProduct.id,
                productName: selectedProduct.name,
                quantity,
                productExpiryDate: expiryDate,
                storageLocationId
            });
            setShowSuccess(true);
            setTimeout(() => {
                navigate(storageUri);
            }, 2000);
        } catch (error) {
            console.error('Failed to create storage item:', error);
        }
    };

    const isLoading = isGettingCategoriesPending || isGetSubCategoriesPending ||
        isGetProductNamesPending || isStorageLocationsPending ||
        createStoredItem.isPending;

    const canSubmit = selectedProduct && storageLocationId && quantity > 0 && !isLoading;

    return (
        <>
            <Card>
                <CardContent>
                    <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
                        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, justifyContent: "center", alignItems: "center" }}>
                            <Typography color="primary" variant="h5">Add Storage Item</Typography>
                            {isLoading && <CircularProgress size={20} />}
                        </Box>

                        <SelectInput
                            value={CategoryId || -1}
                            label={"Categories"}
                            name="categoriesList"
                            options={[{ id: -1, name: "---Select---" }, ...(categoriesFromServer ?? []).sort((a: Category, b: Category) => a.name.localeCompare(b.name))]}
                            onChange={(value) => {
                                setCategoryId(value === -1 ? null : value);
                                setSubCategoryId(null);
                                setSelectedProductId(null);
                            }}
                        />

                        <SelectInput
                            value={SubCategoryId || -1}
                            label={"Sub Categories"}
                            name="subCategoryId"
                            options={[{ id: -1, name: "---Select---" }, ...(subcategoriesFromServer ?? []).sort((a: SubCategory, b: SubCategory) => a.name.localeCompare(b.name))]}
                            onChange={(value) => {
                                setSubCategoryId(value === -1 ? null : value);
                                setSelectedProductId(null);
                            }}
                        />

                        <SelectInput
                            value={selectedProductId || -1}
                            label={"Products"}
                            name="productName"
                            options={productOptions}
                            onChange={(value) => setSelectedProductId(value === -1 ? null : value)}
                        />

                        <SelectInput
                            value={storageLocationId || -1}
                            label={"Storage Location"}
                            name="storageLocation"
                            options={[{ id: -1, name: "---Select---" }, ...(StorageLocations ?? []).map(loc => ({ id: loc.id, name: loc.name }))]}
                            onChange={(value) => setStorageLocationId(value === -1 ? null : value)}
                        />

                        <TextField
                            label="Quantity"
                            type="number"
                            value={quantity}
                            onChange={(e) => setQuantity(Math.max(1, parseInt(e.target.value) || 1))}
                            inputProps={{ min: 1 }}
                            required
                        />

                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DesktopDatePicker
                                label="Expiry Date (Optional)"
                                value={expiryDate ? dayjs(expiryDate) : null}
                                onChange={(newValue) => setExpiryDate(newValue?.toDate())}
                                slotProps={{
                                    textField: { fullWidth: true },
                                    field: { clearable: true }
                                }}
                            />
                        </LocalizationProvider>

                        <Button
                            variant="contained"
                            color="primary"
                            onClick={handleSubmit}
                            disabled={!canSubmit}
                            fullWidth
                        >
                            Add Item to Storage
                        </Button>
                    </Box>
                </CardContent>
            </Card>

            <Snackbar
                open={showSuccess}
                autoHideDuration={2000}
                anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            >
                <Alert severity="success" sx={{ width: '100%' }}>
                    Item successfully added to storage!
                </Alert>
            </Snackbar>
        </>
    );
} 