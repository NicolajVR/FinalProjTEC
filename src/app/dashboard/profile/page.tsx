"use client";
import React, { useEffect, useState } from "react";
import { useSession } from "next-auth/react";
import Paper from "@mui/material/Paper";
import {
  Alert,
  AlertProps,
  Avatar,
  Box,
  Button,
  Checkbox,
  FormControlLabel,
  Grid,
  Snackbar,
  TextField,
  Typography,
} from "@mui/material";
import { redirect } from "next/navigation";
import { getUserById, updateUser } from "@/app/lib/user";
import { getProfileById } from "@/app/lib/profile";
import updateProfile from "@/app/lib/updateProfile";
import MuiAlert  from '@mui/material/Alert';

const Profile =  () => {
  //const names = session?.user?.name ? session.user.name.split(" "): [];
  //const firstName = names[0];
  //const lastName = names.length > 1 ? names[names.length - 1] : "";
  const { data: session, status } = useSession();
  const [openError, setOpenError] = React.useState(false);
  const [openSuccess, setOpenSuccess] = React.useState(false);
  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
    date_of_birth: "",
    address: "",
    gender: 1,
    confirmPassword: "",
  });

  useEffect(() => {

    const fetchData = async () => {

      try {
        const user = await getUserById(session?.user.user_id as number);
        const profile = await getProfileById(session?.user.user_id as number);

        setFormData({
          firstName: profile.name,
          lastName: profile.last_name,
          email: user.email,
          phone: profile.phone,
          date_of_birth: profile.date_of_birth,
          address: profile.address,
          gender: profile.gender_id,
          confirmPassword: "",
        });
      } catch (error) {
        // Handle error fetching data
      }
  };

    if (status === "authenticated") {
      setIsReady(true);
      fetchData();
    } else if (status === "unauthenticated") {
      redirect("/auth/signin");
    }
  
  }, [status]);

  // Only render the profile page if isReady is true
  if (!isReady) {
    return null;
  }

  const Alert = React.forwardRef<HTMLDivElement, AlertProps>(function Alert(
    props,
    ref,
  ) {
    return <MuiAlert elevation={6} ref={ref} variant="filled" {...props} />;
  });

  

  const handleFormChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, checked } = event.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: name === "receiveEmails" ? checked : value,
    }));
  };

  const handleCloseError = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setOpenError(false);

  };

  const handleCloseSuccess = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    setOpenSuccess(false);

  };

  



  const handleSubmit = async (event: React.ChangeEvent<HTMLFormElement>) => {
    event.preventDefault();

    const user = await getUserById(session?.user.user_id as number);
    const profile = await getProfileById(session?.user.user_id as number);

    const userData = {
      user_id: user.user_id,
      surname: user.surname,
      email: formData.email,
      password_hash: user.password_hash,
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

    if(formData.confirmPassword == userData.password_hash)
    {
      updateUser(userData,user.user_id);
      updateProfile(profileData,profile.user_information_id);
      setOpenSuccess(true);

    } else{
      setOpenError(true);
    }

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
                      label="Confirm changes with your password"
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
      <Snackbar open={openError} autoHideDuration={6000} onClose={handleCloseError}>
        <Alert onClose={handleCloseError} severity="error" sx={{ width: '100%' }}>
          Password dosen't match 
        </Alert>
      </Snackbar>
      <Snackbar open={openSuccess} autoHideDuration={6000} onClose={handleCloseSuccess}>
        <Alert onClose={handleCloseSuccess} severity="success" sx={{ width: '100%' }}>
          Profile updated
        </Alert>
      </Snackbar>
    </>
  );
};

export default Profile;
