"use client"; // Markerer, at denne fil kun skal køres på klientens side, ikke på serveren

import SideMenu from "../SideMenu/page"; 
import scss from "./Layout.module.scss"; 
import { useSession } from "next-auth/react"; 
import React from "react"; 
import Head from "next/head"; 

// Definér Layout-komponenten med props
const Layout = (props: any) => {
  // Brug af React-hook til at få sessionsdata
  const { data: session } = useSession(); 

  return (
    <> {/* Fragment bruges til at returnere flere elementer uden et overordnet HTML-element */}
      {/* Head-komponenten indstiller dokumentets metadata og titel */}
      <Head>
        <title>School</title>
        <meta name="description" content="Data Dashboard" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      {/* Hovedindholdet af Layout-komponenten */}
      <main className={scss.layout}>
        {/* SideMenu-komponenten vises kun, hvis der er en aktiv session */}
        {session && <SideMenu />}
        
        {/* Renderer indholdet, der sendes som props til Layout-komponenten */}
        {props.children}
      </main>
    </>
  );
};

export default Layout; // Eksporterer Layout-komponenten som standard, så den kan bruges andre steder i koden
