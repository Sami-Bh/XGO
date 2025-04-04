import { Box, Button, Paper, Typography } from '@mui/material'
import useAuthentication from '../lib/hooks/useAuthentication'
import MicrosoftIcon from '@mui/icons-material/Microsoft';
export default function AuthenticationPage() {

    const { handleSignIn } = useAuthentication();
    return (
        <Paper sx={{
            height: 400,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            p: 3,
            gap: 3,
            maxWidth: 'md',
            mx: 'auto',
            borderRadius: 3
        }}>
            <Box sx={{ display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
                <Typography>Welcome to XGo, please sign in</Typography>
                <Button startIcon={<MicrosoftIcon />}
                    variant="contained"
                    size="large"
                    onClick={() => { handleSignIn(); }}>
                    Sign in
                </Button>
            </Box>

        </Paper>

    )
}
