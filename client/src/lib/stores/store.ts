import { createContext } from "react"
import CategoryStore from "./categoryStore"
import StoredItemsFilterStore from "./storedItemsFilterStore";
import UpdatedStorageItemsStore from "./updatedStorageItemsStore";

export const store = {
    categorystore: new CategoryStore(),
    storedItemsFilterStore: new StoredItemsFilterStore(),
    updatedStorageItemsStore: new UpdatedStorageItemsStore()
}

export const StoreContext = createContext(store);