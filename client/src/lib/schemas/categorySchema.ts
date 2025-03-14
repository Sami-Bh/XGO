import { z } from "zod"
import { RequiredString } from "./schemaUtils";

export const categorySchema = z.object({
    name: RequiredString("Name"),
});

export type CategorySchema = z.infer<typeof categorySchema>;