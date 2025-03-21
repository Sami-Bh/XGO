import { AppBar, Box, Container, MenuItem, Toolbar, Typography } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import StoreIcon from '@mui/icons-material/Store';
import { NavLink } from "react-router";
import { categoriesUri, productsUri } from "../routes/routesconsts";
export default function Navbar() {
    return (

        <Box sx={{ flexGrow: 1, pb: 7 }}>
            <AppBar >
                <Container maxWidth={"xl"}>
                    <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
                        <Box >
                            <MenuItem component={NavLink} to="/" sx={{ display: "flex", gap: 2 }}>
                                <StoreIcon>
                                    <MenuIcon />
                                </StoreIcon>
                                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                                    XGO
                                </Typography>
                            </MenuItem>
                        </Box>
                        <Box sx={{ display: "flex" }}>
                            <MenuItem component={NavLink} to={categoriesUri}>Categories</MenuItem>

                            <MenuItem component={NavLink} to={productsUri}>Products</MenuItem>
                            {/* <MenuItem component={NavLink} to="/categories">SubCategories</MenuItem>
                            <MenuItem component={NavLink} to="/categories">Products</MenuItem> */}
                        </Box>
                        <Box></Box>
                    </Toolbar>
                </Container>
            </AppBar>
        </Box>

    )
}
