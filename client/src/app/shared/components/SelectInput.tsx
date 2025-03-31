import { FormControl, FormHelperText, InputLabel, MenuItem, Select, SelectProps } from "@mui/material";

type SelectInputProps<T> = Omit<SelectProps<T>, "onChange"> & {
    label: string;
    value: T;
    options: { id: T; name: string }[];
    onChange: (value: T) => void;
    errorMessage?: string
    isDisabled?: boolean;
}

export default function SelectInput<T extends number | "">({
    label,
    value,
    options,
    errorMessage,
    onChange,
    isDisabled = false,
    ...rest
}: SelectInputProps<T>) {
    return (
        <FormControl fullWidth disabled={isDisabled}>
            <InputLabel>{label}</InputLabel>
            <Select
                {...rest}
                value={value}

                onChange={(e) => onChange(e.target.value as T)} label={label}
            >
                {
                    options.map((option) => (
                        <MenuItem key={option.id} value={option.id}>
                            {option.name}
                        </MenuItem>
                    ))
                }
            </Select>
            <FormHelperText>{errorMessage}</FormHelperText>

        </FormControl>
    );
}
