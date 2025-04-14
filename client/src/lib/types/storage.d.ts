export type StoredItem = {
    productId: number
    productName: string
    productExpiryDate?: Date
    quantity: number
    id: number
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