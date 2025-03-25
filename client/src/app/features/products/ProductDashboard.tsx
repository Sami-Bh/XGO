import { Box, Button, CircularProgress, Grid2, InputAdornment, Paper, TextField, Typography } from "@mui/material";
import ProductList from "./ProductList";
import { useEffect, useMemo, useState } from "react";
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import useCategories from "../../../lib/hooks/useCategories";
import useSubCategory from "../../../lib/hooks/useSubCategory";
import SelectInput from "../../shared/components/SelectInput";
import { All_Item } from "../../../lib/types/constants";
import { GetFilterdProducts } from "../../../lib/hooks/useProducts";
export default function ProductDashboard() {

    //Memos
    const defaultSubCategory = useMemo(() => { return [{ id: -1, name: "---All---" } as SubCategory] }, []);

    //use state
    const [SearchValue, setSearchValue] = useState("");
    const [Products, setProducts] = useState<Product[] | undefined>([]);
    const [SelectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
    const [SelectedSubcategoryId, setSelectedSubcategoryId] = useState<number>(-1);
    const [SubCategoriesFilterItemsSource, setSubCategoriesFilterItemsSource] = useState<SubCategory[]>([]);

    // const { filteredProductsFromServer, isGeFilteredtProductsPending } = useProducts(SelectedCategoryId || undefined, SelectedSubcategoryId, SearchValue, EnableSearchProduct);
    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();
    const { isGetSubCategoriesPending, subcategoriesFromServer = [] } = useSubCategory(SelectedCategoryId || undefined);



    const IsFiltersLoading = isGetSubCategoriesPending || isGettingCategoriesPending;


    useEffect(() => {

        if ((isGetSubCategoriesPending || !SelectedCategoryId) && subcategoriesFromServer.length !== 0) {
            setSubCategoriesFilterItemsSource(defaultSubCategory);
            setSelectedSubcategoryId(-1);
            return;
        }
        if (subcategoriesFromServer.length === 1) {
            setSubCategoriesFilterItemsSource(subcategoriesFromServer);
            setSelectedSubcategoryId(subcategoriesFromServer[0].id);
            return;
        }

        const newValue = [...defaultSubCategory, ...subcategoriesFromServer.sort((a, b) => a.name.localeCompare(b.name))];
        if (JSON.stringify(newValue) != JSON.stringify(SubCategoriesFilterItemsSource)) {
            setSubCategoriesFilterItemsSource(newValue);
            setSelectedSubcategoryId(-1);
        }

    }, [SelectedCategoryId, SubCategoriesFilterItemsSource, defaultSubCategory, isGetSubCategoriesPending, subcategoriesFromServer]);

    async function GetProductsDataAsync() {

        const products = await GetFilterdProducts(SelectedCategoryId || undefined, SelectedSubcategoryId, SearchValue);
        setProducts(products);
    }

    useEffect(() => {
        GetProductsDataAsync();

    }, []);

    function search() {
        GetProductsDataAsync();
    }
    return (
        <Grid2 container spacing={3}>
            <Grid2 size={8}>

                <ProductList products={Products ?? []} />
            </Grid2>
            <Grid2 size={4}>
                <Box sx={{
                    position: "sticky", alignSelf: "flex-start", top: 80
                }}>
                    <Paper>
                        <form action={search}>
                            <Box sx={{ display: "flex", flexDirection: "column", mx: 5, pb: 5, gap: 2 }}>

                                <Box sx={{ display: "flex", flexDirection: "row", gap: 1 }}>
                                    {IsFiltersLoading && <CircularProgress />}
                                    <Typography color="primary" sx={{ alignSelf: "center" }} variant="h5"><FilterListIcon /> Filters</Typography>
                                </Box>

                                <SelectInput
                                    name="categorySelect"
                                    label="Category"
                                    value={SelectedCategoryId !== null ? SelectedCategoryId : "-1"}
                                    options={[{ id: "-1", name: "---All---" }, ...(categoriesFromServer ?? []).sort((a, b) => a.name.localeCompare(b.name))]}
                                    onChange={(value) => setSelectedCategoryId(value === "-1" ? null : Number(value))}
                                />

                                <SelectInput
                                    name="subCategorySelect"
                                    label="SubCategory"
                                    value={SelectedSubcategoryId}
                                    options={SubCategoriesFilterItemsSource}
                                    onChange={(value) => setSelectedSubcategoryId(Number(value))}
                                />

                                <TextField
                                    id="input-with-icon-textfield"
                                    label="SearchName"
                                    name="SearchName"
                                    value={SearchValue}
                                    onChange={(e) => setSearchValue(e.target.value)}
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
                                <Button variant="contained" onClick={() => search()} endIcon={<SearchIcon />} >Search</Button>
                            </Box>
                        </form>

                    </Paper>
                </Box>
            </Grid2>
        </Grid2 >
    )
}
