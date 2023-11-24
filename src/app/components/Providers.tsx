"use client"; // Angiver, at dette er klientkode (f.eks. JavaScript, der kører i en webbrowser).


import { SessionProvider } from "next-auth/react"; 
import React, { ReactNode } from "react";

// Props beskriver de egenskaber (props), komponenten forventer.
interface Props {
  children: ReactNode; // Egenskaben 'children' forventes at være af typen ReactNode, som repræsenterer React-noder, der kan rendres.
}

// Providers  modtager Props som argument.
const Providers = (props: Props) => {
  // Returnerer 'SessionProvider' og placerer 'props.children' indeni det.
  // Dette tillader det indhold, der er omsluttet af denne komponent, at få adgang til NextAuth-sessionen.
  return <SessionProvider>{props.children}</SessionProvider>;
};

// Eksporterer 'Providers'-komponenten som standard for brug i andre filer.
export default Providers;