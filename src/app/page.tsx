"use client";
import { useSession } from "next-auth/react"
import scss from './Home.module.scss'
import React from "react";
import Dashboard from "./dashboard/page";
import SignIn from "./auth/signin/page";

const  Home: React.FC =() => {
  const {data: session} = useSession();
  return (

    <main className={scss.main}>
      {session && <Dashboard />}
      {!session && <SignIn />}
    </main>
  )
}

export default Home;
