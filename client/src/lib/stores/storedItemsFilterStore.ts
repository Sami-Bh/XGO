import { makeAutoObservable } from "mobx";

export default class StoredItemsFilterStore {

    selectedStorageId?: number;
    selectedStoredItemName?: string;
    constructor() {
        makeAutoObservable(this);
    }

    setSelectedStorageId = (input?: number) => {
        this.selectedStorageId = input;
    }

    setSelectedStoredItemName = (input?: string) => {
        this.selectedStoredItemName = input;
    }

    get getPageFilter() {
        return { selectedStorageId: this.selectedStorageId, selectedStoredItemName: this.selectedStoredItemName };
    }
}