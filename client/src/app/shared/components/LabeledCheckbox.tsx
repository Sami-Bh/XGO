import { Checkbox, FormControlLabel } from "@mui/material";
import { FieldValues, useController, UseControllerProps } from "react-hook-form";

type Props<T extends FieldValues> = {
    Label: string
} & UseControllerProps<T>

export default function LabeledCheckbox<T extends FieldValues>(props: Props<T>) {
    const { field } = useController({ ...props });
    return (
        <FormControlLabel label={props.Label} control={<Checkbox checked={field.value || false} {...field} />} />
    )
}
