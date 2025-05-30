import { makeAutoObservable } from "mobx";
import { StoredItem } from "../types/storage";

export default class UpdatedStorageItemsStore {
    updatedItems: StoredItem[] = [];

    constructor() {
        makeAutoObservable(this);
        this.loadFromLocalStorage();
    }

    private loadFromLocalStorage() {
        const savedItems = localStorage.getItem('updatedStorageItems');
        if (savedItems) {
            this.updatedItems = JSON.parse(savedItems);
        }
    }

    private saveToLocalStorage() {
        localStorage.setItem('updatedStorageItems', JSON.stringify(this.updatedItems));
    }

    addOrUpdateItem(item: StoredItem) {
        const existingIndex = this.updatedItems.findIndex(i => i.id === item.id);
        if (existingIndex !== -1) {
            this.updatedItems[existingIndex] = item;
        } else {
            this.updatedItems.push(item);
        }
        this.saveToLocalStorage();
    }

    removeItem(itemId: number) {
        this.updatedItems = this.updatedItems.filter(item => item.id !== itemId);
        this.saveToLocalStorage();
    }

    clearItems() {
        this.updatedItems = [];
        this.saveToLocalStorage();
    }

    get getUpdatedItems() {
        return this.updatedItems;
    }
} 