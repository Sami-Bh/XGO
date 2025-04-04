import { AppBar, Box, Button, Container, MenuItem, Toolbar, Typography } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import StoreIcon from '@mui/icons-material/Store';
import { NavLink } from "react-router";
import { categoriesUri, productsUri } from "../routes/routesconsts";
import useAuthentication from "../../lib/hooks/useAuthentication";
import LogoutIcon from '@mui/icons-material/Logout';

export default function Navbar() {
    const { LoggedInUser, handleSignOut } = useAuthentication();
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
                        <Box sx={{ display: "flex", flexDirection: "row", gap: 1, justifyContent: "center", alignItems: "center" }}>
                            <Typography> Hi, {LoggedInUser?.name}</Typography>
                            <Button color="error"
                                startIcon={<LogoutIcon />}
                                variant="contained"
                                onClick={() => { handleSignOut(); }}
                            >Log out</Button>
                        </Box>
                    </Toolbar>
                </Container>
            </AppBar>
        </Box>

    )
}
