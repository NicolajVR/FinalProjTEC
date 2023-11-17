"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import Paper from "@mui/material/Paper";
import {
  Avatar,
  Box,
  Button,
  Checkbox,
  FormControlLabel,
  Grid,
  TextField,
  Typography,
} from "@mui/material";
import { redirect } from "next/navigation";
import { getUserById, updateUser } from "@/app/lib/user";
import { getProfileById } from "@/app/lib/profile";
import updateProfile from "@/app/lib/updateProfile";

const Profile =  () => {
  //const names = session?.user?.name ? session.user.name.split(" "): [];
  //const firstName = names[0];
  //const lastName = names.length > 1 ? names[names.length - 1] : "";
  const { data: session, status } = useSession();
  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phone: "",
    date_of_birth: "",
    address: "",
    gender: 1,
    confirmPassword: "",
  });

  useEffect(() => {
    const fetchData = async () => {
      if (status === "authenticated") {
        try {
          setIsReady(true);
          const user = await getUserById(1);
          const profile = await getProfileById(1);

          setFormData({
            firstName: profile.name,
            lastName: profile.last_name,
            email: user.email,
            password: user.password_hash,
            phone: profile.phone,
            date_of_birth: profile.date_of_birth,
            address: profile.address,
            gender: profile.gender_id,
            confirmPassword: "",
          });
        } catch (error) {
          // Handle error fetching data
        }
      } else if (status === "unauthenticated") {
        redirect("/auth/signin");
      }
    };

    fetchData();
  }, [status]);

  // Only render the profile page if isReady is true
  if (!isReady) {
    return null;
  }

  const handleFormChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, checked } = event.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: name === "receiveEmails" ? checked : value,
    }));
  };

  const handleSubmit = async (event: React.ChangeEvent<HTMLFormElement>) => {
    event.preventDefault();

    const user = await getUserById(1);
    const profile = await getProfileById(1);

    const userData = {
      user_id: user.user_id,
      surname: user.surname,
      email: formData.email,
      password_hash: formData.password,
      is_deleted: user.is_deleted,
    };

    

    const profileData = {
      user_information_id: profile.user_information_id,
      name: formData.firstName,
      last_name: formData.lastName,
      phone: formData.phone,
      date_of_birth: formData.date_of_birth,
      address: formData.address,
      is_deleted: profile.is_deleted,
      gender_id: profile.gender_id,
      user_id: profile.user_id,
    };

    updateUser(userData,user.user_id);
    updateProfile(profileData,profile.user_information_id);

    console.log(formData); // Submit form data to server here





  };
  return (
    <>
      <Box>
        <Typography variant={"h4"} sx={{ paddingBottom: 4 }}>
          Hey {session ? session?.user?.surname : "User"}, welcome to your profile
          👋
        </Typography>
        <Paper sx={{ padding: "1rem 2rem" }}>
          <Grid container justifyContent="center">
            <Grid item xs={12} sm={8} md={6}>
              <Box display="flex" flexDirection="column" alignItems="center">
              </Box>
              <form
                onSubmit={handleSubmit}
                style={{ maxWidth: 600, margin: "0 auto" }}
              >
                <Grid container spacing={3}>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      required
                      fullWidth
                      label="First Name"
                      name="firstName"
                      value={formData.firstName}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12} sm={6}>
                    <TextField
                      required
                      fullWidth
                      label="Last Name"
                      name="lastName"
                      value={formData.lastName}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <TextField
                      required
                      fullWidth
                      type="email"
                      label="Email"
                      name="email"
                      value={formData.email}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <TextField
                      required
                      fullWidth
                      type="phone"
                      label="Phone"
                      name="phone"
                      value={formData.phone}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <TextField
                      required
                      fullWidth
                      type="date_of_birth"
                      label="date_of_birth"
                      name="date_of_birth"
                      value={formData.date_of_birth}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <TextField
                      required
                      fullWidth
                      type="address"
                      label="address"
                      name="address"
                      value={formData.address}
                      onChange={handleFormChange}
                    />
                  </Grid>

                  <Grid item xs={12}>
                    <TextField
                      required
                      fullWidth
                      type="password"
                      label="Password"
                      name="password"
                      value={formData.password}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <TextField
                      required
                      fullWidth
                      type="password"
                      label="Confirm Password"
                      name="confirmPassword"
                      value={formData.confirmPassword}
                      onChange={handleFormChange}
                    />
                  </Grid>
                  <Grid item xs={12}>
                    <Button type="submit" variant="contained" color="primary">
                      Save Changes
                    </Button>
                  </Grid>
                </Grid>
              </form>
            </Grid>
          </Grid>
        </Paper>
      </Box>
    </>
  );
};

export default Profile;
