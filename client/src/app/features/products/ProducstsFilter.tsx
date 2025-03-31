import { useEffect, useMemo, useState } from 'react'
import useCategories from '../../../lib/hooks/useCategories';
import useSubCategory from '../../../lib/hooks/useSubCategory';
import { Paper, Box, CircularProgress, Typography, TextField, Button } from '@mui/material';
import FilterListIcon from '@mui/icons-material/FilterList';
import SearchIcon from '@mui/icons-material/Search';
import SelectInput from '../../shared/components/SelectInput';

type Props = {
    setFilter: (productsFilter: ProductsFilter) => void
}
export default function ProducstsFilter({ setFilter }: Props) {
    //use state
    const [SearchValue, setSearchValue] = useState("");
    const [IsFiltersLoading, setIsFiltersLoading] = useState(false);
    const [SelectedCategoryId, setSelectedCategoryId] = useState<number | null>(null);
    const [SelectedSubcategoryId, setSelectedSubcategoryId] = useState<number>(-1);
    const [SubCategoriesFilterItemsSource, setSubCategoriesFilterItemsSource] = useState<SubCategory[]>([]);

    //Memos
    const defaultSubCategory = useMemo(() => { return [{ id: -1, name: "---All---" } as SubCategory] }, []);

    const { isGettingCategoriesPending, categoriesFromServer } = useCategories();
    const { isGetSubCategoriesPending, subcategoriesFromServer = [] } = useSubCategory(SelectedCategoryId || undefined);
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

    useEffect(() => {
        setIsFiltersLoading(isGetSubCategoriesPending || isGettingCategoriesPending);
    }, [isGetSubCategoriesPending, isGettingCategoriesPending, setIsFiltersLoading])

    const SetFilter = () => {
        setFilter({ categoryId: SelectedCategoryId, subcategoryId: SelectedSubcategoryId, textSearch: SearchValue, pageIndex: 1 } as ProductsFilter)
    }
    return (
        <Paper>

            <Box sx={{ display: "flex", flexDirection: "column", mx: 5, pb: 5, gap: 2 }}>

                <Box sx={{ display: "flex", flexDirection: "row", gap: 1 }}>
                    {IsFiltersLoading && <CircularProgress />}
                    <Typography color="primary" sx={{ alignSelf: "center" }} variant="h5"><FilterListIcon /> Filters</Typography>
                </Box>

                <SelectInput
                    name="categorySelect"
                    label="Category"
                    value={SelectedCategoryId !== null ? SelectedCategoryId : -1}
                    options={[{ id: -1, name: "---All---" }, ...(categoriesFromServer ?? []).sort((a, b) => a.name.localeCompare(b.name))]}
                    onChange={(value) => setSelectedCategoryId(value === -1 ? null : Number(value))}
                />

                <SelectInput
                    name="subCategorySelect"
                    label="Sub Category"
                    value={SelectedSubcategoryId}
                    options={SubCategoriesFilterItemsSource}
                    onChange={(value) => setSelectedSubcategoryId(Number(value))}
                />

                <TextField
                    id="input-with-icon-textfield"
                    label="Name"
                    name="SearchName"
                    value={SearchValue}
                    onChange={(e) => setSearchValue(e.target.value)}
                    variant="standard"
                />

                {/* <SelectInput isDisabled={IsFiltersLoading} items={weightOptions} label={"Weight"} /> */}
                <Button variant="contained" onClick={SetFilter} endIcon={<SearchIcon />} >Search</Button>
            </Box>
        </Paper>
    )
}
