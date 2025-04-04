import { Outlet, useLocation } from 'react-router'
import './App.css'
import Navbar from '../app/layout/Navbar';
import { Box, Container, CssBaseline } from '@mui/material';
import Home from '../app/features/home/Home';
import { AuthenticatedTemplate, MsalProvider, UnauthenticatedTemplate } from '@azure/msal-react';
import { PUBLIC_CLIENT_APPLICATION } from '../msalConfig';
import AuthenticationPage from './AuthenticationPage';

function App() {
  const { pathname } = useLocation();
  return (
    <MsalProvider instance={PUBLIC_CLIENT_APPLICATION}>
      <Box sx={{ background: "lightgrey", minHeight: "100vh" }}>
        <CssBaseline />
        <AuthenticatedTemplate>


          {(pathname === '') ?
            <Home /> :
            <>
              <Navbar />
              <Container maxWidth="xl" sx={{ mt: 3 }}>
                <Outlet />
              </Container>
            </>
          }
        </AuthenticatedTemplate>
        <UnauthenticatedTemplate>
          <AuthenticationPage />
        </UnauthenticatedTemplate>

      </Box>
    </MsalProvider>
  )
}

export default App
