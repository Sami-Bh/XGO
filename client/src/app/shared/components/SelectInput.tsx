import { FormControl, InputLabel, Select, MenuItem, SelectProps } from '@mui/material'

type Props = {
    items: { id: number, value: string }[],
    label: string,
    isDisabled: boolean
} & SelectProps
export default function SelectInput(props: Props) {
    return (
        <FormControl fullWidth>
            <InputLabel id="demo-simple-select-label">{props.label}</InputLabel>
            <Select
                disabled={props.isDisabled}
                labelId="demo-simple-select-label"
                label={props.label}>
                {props.items?.map(item =>
                    (<MenuItem key={item.id} value={item.id}>{item.value}</MenuItem>)
                )}
            </Select>
        </FormControl>
    )
}
