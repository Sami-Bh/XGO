import { createBrowserRouter } from "react-router";
import App from "../../assets/App";
import { Home } from "@mui/icons-material";
import CategoriesDashboard from "../features/categories/CategoriesDashboard";
import CategoryDetails from "../features/categories/CategoryDetails";
import SubCategoryDashboard from "../features/subcategories/SubCategoryDashboard";
import SubCategoryDetails from "../features/subcategories/SubCategoryDetails";
import NotFound from "../features/errors/NotFound";
import { categoriesUri, productsUri, subcategoriesUri } from "./routesconsts";
import ProductDashboard from "../features/products/ProductDashboard";
import ProductDetails from "../features/products/ProductDetails";

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