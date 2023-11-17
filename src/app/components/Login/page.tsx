"use client";
import { useSession, signIn, signOut } from "next-auth/react"
import scss from "./Login.module.scss";
import styled from '@emotion/styled';
import { Paper, Typography, TextField, Button } from '@mui/material';
import { redirect } from "next/navigation";

const Login = () => {
    const { data: session } = useSession();
    const CenteredContainer = styled.div``;

//styledpaper for loginform
const StyledPaper = styled(Paper)``;

// individuel text field
const StyledTextField = styled(TextField)``;

// button
const StyledButton = styled(Button)``;


    if(session) {
        return redirect("/dashboard/calendar");
        
    }
    return <>
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