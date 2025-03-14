import { createContext } from "react"
import CategoryStore from "./categoryStore"

export const store = {
    categorystore: new CategoryStore(),
}

export const StoreContext = createContext(store);