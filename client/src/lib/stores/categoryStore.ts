import { makeAutoObservable } from "mobx"

export default class CategoryStore {
    selectedCategoryId?: number = undefined;
    createOrEditMode = false;
    constructor() {
        makeAutoObservable(this);
    }

    select(id: number) {
        this.selectedCategoryId = id;
    }
    unselect() {
        this.selectedCategoryId = undefined;
    }

    setCreateOrEdit(value: boolean) {
        this.createOrEditMode = value;
    }

    isEditMode() {
        return this.createOrEditMode;
    }
}