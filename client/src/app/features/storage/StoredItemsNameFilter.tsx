import { Autocomplete, CircularProgress, TextField } from '@mui/material';
import { useState } from 'react';
import useSearchStoredItemsNames from '../../../lib/hooks/storage/useSearchStoredItemsNames';
import { useDebounce } from 'use-debounce';
import { StoredItemName } from '../../../lib/types/storage';

export default function StoredItemsNameFilter() {
    const [SeatchText, setSeatchText] = useState("");
    const [DebouncedSeatchText] = useDebounce(SeatchText, 1000);
    const [SelectedStoredItemName, setSelectedStoredItemName] = useState<StoredItemName | null>(null);
    const { ProductNames, isPending: isProductNamesPeding } = useSearchStoredItemsNames(DebouncedSeatchText);
    const IsLoading = !!ProductNames && isProductNamesPeding;


    // if ((!!SeatchText && isProductNamesPeding)) return <>Loading...</>
    return (
        <Autocomplete
            sx={{ width: 300 }}
            onInputChange={(_, newInputValue) => {
                setSeatchText(newInputValue);
            }}
            isOptionEqualToValue={(option, value) => option.name === value.name}
            getOptionLabel={(option) => option.name}
            options={ProductNames || []}
            loading={isProductNamesPeding}
            noOptionsText="No products"
            value={SelectedStoredItemName}
            onChange={(_, value) => { setSelectedStoredItemName(value); }}
            renderInput={(params) => (
                <TextField
                    {...params}
                    label="Product name"
                    InputProps={{
                        ...params.InputProps,
                        endAdornment: (
                            <>
                                {IsLoading ? <CircularProgress color="inherit" size={20} /> : null}
                                {params.InputProps.endAdornment}
                            </>
                        ),
                    }}
                />
            )}
        />
    );
}
