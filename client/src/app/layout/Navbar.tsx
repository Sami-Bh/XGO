import {
  AppBar,
  Box,
  Button,
  Container,
  MenuItem,
  Toolbar,
  Typography,
} from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import StoreIcon from "@mui/icons-material/Store";
import { NavLink } from "react-router";
import { categoriesUri, productsUri, storageUri } from "../routes/routesconsts";
import LogoutIcon from "@mui/icons-material/Logout";
import useAuthentication from "../../lib/hooks/authentication/useAuthentication";
import {
  navBarItemBaseStyle,
  navBarMenuItemStyle,
} from "../shared/actionStyles";

export default function Navbar() {
  const { LoggedInUser, handleSignOut } = useAuthentication();
  return (
    <Box sx={{ flexGrow: 1, pb: 7 }}>
      <AppBar>
        <Container maxWidth={"xl"} sx={{ overflowX: "auto" }}>
          <Toolbar
            sx={{
              display: "flex",
              justifyContent: "space-between",
              flexWrap: { xs: "nowrap", md: "wrap" },
              minWidth: "max-content",
              gap: { xs: 1, md: 0 },
            }}
          >
            <Box sx={{ flexShrink: 0 }}>
              <MenuItem
                component={NavLink}
                to="/"
                sx={{ display: "flex", gap: 2 }}
              >
                <StoreIcon>
                  <MenuIcon />
                </StoreIcon>
                <Typography
                  variant="h6"
                  component="div"
                  sx={{ ...navBarItemBaseStyle, flexGrow: 1 }}
                >
                  XGO
                </Typography>
              </MenuItem>
            </Box>
            <Box sx={{ display: "flex", flexShrink: 1 }}>
              <MenuItem
                component={NavLink}
                to={categoriesUri}
                sx={navBarMenuItemStyle}
              >
                Categories
              </MenuItem>

              <MenuItem
                component={NavLink}
                to={productsUri}
                sx={navBarMenuItemStyle}
              >
                Products
              </MenuItem>
              <MenuItem
                component={NavLink}
                to={storageUri}
                sx={navBarMenuItemStyle}
              >
                Storage
              </MenuItem>
              {/* <MenuItem component={NavLink} to="/categories">SubCategories</MenuItem>
                            <MenuItem component={NavLink} to="/categories">Products</MenuItem> */}
            </Box>
            <Box
              sx={{
                display: "flex",
                flexDirection: "row",
                gap: 1,
                justifyContent: "center",
                alignItems: "center",
                flexShrink: 0,
              }}
            >
              <Typography sx={{ whiteSpace: "nowrap" }}>
                Hi, {LoggedInUser?.name}
              </Typography>
              <Button
                color="error"
                startIcon={<LogoutIcon />}
                variant="contained"
                onClick={() => {
                  handleSignOut();
                }}
                sx={{ whiteSpace: "nowrap" }}
              >
                Log out
              </Button>
            </Box>
          </Toolbar>
        </Container>
      </AppBar>
    </Box>
  );
}
