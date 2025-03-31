import { useForm } from "react-hook-form"
import { productSchema, ProductSchema } from "../../../lib/schemas/productSchema"
import { Box, Card, CardContent, CardHeader, Paper, TextField, Typography } from "@mui/material";
import { zodResolver } from "@hookform/resolvers/zod";
import { useParams } from "react-router";
import useProducts from "../../../lib/hooks/useProducts";
import { useEffect, useState } from "react";
import TextInput from "../../shared/components/TextInput";
import LabeledCheckbox from "../../shared/components/LabeledCheckbox";
import useCategories from "../../../lib/hooks/useCategories";
import useSubCategory from "../../../lib/hooks/useSubCategory";
import SelectInput from "../../shared/components/SelectInput";

export default function ProductDetails() {
    const { handleSubmit, control, reset, register, formState: { errors } } = useForm<ProductSchema>({
        resolver: zodResolver(productSchema),
        mode: "onTouched"
    });
    const { id } = useParams();
    const { IsGetProductPending, ProductFromServer, createProduct, updateProduct } = useProducts(undefined, Number(id));
    const [CategoryId, setCategoryId] = useState<number>();
    const [SubCategoryId, setSubCategoryId] = useState<number>();

    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();

    const { isGetSubCategoriesPending, subcategoriesFromServer, subcategoryFromServer, isGetSubCategoryLoading } = useSubCategory(CategoryId, SubCategoryId);

    const PageTitle = (id ? "Edit" : "Create") + " Product";

    useEffect(() => {
        if (ProductFromServer) {
            reset(ProductFromServer);
            setSubCategoryId(ProductFromServer.subCategoryId);
        }
        if (ProductFromServer && subcategoryFromServer) {
            setCategoryId(subcategoryFromServer.categoryId);
        }
    }, [ProductFromServer, reset, subcategoryFromServer]);

    useEffect(() => {
        if (categoriesFromServer && categoriesFromServer.length > 0) {
            setCategoryId(categoriesFromServer[0].id);

        }
    }, [categoriesFromServer]);

    useEffect(() => {
        if (subcategoriesFromServer && subcategoriesFromServer.length > 0) {
            setSubCategoryId(subcategoriesFromServer[0].id);

        }
    }, [subcategoriesFromServer]);

    const OnSumbit = (productSchema: ProductSchema) => {

    }

    if (isGetSubCategoryLoading || isGettingCategoriesPending || isGetSubCategoriesPending) return <Typography>Loading...</Typography>
    if (id && IsGetProductPending) return <Typography>Loading...</Typography>
    return (
        <Box component={"form"} onSubmit={handleSubmit(OnSumbit)}>
            <Paper>
                <Card >
                    <CardHeader title={PageTitle} slotProps={{
                        title: { fontWeight: "bold", fontSize: 20, align: "center" }
                    }} />
                    <CardContent>
                        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
                            {categoriesFromServer && <SelectInput defaultValue={categoriesFromServer[0].id} value={CategoryId!} label={"Categories"} name="categoriesList" options={categoriesFromServer || []} onChange={(value) => setCategoryId(Number(value))} />}
                            {subcategoriesFromServer && <SelectInput defaultValue={subcategoriesFromServer[0].id} value={SubCategoryId!} label={"Sub Categories"} name="subCategoryId" options={subcategoriesFromServer || []} onChange={(value) => setSubCategoryId(Number(value))} />}
                            <TextInput label="Name" control={control} name="name" />
                            <Box sx={{ display: "flex", flexDirection: "row", gap: 2 }}>
                                <LabeledCheckbox control={control} Label="Heavy" name="isHeavy" />
                                <LabeledCheckbox control={control} Label="Proximity" name="isProximity" />
                                <LabeledCheckbox control={control} Label="Bulky" name="isBulky" />
                            </Box>
                            <TextField label="Description" {...register("extraProperties")} name="extraProperties" multiline />
                        </Box>
                    </CardContent>
                </Card>

            </Paper>
        </Box>
    )
}
