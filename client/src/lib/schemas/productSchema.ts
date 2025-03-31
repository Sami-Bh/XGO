import { z } from "zod"
import { RequiredString } from "./schemaUtils";

export const productSchema = z.object({
    name: RequiredString("name"),
    extraProperties: z.string(),
    isProximity: z.boolean(),
    isHeavy: z.boolean(),
    isBulky: z.boolean(),
    subCategoryId: z.number({ message: "a Sub Category is mandatory" })
});

export type ProductSchema = z.infer<typeof productSchema>