"use client";
import React from "react";
import Login from "@/app/components/Login/page";
import { useSession } from "next-auth/react";
import Box from "@mui/material/Box";

const SignIn = () => {

  return (
      <Login />
  );
};

export default SignIn;