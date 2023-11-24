"use client"; // Markerer, at denne fil kun skal køres på klientens side, ikke på serveren
import { useSession, signIn, signOut } from "next-auth/react"
import scss from "./Login.module.scss";
import styled from '@emotion/styled';
import { Paper, Typography, TextField, Button } from '@mui/material';
import { redirect } from "next/navigation";

// Definér Login-komponenten
const Login = () => {
  // Brug af React-hook til at få sessionsdata
    const { data: session } = useSession();
    const CenteredContainer = styled.div``;

// Styled-komponenter til at tilpasse udseendet af Material-UI komponenter
const StyledPaper = styled(Paper)``;


const StyledButton = styled(Button)``;

 // Hvis der er en aktiv session, omdiriger til "/pages/calendar"
    if(session) {
        return redirect("/pages/calendar");
        
    }
    return <>
    {/* Login-formular med styled Material-UI komponenter */}
        <div>
    <CenteredContainer className={scss.CenteredContainer} >
      <StyledPaper elevation={3} className={scss.StyledPaper}>
        <Typography variant="h5" align="center" gutterBottom>
          Login
        </Typography>
        <form style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
          <StyledButton className={scss.StyledButton} variant="contained" color="primary" fullWidth onClick={() => signIn()}>Sign in
          </StyledButton>
        </form>
      </StyledPaper>
    </CenteredContainer>
  </div>

    </>
}

export default Login;