"use client";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";
import React, { useEffect, useState } from "react";

const Settings = () => {
  const { data: session, status } = useSession();

  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    if (status === "authenticated") {
      setIsReady(true);
    } else if (status === "unauthenticated") {
      redirect("/auth/signin");
    }
  }, [status]);

  // Only render the profile page if isReady is true
  if (!isReady) {
    return null;
  }

  return <h1>Settings</h1>;
};

export default Settings;
