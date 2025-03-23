type Category = {
    name: string,
    id: number,
    hasChildren: boolean,
}

type SubCategory = {
    name: string,
    id: number,
    categoryId: number,
    hasChildren: boolean,
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