/*

Koden har en knap til at bekræfte fravær, en DataGrid til visning af brugerdata, dialoger til at angive fraværsoplysninger, og bruger autocompletes til at vælge klasser og tilhørende brugere.

Komponenten bruger hooks som useState, useEffect til at styre tilstanden og håndtere livscyklus-hændelser for datahentning og opdateringer.

DataGrid'en viser brugerinformation, og der er logik til at redigere rækker og vælge bestemte brugere fra grid'en til fraværsregistrering.

Derudover er der brugt dialoger til at indtaste fraværsoplysninger og knapper til at udløse disse dialoger. Snackbar-komponenten bruges til at vise beskeder om succes eller fejl ved fraværsoprettelse.

*/



"use client";
import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
//import AddIcon from '@mui/icons-material/Add';
import AddBoxIcon from "@mui/icons-material/AddBox";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/DeleteOutlined";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import {
  GridRowsProp,
  GridRowModesModel,
  GridRowModes,
  DataGrid,
  GridColDef,
  GridToolbarContainer,
  GridActionsCellItem,
  GridEventListener,
  GridRowId,
  GridRowModel,
  GridRowEditStopReasons,
} from "@mui/x-data-grid";
import {
  randomCreatedDate,
  randomTraderName,
  randomId,
  randomArrayItem,
} from "@mui/x-data-grid-generator";
import { redirect } from "next/navigation";
import { useEffect, useState } from "react";
import getProfiles from "@/app/lib/getProfiles";
import { useSession } from "next-auth/react";
import Autocomplete from "@mui/material/Autocomplete";
import TextField from "@mui/material/TextField";
import Chip from "@mui/material/Chip";
import { getClassesFromUser, getUsersFromClass } from "@/app/lib/enrollment";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import { Checkbox, Typography } from "@mui/material";
import FormControlLabel from "@mui/material/FormControlLabel";
import { createAbsence } from "@/app/lib/absence";

// Initialisering af konstanter og hooks
import Snackbar from "@mui/material/Snackbar";
import MuiAlert, { AlertProps } from "@mui/material/Alert";

let options: string[] = [];

// Data til brug i DataGrid komponenten
const initialRows: GridRowsProp = [
  {
    id: randomId(),
    name: randomTraderName(),
    last_name: 25,
    is_deleted: false,
    gender_id: 1,
    user_id: 2,
  },
];

interface EditToolbarProps {
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel
  ) => void;
}

function EditToolbar(props: EditToolbarProps) {
    // Returnerer en værktøjscontainer til brug i grid'en
  const { setRows, setRowModesModel } = props;
  return <GridToolbarContainer></GridToolbarContainer>;
}

