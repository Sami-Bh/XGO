import { useForm } from "react-hook-form"
import { productSchema, ProductSchema } from "../../../lib/schemas/productSchema"
import { Box, Button, Card, CardContent, CircularProgress, Paper, Typography } from "@mui/material";
import { zodResolver } from "@hookform/resolvers/zod";
import { useNavigate, useParams } from "react-router";
import { useEffect, useState } from "react";
import TextInput from "../../shared/components/TextInput";
import LabeledCheckbox from "../../shared/components/LabeledCheckbox";
import SelectInput from "../../shared/components/SelectInput";
import { GetNumberOrUndefined } from "../../../lib/Utils/NumberUtils";
import { productsUri } from "../../routes/routesconsts";
import useCategories from "../../../lib/hooks/store/useCategories";
import useProducts from "../../../lib/hooks/store/useProducts";
import useSubCategory from "../../../lib/hooks/store/useSubCategory";

export default function ProductDetails() {
    const { handleSubmit, control, reset } = useForm<ProductSchema>({
        resolver: zodResolver(productSchema),
        mode: "onTouched",
        defaultValues: { isProximity: false, isBulky: false, isHeavy: false, extraProperties: "" }
    });
    const { id } = useParams();
    const navigate = useNavigate();
    const { IsGetProductPending, ProductFromServer, createProduct, updateProduct, deleteProduct } = useProducts(undefined, Number(id));
    const [CategoryId, setCategoryId] = useState<number | "">("");
    const [SubCategoryId, setSubCategoryId] = useState<number | "">("");

    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();

    const { isGetSubCategoriesPending, subcategoriesFromServer } = useSubCategory(GetNumberOrUndefined(CategoryId));
    const { isGetSubCategoryLoading, subcategoryFromServer } = useSubCategory(undefined, ProductFromServer ? GetNumberOrUndefined(ProductFromServer.subCategoryId) : undefined);

    const PageTitle = (id ? "Edit" : "Create") + " Product";
    const ButtonMessage = id ? "Edit" : "Create";
    // reset the form
    useEffect(() => {
        if (ProductFromServer) {
            reset(ProductFromServer);
        }
    }, [ProductFromServer, reset]);

    //initialize category id when the list changes 
    useEffect(() => {

        if (categoriesFromServer && categoriesFromServer.length > 0) {
            setCategoryId(categoriesFromServer[0].id);
            return;
        }

        ResetSubCategoriesSelection();

    }, [categoriesFromServer]);

    //initialize subcategory id when the list changes 
    useEffect(() => {

        if (subcategoriesFromServer && subcategoriesFromServer.length > 0) {
            setSubCategoryId(subcategoriesFromServer[0].id);
            return;
        }
        ResetSubCategoriesSelection();
    }, [subcategoriesFromServer]);

    //in case of an existing product, this will update the category and subcategory ids with the ones coming from the product
    useEffect(() => {
        if (subcategoryFromServer) {
            setCategoryId(subcategoryFromServer.categoryId);
            ResetSubCategoriesSelection();
            setSubCategoryId(subcategoryFromServer.id)
        }
    }, [subcategoryFromServer]);

    const ResetSubCategoriesSelection = () => {
        setSubCategoryId("");
    }

    const OnSumbit = async (productSchema: ProductSchema) => {
        const productToSend = { ...ProductFromServer, ...productSchema, subCategoryId: SubCategoryId } as Product;
        if (ProductFromServer) {
            await updateProduct.mutateAsync(productToSend, {
                onSuccess: () => {
                    navigate(`${productsUri}`);
                }
            });
            return;
        }

        const newid = await createProduct.mutateAsync(productToSend);
        navigate(`${productsUri}/${newid}`);
    }

    const OnDelete = async () => {
        if (!ProductFromServer) return;

        await deleteProduct.mutateAsync(ProductFromServer.id, { onSuccess: () => navigate(productsUri) })
    }

    if (id && IsGetProductPending) return <Typography>Loading...</Typography>

    return (
        <Box component={"form"} onSubmit={handleSubmit(OnSumbit)}>
            <Paper>
                <Card >
                    <Box sx={{ display: "flex", flexDirection: "row", gap: 1, justifyContent: "center" }}>
                        <Typography color="primary" variant="h5">{PageTitle}</Typography>
                        {(isGettingCategoriesPending || isGetSubCategoriesPending || isGetSubCategoryLoading) && <CircularProgress />}
                    </Box>
                    <CardContent>
                        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
                            <SelectInput value={CategoryId} label={"Categories"} name="categoriesList"
                                options={categoriesFromServer?.sort((a: Category, b: Category) => a.name.localeCompare(b.name)) || []} onChange={(value) => {
                                    setCategoryId(value);
                                    setSubCategoryId("");
                                }} />

                            <SelectInput value={SubCategoryId} label={"Sub Categories"} name="subCategoryId"
                                options={subcategoriesFromServer?.sort((a: SubCategory, b: SubCategory) => a.name.localeCompare(b.name)) || []} onChange={(value) => setSubCategoryId(value)} />

                            <TextInput label="Name" control={control} name="name" />
                            <Box sx={{ display: "flex", flexDirection: "row", gap: 2 }}>
                                <LabeledCheckbox control={control} label="Heavy" name="isHeavy" />
                                <LabeledCheckbox control={control} label="Proximity" name="isProximity" />
                                <LabeledCheckbox control={control} label="Bulky" name="isBulky" />
                            </Box>
                            <TextInput label="Description" control={control} name="extraProperties" multiline />
                            <Box sx={{ display: "flex", justifyContent: "end", gap: 2, mt: 2 }}>
                                {ProductFromServer &&
                                    <Button variant="contained" size="medium" color="error" onClick={OnDelete} >
                                        Delete
                                    </Button>
                                }
                                <Button type='submit' variant="contained" size="medium" color="primary" >
                                    {ButtonMessage}
                                </Button>
                            </Box>
                        </Box>

                    </CardContent>
                </Card>
            </Paper>
        </Box>
    )
}
