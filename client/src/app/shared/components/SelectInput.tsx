import { FormControl, FormHelperText, InputLabel, MenuItem, Select, SelectProps } from "@mui/material";

type SelectInputProps<T> = Omit<SelectProps<T>, "onChange"> & {
    label: string;
    value: T;
    options: { id: T; name: string }[];
    onChange: (value: T) => void;
    errorMessage?: string;
    isDisabled?: boolean;
    showDeleteOption?: boolean; // New prop to toggle the delete option
};

export default function SelectInput<T extends number | "">({
    label,
    value,
    options,
    errorMessage,
    onChange,
    isDisabled = false,
    showDeleteOption = false, // Default to false
    ...rest
}: SelectInputProps<T>) {
    return (
        <FormControl fullWidth disabled={isDisabled}>
            <InputLabel>{label}</InputLabel>
            <Select
                {...rest}
                value={value}
                onChange={(e) => onChange(e.target.value as T)}
                label={label}
            >
                {showDeleteOption && (
                    <MenuItem value="">
                        <em>None</em> {/* This represents the "Delete" or "Clear" option */}
                    </MenuItem>
                )}
                {options.map((option) => (
                    <MenuItem key={option.id} value={option.id}>
                        {option.name}
                    </MenuItem>
                ))}
            </Select>
            <FormHelperText>{errorMessage}</FormHelperText>
        </FormControl>
    );
}
