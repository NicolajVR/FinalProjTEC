"use client";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";
import React from "react";

const Settings = () => {
    const {data: session} = useSession();
    if(!session){
        redirect("/auth/signin");
    }

    return (
        <h1>Settings</h1>
    )
}

export default Settings;