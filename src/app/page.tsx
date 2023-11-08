"use client";
import { useSession } from "next-auth/react"
import scss from './Home.module.scss'
import React from "react";
import Dashboard from "./dashboard/page";
import SignIn from "./auth/signin/page";
import Calendar from "./dashboard/calendar/page"

const  Home: React.FC =() => {
  const {data: session, status} = useSession();
  // Loading state: Display a loading indicator while the session data is being fetched.
  if (status === "loading") {
    return ;
  }

  return (
    

    <main className={scss.main}>
      {/* Only render the "Calendar" component if the session is available. */}
      {session ? <Calendar /> : <SignIn />}
    </main>
  )
}

export default Home;
