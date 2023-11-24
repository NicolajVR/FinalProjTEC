"use client"; // Dette markerer, at denne fil kun skal køres på klienten (browseren), ikke på serveren.

// Importér nødvendige biblioteker og komponenter fra React og andre moduler
import React from "react";
import Login from "@/app/components/Login/page"; 

// Definér funktionen SignIn 
const SignIn = () => {

  return (
    // Returnér Login-komponenten, som  indeholder loginformularen
      <Login /> 
  );
};

// Eksporter SignIn-komponenten som standard, så den kan bruges andre steder i koden
export default SignIn; 
