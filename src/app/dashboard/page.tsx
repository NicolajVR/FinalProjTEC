"use client";
import { Box, Grid, Paper } from "@mui/material";
import React from "react";
import scss from "./Dashboard.module.scss"
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";

const Dashboard = () => {
    const {data: session} = useSession();
    if(!session){
        redirect("/auth/signin");
    }

    return (
        <Box>
            <Grid container gap={2} className={scss.topCardsContainer} >
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
    )
}
export default Dashboard;