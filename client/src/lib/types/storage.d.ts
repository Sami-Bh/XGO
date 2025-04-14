export type StoredItem = {
    productId: number
    productName: string
    productExpiryDate?: Date
    quantity: number
    id: number
}

export type StorageFilter = {
    StorageId: number,
    pageSize: number,
    pageIndex: number,
    orderby: string,
    oderDirection: string,
    productNameSearchText: string,
}