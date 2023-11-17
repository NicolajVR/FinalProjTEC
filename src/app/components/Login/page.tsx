"use client";
import { signOut, useSession } from "next-auth/react";
import React, { useState } from "react";
import { Paper, Typography, TextField, Button } from '@mui/material';
import styled from '@emotion/styled';
import { login } from "@/app/lib/user";
import scss from "./Login.module.scss"

const CenteredContainer = styled.div``;

//styledpaper for loginform
const StyledPaper = styled(Paper)``;

// individuel text field
const StyledTextField = styled(TextField)``;

// button
const StyledButton = styled(Button)``;



const Login: React.FC = () => {
  const { data: session } = useSession();
  const [username, setUsername] = useState(""); // Add this line
  const [password, setPassword] = useState(""); // Add this line

  const handleLogin = async () => {
    const userData = {
      surname: username,
      password_hash: password,
    };

    console.log(userData);
    
    //await login(userData);
  };

  const handleUsernameChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setUsername(event.target.value); // Add this function
  };
  const handlePasswordChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setPassword(event.target.value); // Add this function
  };

  if (session) {
    return (
      <>
        <Button variant={"contained"} color={"error"} onClick={() => signOut()}>
          Sign out
        </Button>
      </>
    );
  }
  return (

    <div>
    <CenteredContainer className={scss.CenteredContainer} >
      <StyledPaper elevation={3} className={scss.StyledPaper}>
        <Typography variant="h5" align="center" gutterBottom>
          Login
        </Typography>
        <form style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
          <StyledTextField className={scss.StyledTextField} label="Username" variant="outlined" fullWidth onChange={handleUsernameChange} />
          <StyledTextField className={scss.StyledTextField} label="Password" type="password" variant="outlined" fullWidth onChange={handlePasswordChange} />
          <StyledButton className={scss.StyledButton} variant="contained" color="primary" fullWidth onClick={handleLogin}>
            Login
          </StyledButton>
        </form>
      </StyledPaper>
    </CenteredContainer>
  </div>









  );
};

export default Login;