import { Checkbox, CheckboxProps, FormControlLabel, FormControlProps } from "@mui/material";
import { FieldValues, useController, UseControllerProps } from "react-hook-form";

type Props<T extends FieldValues> = {
    label: string
} & UseControllerProps<T> & FormControlProps & CheckboxProps

export default function LabeledCheckbox<T extends FieldValues>(props: Props<T>) {
    const { field } = useController({ ...props });
    return (
        <FormControlLabel label={props.label} control={<Checkbox checked={field.value || false} {...field} {...props} />} />
    )
}
