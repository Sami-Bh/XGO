import { createContext } from "react"
import CategoryStore from "./categoryStore"
import StoredItemsFilterStore from "./storedItemsFilterStore";

export const store = {
    categorystore: new CategoryStore(),
    storedItemsFilterStore: new StoredItemsFilterStore(),
}

export const StoreContext = createContext(store);