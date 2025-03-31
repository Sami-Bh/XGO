export function GetNumberOrUndefined<T extends number | "">(input: T) {
    return input ? Number(input) : undefined;
};