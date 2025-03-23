import { Box, CircularProgress, FormControl, Grid2, InputAdornment, InputLabel, MenuItem, Paper, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import ProductList from "./ProductList";
import { useEffect, useMemo, useState } from "react";
import useProducts from "../../../lib/hooks/useProducts";
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import useCategories from "../../../lib/hooks/useCategories";
import useSubCategory from "../../../lib/hooks/useSubCategory";
import { useQueryClient } from "@tanstack/react-query";
import SelectInput from "../../shared/components/SelectInput";
import { All_Item } from "../../../lib/types/constants";
export default function ProductDashboard() {
    const queryClient = useQueryClient();

    //Memos
    const defaultCategory = useMemo(() => { return [All_Item as Category] }, []);
    const defaultSubCategory = useMemo(() => { return [All_Item as SubCategory] }, []);

    //use state
    const [Products, setProducts] = useState<Product[]>([]);
    const [SelectedCategoryId, setSelectedCategoryId] = useState<number>(0);
    const [SubcategoriesFilterItems, SetsubcategoriesFilterItems] = useState<SubCategory[]>(defaultSubCategory);
    const [SelectedsubcategoryId, setSelectedsubcategoryId] = useState<number | "">("");

    const { productsFromServer, isGetProductsPending } = useProducts();
    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();
    const { isGetSubCategoriesPending, subcategoriesFromServer } = useSubCategory(SelectedCategoryId);

    const categoriesFilterSource = useMemo(() => {
        return isGettingCategoriesPending || !categoriesFromServer ?
            defaultCategory :
            [...defaultCategory, ...categoriesFromServer.sort((c1, c2) => c1.name > c2.name ? 1 : -1)]
    }, [isGettingCategoriesPending, categoriesFromServer, defaultCategory]);

    // const subcategoriesFilterSource = useMemo(() => {
    //     return !subcategoriesFromServer ?
    //         defaultSubCategory :
    //         subcategoriesFromServer.length == 1 ?
    //             subcategoriesFromServer :
    //             [...defaultSubCategory, ...subcategoriesFromServer.sort((c1, c2) => c1.name > c2.name ? 1 : -1)];
    // }, [defaultSubCategory, subcategoriesFromServer]);

    //fill list with items from server
    useEffect(() => {
        SetsubcategoriesFilterItems(!subcategoriesFromServer || isGetSubCategoriesPending ?
            defaultSubCategory :
            subcategoriesFromServer.length == 1 ?
                subcategoriesFromServer :
                [...defaultSubCategory, ...subcategoriesFromServer.sort((c1, c2) => c1.name > c2.name ? 1 : -1)]);

    }, [defaultSubCategory, isGetSubCategoriesPending, subcategoriesFromServer]);



    const IsFiltersLoading = isGetSubCategoriesPending || isGettingCategoriesPending;
    const weightOptions = [All_Item, { id: 1, value: "Heavy" }, { id: 2, value: "Light" }];

    useEffect(() => {
        if (productsFromServer) {
            setProducts(productsFromServer);
        }
    }, [productsFromServer]);

    // invalidate sub categories when category change
    useEffect(() => {
        if (SelectedCategoryId) {
            queryClient.invalidateQueries({ queryKey: ["getSubCategories"] });
        } else {
            SetsubcategoriesFilterItems(defaultSubCategory);
        }
    }, [queryClient, SelectedCategoryId, defaultSubCategory]);

    useEffect(() => {
        if (SubcategoriesFilterItems) {

            setSelectedsubcategoryId(SubcategoriesFilterItems[0].id);
        }

    }, [SubcategoriesFilterItems, isGetSubCategoriesPending]);

    const handleCategoryChange = (event: SelectChangeEvent<number>) => {
        setSelectedsubcategoryId("");
        const selectedcategory = Number(event.target.value);

        setSelectedCategoryId(selectedcategory);

    }
    const handleSubCategoryChange = (event: SelectChangeEvent<number>) => {
        setSelectedsubcategoryId(Number(event.target.value));
    }

    return (
        <Grid2 container spacing={3}>
            <Grid2 size={8}>
                <ProductList products={Products!} isLoading={isGetProductsPending} />
            </Grid2>
            <Grid2 size={4}>
                <Box sx={{
                    position: "sticky", alignSelf: "flex-start", top: 80
                }}>
                    <Paper>
                        <Box sx={{ display: "flex", flexDirection: "column", mx: 5, pb: 5, gap: 2 }}>
                            <Box sx={{ display: "flex", flexDirection: "row", gap: 1 }}>
                                {IsFiltersLoading && <CircularProgress />}
                                <Typography color="primary" sx={{ alignSelf: "center" }} variant="h5"><FilterListIcon /> Filters</Typography>
                            </Box>
                            <FormControl fullWidth>
                                <InputLabel id="demo-simple-select-label">Category</InputLabel>
                                <Select
                                    disabled={IsFiltersLoading}
                                    value={SelectedCategoryId}
                                    labelId="demo-simple-select-label"

                                    onChange={handleCategoryChange}
                                    label="Category">
                                    {categoriesFilterSource?.map(category =>
                                        (<MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>)
                                    )}
                                </Select>
                            </FormControl>

                            <FormControl fullWidth>
                                <InputLabel id="demo-simple-select-label">Sub Category</InputLabel>
                                <Select
                                    disabled={IsFiltersLoading}
                                    value={SelectedsubcategoryId}
                                    labelId="demo-simple-select-label"
                                    onChange={handleSubCategoryChange}
                                    label="Sub Category">
                                    {SubcategoriesFilterItems?.map(subcategory =>
                                        (<MenuItem key={subcategory.id} value={subcategory.id}>{subcategory.name}</MenuItem>)
                                    )}
                                </Select>
                            </FormControl>

                            <TextField
                                id="input-with-icon-textfield"
                                label="Name"
                                slotProps={{
                                    input: {
                                        startAdornment: (
                                            <InputAdornment position="start">
                                                < SearchIcon />
                                            </InputAdornment>
                                        ),
                                    },
                                }}
                                variant="standard"
                            />

                            {/* <SelectInput isDisabled={IsFiltersLoading} items={weightOptions} label={"Weight"} /> */}

                        </Box>
                    </Paper>

                </Box>
            </Grid2>
        </Grid2>
    )
}
