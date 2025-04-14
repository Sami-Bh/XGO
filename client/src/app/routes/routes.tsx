import { createBrowserRouter } from "react-router";
import App from "../../assets/App";
import CategoriesDashboard from "../features/categories/CategoriesDashboard";
import CategoryDetails from "../features/categories/CategoryDetails";
import SubCategoryDashboard from "../features/subcategories/SubCategoryDashboard";
import SubCategoryDetails from "../features/subcategories/SubCategoryDetails";
import NotFound from "../features/errors/NotFound";
import { categoriesUri, productsUri, storageUri, subcategoriesUri } from "./routesconsts";
import ProductDashboard from "../features/products/ProductDashboard";
import ProductDetails from "../features/products/ProductDetails";
import Home from "../features/home/Home";
import StorageDashboard from "../features/storage/StorageDashboard";

export const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        children: [
            {
                path: "",
                element: <Home />,
            },
            {
                path: storageUri,
                element: <StorageDashboard />
            },
            {
                path: categoriesUri,
                element: <CategoriesDashboard />,
                children: [
                    {
                        path: "new",
                        element: <CategoryDetails key="create" />,
                    },
                    {
                        path: ":id",
                        element: <CategoryDetails key="edit" />,
                    },
                ]
            },
            {
                path: `${subcategoriesUri}/:categoryId`,
                element: <SubCategoryDashboard />,
                children: [
                    {
                        path: "new",
                        element: <SubCategoryDetails key="create" />,
                    },
                    {
                        path: ":id",
                        element: <SubCategoryDetails key="edit" />,
                    },
                ]
            },
            {
                path: `${productsUri}`,
                element: <ProductDashboard />
            },
            {
                path: `${productsUri}/:id`,
                element: <ProductDetails key="create" />,
            },
            {
                path: `${productsUri}/new`,
                element: <ProductDetails key="edit" />,
            },
        ]
    },
    {
        path: "*",
        element: <NotFound />
    }
]);