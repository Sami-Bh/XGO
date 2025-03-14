import { createBrowserRouter } from "react-router";
import App from "../../assets/App";
import { Home } from "@mui/icons-material";
import CategoriesDashboard from "../features/categories/CategoriesDashboard";
import CategoryDetails from "../features/categories/CategoryDetails";

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
                path: "/categories",
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

            // {
            //     path: `/categories/:id`,
            //     element: <CategoriesDashboard />,
            // },
        ]
    }
]);