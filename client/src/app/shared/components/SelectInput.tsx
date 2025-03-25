// import { FormControl, InputLabel, Select, MenuItem, SelectProps, SelectChangeEvent } from '@mui/material'

// type Props<T extends { id: number | ""; name: string }> = Omit<SelectProps<T["id"]>, "onChange" | "value"> & {
//     items: T[],
//     label: string,
//     value: number | "",
//     onChange: (event: SelectChangeEvent<T["id"]>) => void
// }
// export default function SelectInput<T extends { id: number | ""; name: string }>(
//     { items, label, onChange, value, ...rest }: Props<T>) {
//     return (
//         <FormControl fullWidth>
//             <InputLabel >{label}</InputLabel>
//             <Select
//                 value={value}
//                 onChange={onChange}
//                 label={label} {...rest}>
//                 {items?.map(item =>
//                     (<MenuItem key={item.id} value={item.id}>{item.name}</MenuItem>)
//                 )}
//             </Select>
//         </FormControl>
//     )
// }

import { FormControl, InputLabel, MenuItem, Select, SelectProps } from "@mui/material";

type SelectInputProps<T> = Omit<SelectProps<T>, "onChange"> & {
    label: string;
    value: T;
    options: { id: number | string; name: string }[];
    onChange: (value: T) => void;

    isDisabled?: boolean;
}

export default function SelectInput<T extends number | string>({
    label,
    value,
    options,
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
        </FormControl>
    );
}
