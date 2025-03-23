import { Box, CircularProgress, FormControl, Grid2, InputAdornment, InputLabel, MenuItem, Paper, Select, SelectChangeEvent, TextField, Typography } from "@mui/material";
import ProductList from "./ProductList";
import { useEffect, useMemo, useState } from "react";
import useProducts from "../../../lib/hooks/useProducts";
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import useCategories from "../../../lib/hooks/useCategories";
import useSubCategory from "../../../lib/hooks/useSubCategory";
import { useQueryClient } from "@tanstack/react-query";
export default function ProductDashboard() {
    const queryClient = useQueryClient();

    //Memos
    const defaultCategory = useMemo(() => { return { id: 0, name: "---All---" } as Category }, []);
    const defaultSubCategory = useMemo(() => { return { id: 0, name: "---All---" } as SubCategory }, []);

    //use state
    const [Products, setProducts] = useState<Product[]>([]);
    const [SelectedCategoryId, setSelectedCategoryId] = useState<number>(0);
    const [SelectedsubcategoryId, setSelectedsubcategoryId] = useState<number>(0)
    const [IsFiltersLoading, setIsFiltersLetLoading] = useState(false);

    const { productsFromServer, isGetProductsPending } = useProducts();
    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();
    const { isGetSubCategoriesPending, subcategoriesFromServer } = useSubCategory(SelectedCategoryId);

    const categoriesFilterSource = useMemo(() => {
        return isGettingCategoriesPending || !categoriesFromServer ?
            [defaultCategory] :
            [defaultCategory, ...categoriesFromServer.sort((c1, c2) => c1.name > c2.name ? 1 : -1)]
    }, [isGettingCategoriesPending, defaultCategory, categoriesFromServer]);

    // const subcategoriesFilterSource = useMemo(() => {
    //     return isGetSubCategoriesPending || !subcategoriesFromServer || SelectedCategoryId == 0 ?
    //         [defaultSubCategory] :
    //         [defaultSubCategory, ...subcategoriesFromServer.sort((c1, c2) => c1.name > c2.name ? 1 : -1)]

    // }, [isGetSubCategoriesPending, subcategoriesFromServer, defaultSubCategory, SelectedCategoryId])

    const subcategoriesFilterSource = isGetSubCategoriesPending || !subcategoriesFromServer || SelectedCategoryId == 0 ?
        [defaultSubCategory] :
        [defaultSubCategory, ...subcategoriesFromServer.sort((c1, c2) => c1.name > c2.name ? 1 : -1)];

    // const weightOptions = [{ id: 0, option: "All" }, { id: 1, option: "Heavy" }, { id: 2, option: "Light" }];

    //use effects
    useEffect(() => {
        setIsFiltersLetLoading(isGetSubCategoriesPending || isGettingCategoriesPending);
    }, [isGetSubCategoriesPending, isGettingCategoriesPending])


    useEffect(() => {
        if (productsFromServer) {
            setProducts(productsFromServer);
        }

    }, [productsFromServer]);

    useEffect(() => {

        queryClient.invalidateQueries({ queryKey: ["getSubCategories"] });
        setSelectedsubcategoryId(0);
    }, [queryClient, SelectedCategoryId]);

    const handleCategoryChange = (event: SelectChangeEvent<number>) => {
        setSelectedCategoryId(Number(event.target.value));

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
                                    disabled={isGettingCategoriesPending}
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
                                    disabled={isGetSubCategoriesPending}
                                    value={SelectedsubcategoryId}
                                    labelId="demo-simple-select-label"
                                    onChange={handleSubCategoryChange}
                                    label="Sub Category">
                                    {subcategoriesFilterSource?.map(subcategory =>
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
                        </Box>
                    </Paper>

                </Box>
            </Grid2>
        </Grid2>
    )
}
