"use client";
import { Box, Grid, Paper } from "@mui/material";
import React, { useEffect, useState } from "react";
import scss from "./Dashboard.module.scss";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";

const Dashboard = () => {
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

  return (
    <Box>
      <Grid container gap={2} className={scss.topCardsContainer}>
        <Grid>
          <Paper className={scss.dataCard}>xs=4</Paper>
        </Grid>
        <Grid>
          <Paper className={scss.dataCard}>xs=4</Paper>
        </Grid>
        <Grid>
          <Paper className={scss.dataCard}>xs=4</Paper>
        </Grid>
      </Grid>
      <Grid item={true} xs={12} marginY={2}>
        <Paper className={scss.dataCard}>xs=12</Paper>
      </Grid>
    </Box>
  );
};
export default Dashboard;
