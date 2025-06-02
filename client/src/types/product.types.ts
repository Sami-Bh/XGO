export type ProductInfo = {
    id: number;
    name: string;
}

export type Product = {
    id: number;
    name: string;
    extraProperties?: string;
    isProximity: boolean;
    isHeavy: boolean;
    isBulky: boolean;
    subcategoryId: number;
}

export type ListedItem<T> = {
    items: T[];
    pageCount: number;
}

export type ProductsFilter = {
    pageIndex: number;
    pageSize: number;
    textSearch?: string;
    categoryId?: number;
    subcategoryId?: number;
} 