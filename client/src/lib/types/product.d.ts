type ProductInfo = {
    id: number;
    name: string;
}

type Product = {
    name: string
    extraProperties: string
    isProximity: boolean
    isHeavy: boolean
    isBulky: boolean
    subCategoryId: number
    id: number
}

type ListedItem<T> = {
    items: T[];
    pageCount: number;
}

type ProductsFilter = {
    pageIndex: number;
    pageSize: number;
    textSearch?: string;
    categoryId?: number;
    subcategoryId?: number;
} 