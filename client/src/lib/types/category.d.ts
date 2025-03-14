type Category = {
    name: string,
    id: number,
    subCategories?: SubCategory[],
}

type SubCategory = {
    name: string,
    id: number,
}