// Hovedkomponenten
export default function FullFeaturedCrudGrid() {
  // Initialiserer flere React-hooks til brug i komponenten
  const [hour, setHour] = React.useState<number | number>(0);
  const [minutes, setMinutes] = React.useState<number | number>(0);
  const [reason, setReason] = React.useState<string>("");
  const [open, setOpen] = React.useState(false);
  const [selectedID, setSelectedID] = React.useState("no user selected");
  const [selected, setSelected] = React.useState("User not selected");
  const [value, setValue] = React.useState<string | null>(null); // Set initial state to null
  const [inputValue, setInputValue] = React.useState("");
  const [rows, setRows] = React.useState(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>(
    {}
  );
  const [snackbarOpen, setSnackbarOpen] = React.useState<boolean>(false);
  const [snackbarMessage, setSnackbarMessage] = React.useState<string>("");
  const [successSnackbarOpen, setSuccessSnackbarOpen] =
    React.useState<boolean>(false);
  const [successSnackbarMessage, setSuccessSnackbarMessage] =
    React.useState<string>("");

  // åbne error dialog box 
  const handleSnackbarOpen = (message: string) => {
    setSnackbarMessage(message);
    setSnackbarOpen(true);
  };

  // åbne success dialog box
  const handleSuccessSnackbarOpen = (message: string) => {
    setSuccessSnackbarMessage(message);
    setSuccessSnackbarOpen(true);
  };

  //Tjek Attendence 
  const [noAttendanceChecked, setNoAttendanceChecked] = React.useState(false);

  const { data: session, status } = useSession();

  // 
  const [isReady, setIsReady] = useState(false);

   // En række useEffect-hooks anvendes til at håndtere datahentning og opdatering
  useEffect(() => {
    const findIdByName = (
      classesArray: any[],
      classNameToFind: string
    ): number | undefined => {
      const foundClass = classesArray.find(
        (item) => item.enrollmentClassResponse.className === classNameToFind
      );
      return foundClass?.enrollmentClassResponse.id;
    };

    console.log("Autocomplete value changed:", value);

    const fetchData = async () => {
      //(logik til at hente og opdatere data)
      if (value != null) {
        const classes = await getClassesFromUser(
          session?.user.user_id as number,
          session?.user.token
        );

        const classData = findIdByName(classes, value as string);
        const userGetData = await getUsersFromClass(
          classData as number,
          session?.user.token
        );
        const classMembers = await userGetData.filter(
          (item: any) =>
            item.enrollmentUserResponse.user_id !== session?.user.user_id
        );
        console.log("here we romoved teacher: ", classMembers);

        console.log("here is userGetData: ", userGetData);

        if (classMembers.length > 0) {
          setRows(
            classMembers.map((user: any) => ({
              id: user.enrollmentUserResponse.user_id,
              name: user.enrollmentUserResponse.surname,
              email: user.enrollmentUserResponse.email,
            }))
          );
        } else {
          // Clean rows here
          setRows([]);
        }
      }
    };
    fetchData();
  }, [value] ); // Udføres når værdien af 'value' ændres

  useEffect(() => {
    const fetchData = async () => {
      const classes = await getClassesFromUser(
        session?.user.user_id as number,
        session?.user.token
      );
      if(classes){
      options = classes.map(
        (item: { enrollmentClassResponse: { className: any } }) =>
          item.enrollmentClassResponse.className
      );

      setValue(options[0]);
      setInputValue(options[0]);
      }
    };

    // hvis session er authenticated so vis data hvis ikke så redirect til /auth/signin
    if (status === "authenticated") {
      if (session.user.role_id === 2)
      {
        setIsReady(true);
        fetchData();
      }else{
        redirect("/pages/calendar");
      }
    } else if (status === "unauthenticated") {
      redirect("/auth/signin");
    }

    console.log(rows[0]);
  }, [status]); // Udføres når 'status' ændres

  // Only render the profile page if isReady is true
  if (!isReady) {
    return null;
  }

  //absence confirm - lav "Success" efter POST (hvis OK), - Uncheck Checkbox. - lav man kun kan vælge en.
  const handleSubmit = async () => {
    if (!areInputValuesValid()) {
      return;
    }

    const findIdByName = (
      classesArray: any[],
      classNameToFind: string
    ): number | undefined => {
      const foundClass = classesArray.find(
        (item) => item.enrollmentClassResponse.className === classNameToFind
      );
      return foundClass?.enrollmentClassResponse.id;
    };

    if (value != null) {
      const classes = await getClassesFromUser(
        session?.user.user_id as number,
        session?.user.token
      );

      const classData = findIdByName(classes, value as string);

      // Get the current date
      const currentDate: Date = new Date();

      // Extract individual date components
      const year: number = currentDate.getFullYear();
      const month: number = currentDate.getMonth() + 1; // Months are zero-indexed
      const day: number = currentDate.getDate();

      // Format the date as "YYYY-MM-DD"
      const formattedDate: string = `${year}-${
        month < 10 ? "0" + month : month
      }-${day < 10 ? "0" + day : day}`;

      // Output the formatted date
      console.log(formattedDate);

      console.log("Reason submitted:", reason);
      console.log("Hours submitted:", hour);
      console.log("Minutes submitted:", minutes);

      let absence = formattedDate + " hours: " + hour + " minuttes " + minutes;
      // Log a message if the "No Attendance" checkbox is checked
      if (noAttendanceChecked == true) {
        absence = formattedDate + " hours: " + 8;
      }

      const absenceData = {
        user_id: selectedID,
        teacher_id: session?.user.user_id,
        class_id: classData,
        absence_date: absence, //formattedDate + " time: " + hour + " minuttes " + minutes,
        reason: reason,
      };

      createAbsence(absenceData,session?.user.token)
        .then(() => {
          // Handle successful POST
          handleSuccessSnackbarOpen("Absence created successfully!");
          setOpen(false);
          setNoAttendanceChecked(false);
        })
        .catch((error) => {
          // Handle errors if needed
        });
    }
  };

  // Check if the input values are valid
  const areInputValuesValid = () => {
    //kunne også have brugt en SWITCH

    if (selectedID === "no user selected") {
      handleSnackbarOpen(
        "Please select a student before confirming the absence."
      );
      return false;
    }

    if (
      !noAttendanceChecked &&
      hour === 0 &&
      minutes === 0 &&
      reason.trim() !== ""
    ) {
      handleSnackbarOpen(
        'Please select "No Attendance" or make changes to the hour or minutes.'
      );
      return false;
    }

    if (noAttendanceChecked && reason.trim() !== "") {
      // Allow confirmation if "No Attendance" is checked and a reason is provided
      return true;
    }

    if (noAttendanceChecked && (hour !== 0 || minutes !== 0)) {
      handleSnackbarOpen(
        'Cannot select "No Attendance" if hours or minutes have been changed.'
      );
      return false;
    }

    if (hour === 0 && minutes === 0 && reason.trim() !== "") {
      handleSnackbarOpen(
        "Please make changes to the hour or minutes before confirming the absence."
      );
      return false;
    }

    if (hour < 0 || hour > 8 || isNaN(hour)) {
      handleSnackbarOpen(
        "Invalid hours. Please enter a value between 0 and 8."
      );
      return false;
    }

    if (minutes < 0 || minutes >= 60 || isNaN(minutes)) {
      handleSnackbarOpen(
        "Invalid minutes. Please enter a value between 0 and 59."
      );
      return false;
    }

    if (hour === 8 && minutes !== 0) {
      handleSnackbarOpen("If hours are set to 8, minutes must be set to 0.");
      return false;
    }

    if (!reason.trim()) {
      handleSnackbarOpen("Please provide a reason for the absence.");
      return false;
    }
    console.log("her er hour :", hour);
    return true;
  };

   // Funktioner til at åbne og lukke dialogvinduet for fravær
  const handleClickOpen = () => {
    setOpen(true);
    setHour(0); // Reset hour to 0 when opening the dialog
    setMinutes(0); // Reset minutes to 0 when opening the dialog
    setReason(""); // Reset reason to an empty string when opening the dialog
  };
  //absence close
  const handleClose = () => {
    setOpen(false);
    setNoAttendanceChecked(false);
  };

  const handleRowEditStop: GridEventListener<"rowEditStop"> = (
    params,
    event
  ) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };


  const handleCancelClick = (id: GridRowId) => () => {
    setRowModesModel({
      ...rowModesModel,
      [id]: { mode: GridRowModes.View, ignoreModifications: true },
    });

    const editedRow = rows.find((row) => row.id === id);
    if (editedRow!.isNew) {
      setRows(rows.filter((row) => row.id !== id));
    }
  };

   // Event handler til at håndtere valg af rækker i grid'en
  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

   // Definition af kolonner i grid'en
  const columns: GridColDef[] = [
    { field: "name", headerName: "Name", width: 120, editable: true },
    {
      field: "email",
      headerName: "Email",
      width: 180,
      editable: false,
    },
  ];

  const onRowsSelectionHandler = (ids: any[]) => {
    const selectedRowsData = ids.map((id) => rows.find((row) => row.id === id));
    setSelected(selectedRowsData[0]?.name);
    setSelectedID(selectedRowsData[0]?.id);
    console.log("this is the selected row: ", selectedRowsData[0]?.name);
  };

  // Returnerer JSX-koden for den komplette komponent med knapper, grid og dialoger
  return (
    <>
      <h1>Student Absence</h1>

      <React.Fragment>
        <div style={{ display: "flex" }}>
          <Button
            color="primary"
            variant="contained"
            startIcon={<AddBoxIcon />}
            onClick={handleClickOpen}
            sx={{ borderRadius: 0, boxShadow: 0 }}
          >
            Absence
          </Button>

          <Autocomplete
            value={value}
            onChange={(event: any, newValue: string | null) => {
              setValue(newValue);
            }}
            inputValue={inputValue}
            onInputChange={(event, newInputValue) => {
              setInputValue(newInputValue);
            }}
            id="controllable-states-Klasse"
            options={options}
            sx={{
              width: 200,
              "& .MuiAutocomplete-inputRoot": {
                borderRadius: 0,
              },
            }}
            renderInput={(params) => <TextField {...params} label="Class" />}
            renderOption={(props, option) => (
              <li {...props} key={option}>
                {option}
              </li>
            )}
            renderTags={(tagValue, getTagProps) =>
              tagValue.map((option, index) => (
                <Chip {...getTagProps({ index })} key={option} label={option} />
              ))
            }
          ></Autocomplete>
        </div>

        <Dialog open={open} onClose={handleClose}>
          <DialogTitle> {selected} </DialogTitle>
          <DialogContent
            style={{
              display: "flexbox",
              flexDirection: "column",
              alignItems: "center",
            }}
          >
            <TextField
              autoFocus
              margin="dense"
              id="reasson"
              label="reason.. "
              type="text"
              fullWidth
              variant="filled"
              style={{ margin: "10px" }}
              value={reason}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setReason(e.target.value)
              }
            />
            <Typography> arrived </Typography>
            <TextField
              autoFocus
              margin="dense"
              id="hour"
              label=" Hours... "
              type="number"
              variant="filled"
              style={{ margin: "10px" }}
              // olderVersion --> value={hour !== undefined ? hour : ''}
              value={isNaN(hour) ? "" : hour}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setHour(parseInt(e.target.value, 10))
              }
            />
            <Typography> and </Typography>
            <TextField
              autoFocus
              margin="dense"
              id="minutes"
              label=" Minutes..."
              type="number"
              variant="filled"
              style={{ margin: "10px" }}
              // olderVersion --> value={minutes !== undefined ? minutes : ''}
              value={isNaN(minutes) ? "" : minutes}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setMinutes(parseInt(e.target.value, 10))
              }
            />
            <Typography> late </Typography>
            <FormControlLabel
              control={
                <Checkbox
                  checked={noAttendanceChecked}
                  onChange={(e) => setNoAttendanceChecked(e.target.checked)}
                />
              }
              label="No Attendance"
              style={{ margin: "10px" }}
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleClose}>Cancel</Button>
            <Button onClick={handleSubmit}>Confirm</Button>
          </DialogActions>
        </Dialog>
      </React.Fragment>

      <Box
        sx={{
          height: 500,
          width: "100%",
          "& .actions": {
            color: "text.secondary",
          },
          "& .textPrimary": {
            color: "text.primary",
          },
        }}
      >
        <DataGrid
          rows={rows}
          columns={columns}
          //editMode="row"
          checkboxSelection //de-select af CANCEL.
          onRowSelectionModelChange={(ids) => onRowsSelectionHandler(ids)}
          rowModesModel={rowModesModel}
          onRowModesModelChange={handleRowModesModelChange}
          onRowEditStop={handleRowEditStop}
          slots={{
            toolbar: EditToolbar,
          }}
          slotProps={{
            toolbar: { setRows, setRowModesModel },
          }}
          sx={{ borderRadius: 0 }}
        />
      </Box>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000} // Adjust as needed
        onClose={() => setSnackbarOpen(false)}
      >
        <MuiAlert
          elevation={6}
          variant="filled"
          onClose={() => setSnackbarOpen(false)}
          severity="error"
        >
          {snackbarMessage}
        </MuiAlert>
      </Snackbar>
      <Snackbar
        open={successSnackbarOpen}
        autoHideDuration={6000} // Adjust as needed
        onClose={() => setSuccessSnackbarOpen(false)}
      >
        <MuiAlert
          elevation={6}
          variant="filled"
          onClose={() => setSuccessSnackbarOpen(false)}
          severity="success" // Set severity to success
        >
          {successSnackbarMessage}
        </MuiAlert>
      </Snackbar>
    </>
  );
}
