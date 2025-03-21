import { z } from "zod";
import { RequiredString } from "./schemaUtils";

export const subcategorySchema = z.object({
    name: RequiredString("Name")
})

export type SubcategorySchema = z.infer<typeof subcategorySchema>