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