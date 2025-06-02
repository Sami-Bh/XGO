export type StoredItem = {
    productId: number
    productName: string
    productExpiryDate?: Date
    quantity: number
    id: number
    storageLocationId: number
}

export type StorageFilter = {
    categoryId?: number,
    subcategoryId?: number,
    StorageId?: number,
    pageSize: number,
    pageIndex: number,
    orderby: string,
    oderDirection: string,
    productNameSearchText: string,
}

type StorageLocation = {
    name: string,
    id: number,
}

type StoredItemName = {
    name: string,
    id: number,
}