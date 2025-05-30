import { Box, Card, CardContent, Typography, CircularProgress } from "@mui/material";
import { useState } from "react";
import SelectInput from "../../shared/components/SelectInput";
import useCategories from "../../../lib/hooks/store/useCategories";
import useSubCategory from "../../../lib/hooks/store/useSubCategory";
import useProducts from "../../../lib/hooks/store/useProducts";

export default function AddStorageItem() {
    const [CategoryId, setCategoryId] = useState<number | null>(null);
    const [SubCategoryId, setSubCategoryId] = useState<number | null>(null);
    const [selectedProductIndex, setSelectedProductIndex] = useState<number | null>(null);

    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();
    const { isGetSubCategoriesPending, subcategoriesFromServer } = useSubCategory(CategoryId || undefined);
    const { isGetProductNamesPending, productNamesFromServer } = useProducts(undefined, undefined, SubCategoryId || undefined);

    const productOptions = [
        { id: -1, name: "---Select---" },
        ...(productNamesFromServer ?? []).map((name, index) => ({
            id: index,
            name
        }))
    ];

    const selectedProductName = selectedProductIndex !== null && selectedProductIndex >= 0
        ? productNamesFromServer?.[selectedProductIndex]
        : null;

    return (
        <Card>
            <CardContent>
                <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
                    <Box sx={{ display: "flex", flexDirection: "row", gap: 1, justifyContent: "center" }}>
                        <Typography color="primary" variant="h5">Add Storage Item</Typography>
                        {(isGettingCategoriesPending || isGetSubCategoriesPending || isGetProductNamesPending) && <CircularProgress />}
                    </Box>

                    <SelectInput
                        value={CategoryId || -1}
                        label={"Categories"}
                        name="categoriesList"
                        options={[{ id: -1, name: "---Select---" }, ...(categoriesFromServer ?? []).sort((a: Category, b: Category) => a.name.localeCompare(b.name))]}
                        onChange={(value) => {
                            setCategoryId(value === -1 ? null : value);
                            setSubCategoryId(null);
                            setSelectedProductIndex(null);
                        }}
                    />

                    <SelectInput
                        value={SubCategoryId || -1}
                        label={"Sub Categories"}
                        name="subCategoryId"
                        options={[{ id: -1, name: "---Select---" }, ...(subcategoriesFromServer ?? []).sort((a: SubCategory, b: SubCategory) => a.name.localeCompare(b.name))]}
                        onChange={(value) => {
                            setSubCategoryId(value === -1 ? null : value);
                            setSelectedProductIndex(null);
                        }}
                    />

                    <SelectInput
                        value={selectedProductIndex || -1}
                        label={"Products"}
                        name="productName"
                        options={productOptions}
                        onChange={(value) => setSelectedProductIndex(value === -1 ? null : value)}
                    />
                </Box>
            </CardContent>
        </Card>
    );
} 