import { z } from "zod"

export const RequiredString = (fieldname: string) => z.
    string({ required_error: `field ${fieldname} is required` }).
    min(1, `field ${fieldname} is required`);
