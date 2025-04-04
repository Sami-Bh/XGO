import { Typography } from "@mui/material";
import useAuthentication from "../../../lib/hooks/useAuthentication";

export default function Home() {
    const { instance } = useAuthentication();
    return (
        <>
            <Typography>
                Welcome, {instance.getActiveAccount()?.name}
            </Typography>
            <Typography>
            </Typography>


        </>
    )
}
