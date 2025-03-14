import { Outlet, useLocation } from 'react-router'
import './App.css'
import Navbar from '../app/layout/Navbar';
import { Box, Container, CssBaseline } from '@mui/material';
import Home from '../app/features/home/Home';

function App() {
  const { pathname } = useLocation();
  return (
    <Box sx={{ background: "lightgrey" }}>
      <CssBaseline />
      {(pathname === '') ?
        <Home /> :
        <>
          <Navbar />
          <Container maxWidth="xl" sx={{ mt: 3 }}>
            <Outlet />
          </Container>
        </>
      }
    </Box>

  )
}

export default App
