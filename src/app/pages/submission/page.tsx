"use client";
import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/DeleteOutlined";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import TextField, { textFieldClasses } from "@mui/material/TextField";
import Autocomplete from "@mui/material/Autocomplete";
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
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { redirect } from "next/navigation";
import { getClassesFromUser } from "@/app/lib/enrollment";
import { getsubjects } from "@/app/lib/subject";
import {
  Chip,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";
import {
  GetAssignments,
  createAssignment,
  deleteAssignment,
  updateAssignment,
} from "@/app/lib/Assignment";
import {
  createSubmission,
  deleteSubmission,
  getSubmissions,
} from "@/app/lib/userSubmisson";
import { stringify } from "querystring";
import { fetchData } from "next-auth/client/_utils";

const roles = ["Market", "Finance", "Development"];
const randomRole = () => {
  return randomArrayItem(roles);
};

let options: string[] = [];

const initialRows: GridRowsProp = [
  {
    id: randomId(),
    name: randomTraderName(),
    age: 25,
    joinDate: randomCreatedDate(),
    role: randomRole(),
  },
];

export default function FullFeaturedCrudGrid() {
  const [value, setValue] = React.useState<string | null>(null); // Set initial state to null
  const [classId, setclassId] = React.useState(1);
  const [assignment, setAssignmentId] = React.useState(1);
  const [open, setOpen] = React.useState(false);
  const [inputValue, setInputValue] = React.useState("");
  const [answer, setAnswer] = React.useState("");
  const [rowId, setRowId] = React.useState(1);
  const [rows, setRows] = React.useState(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>(
    {}
  );

  const { data: session, status } = useSession();

  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    // Function to find enrollmentClassResponse id by className

    rows.forEach(async (row) => {
      const findIdByName = (
        classesArray: any[],
        classNameToFind: string
      ): number | undefined => {
        const foundClass = classesArray.find(
          (item) => item.enrollmentClassResponse.className === classNameToFind
        );
        return foundClass?.enrollmentClassResponse.id;
      };

      if (row.isNew && row.assignment_description) {
        const classes = await getClassesFromUser(
          session?.user.user_id as number,
          session?.user.token
        );
        const subjects = await getsubjects();

        const classData = findIdByName(classes, value as string);

        const subjectData = subjects.find(
          (item: any) => item.subjectname === value
        );

        const userData = {
          classeId: classData,
          subjectId: subjectData.id,
          assignment_Deadline: row.assignment_deadline,
          assignment_Description: row.assignment_description,
        };

        await createAssignment(userData, session?.user.token);

        row.isNew = false;
        //console.log("token: ", session?.user.token);

        console.log("here: ", classData);
        console.log("here subject: ", subjectData.id);

        //options2 = subjects.map((subject: any) => subject.subjectname);
      }
    });
  }, [rows]);

  React.useEffect(() => {

    console.log("Autocomplete value changed:", value);

    const fetchData = async () => {
      if (value != null) {
        const classes = await getClassesFromUser(
          session?.user.user_id as number,
          session?.user.token
        );

        if (classes != undefined){
        const subjects = await getsubjects();

        //const classData =  findIdByName(classes, value as string);

        const subjectData = await subjects.find(
          (item: any) => item.subjectname === value
        );

        console.log("here: ", classId, subjectData.id);

        const assignments = await GetAssignments();

        const submissions = await getSubmissions(session?.user.token);

        // Filter submissions for opgave_Id where user_id is 1
        const opgaveIdsForUser = submissions
          .filter(
            (submission: any) =>
              submission.userSubmissionUserResponse.user_id ===
              session?.user.user_id
          )
          .map(
            (submission: any) =>
              submission.userSubmissionAssignmentResponse.opgave_Id
          );

        console.log("the ids:", opgaveIdsForUser); // Array of opgave_Id values belonging to user_id 1

        const assignment_c_s = await assignments.filter(
          (item: any) =>
            item.classe.class_id == classId &&
            item.subjects.subject_id == subjectData.id &&
            item.is_deleted == false &&
            !opgaveIdsForUser.includes(item.assignment_id)
        );

        console.log("FINAL: ", assignment_c_s);

        if (assignment_c_s.length > 0) {
          setRows(
            assignment_c_s.map((assignment: any) => ({
              id: assignment.assignment_id,
              assignment_description: assignment.assignment_description,
              assignment_deadline: assignment.assignment_deadline,
            }))
          );
        }else {
          // Clean rows here
          setRows([]);
        }
      
      } else {
          // Clean rows here
          setRows([]);
        }



      }
    };
    fetchData();
  }, [value]);

  useEffect(() => {
    const fetchData = async () => {
      //console.log("TOKEN: ",session?.user.token);
      const classes = await getClassesFromUser(
        session?.user.user_id as number,
        session?.user.token
      );
      const subjects = await getsubjects();
      //console.log("token: ", session?.user.token);

      options = subjects.map((subject: any) => subject.subjectname);

      if(classes != undefined)
      { 
      const getId = classes[0].enrollmentClassResponse?.id;

      console.log("class: ", getId);
      setclassId(classes[0].enrollmentClassResponse.id);

      setValue(options[0]);
      setInputValue(options[0]);

      console.log(classes);
      console.log("look: ", options);
      }else{
        setRows([]);
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

  const handleRowEditStop: GridEventListener<"rowEditStop"> = (
    params,
    event
  ) => {
    if (params.reason === GridRowEditStopReasons.rowFocusOut) {
      event.defaultMuiPrevented = true;
    }
  };

  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } });
  };

  const handleDeleteClick = (id: GridRowId) => async () => {
    setOpen(true);

    setAssignmentId(id as number);
    //setRows(rows.filter((row) => row.id !== id));
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

  const handleClose = async () => {

    if (answer != null && answer.length > 4)
    {
    const currentDate = new Date(); // This will log the current date and time

    // You can also extract specific components of the date/time if needed
    const year = currentDate.getFullYear();
    const month = currentDate.getMonth() + 1; // Note: Month is zero-indexed, so January is 0
    const day = currentDate.getDate();
    const hours = currentDate.getHours();
    const minutes = currentDate.getMinutes();
    const seconds = currentDate.getSeconds();

    const submissionData = {
      userSubmission_text: answer,
      userSubmission_date: `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`,
      userId: session?.user.user_id,
      assignmentId: assignment,
    };

    console.log(submissionData);
    await createSubmission(submissionData);
    setRows(rows.filter((row) => row.id !== assignment));
}
    setOpen(false);
  };

  const handleCurrentClick = async () => {

    

    if (value != null) {
        const classes = await getClassesFromUser(
          session?.user.user_id as number,
          session?.user.token
        );

        const subjects = await getsubjects();

        //const classData =  findIdByName(classes, value as string);

        const subjectData = await subjects.find(
          (item: any) => item.subjectname === value
        );

        console.log("here: ", classId, subjectData.id);

        const assignments = await GetAssignments();

        const submissions = await getSubmissions(session?.user.token);

        // Filter submissions for opgave_Id where user_id is 1
        const opgaveIdsForUser = submissions
          .filter(
            (submission: any) =>
              submission.userSubmissionUserResponse.user_id ===
              session?.user.user_id
          )
          .map(
            (submission: any) =>
              submission.userSubmissionAssignmentResponse.opgave_Id
          );

        console.log("the ids:", opgaveIdsForUser); // Array of opgave_Id values belonging to user_id 1

        const assignment_c_s = await assignments.filter(
          (item: any) =>
            item.classe.class_id == classId &&
            item.subjects.subject_id == subjectData.id &&
            item.is_deleted == false &&
            !opgaveIdsForUser.includes(item.assignment_id)
        );

        console.log("FINAL: ", assignment_c_s);

        if (assignment_c_s.length > 0) {
          setRows(
            assignment_c_s.map((assignment: any) => ({
              id: assignment.assignment_id,
              assignment_description: assignment.assignment_description,
              assignment_deadline: assignment.assignment_deadline,
            }))
          );
        } else {
          // Clean rows here
          setRows([]);
        }
      }
  };

  const handleSubmitedClick = async () => {

    if (value != null) {

        const subjects = await getsubjects();

        //const classData =  findIdByName(classes, value as string);

        const subjectData = await subjects.find(
          (item: any) => item.subjectname === value
        );

        console.log("here: ", classId, subjectData.id);

        const assignments = await GetAssignments();

        const submissions = await getSubmissions(session?.user.token);


        // Filter submissions for opgave_Id where user_id is given
        const opgaveIdsForUser = submissions
          .filter(
            (submission: any) =>
              submission.userSubmissionUserResponse.user_id ===
              session?.user.user_id
          )
          .map(
            (submission: any) =>
              submission.userSubmissionAssignmentResponse.opgave_Id
          );



        console.log("the ids:", session?.user.user_id); // Array of opgave_Id values belonging to user_id 1

        const assignment_c_s = await assignments.filter(
          (item: any) =>
            item.classe.class_id == classId &&
            item.subjects.subject_id == subjectData.id &&
            item.is_deleted == false &&
            opgaveIdsForUser.includes(item.assignment_id)
        );


        console.log("FINAL: ", assignment_c_s);

        if (assignment_c_s.length > 0) {
          setRows(
            assignment_c_s.map((assignment: any) => ({
              id: assignment.assignment_id,
              assignment_description: assignment.assignment_description,
              assignment_deadline: assignment.assignment_deadline,
            }))
          );
        } else {
          // Clean rows here
          setRows([]);
        }
      }

  };

  const handleProcessRowUpdate = (updatedRow: any, originalRow: any) => {
    // Find the index of the row that was edited
    const rowIndex = rows.findIndex((row) => row.id === updatedRow.id);

    // Replace the old row with the updated row
    const updatedRows = [...rows];
    updatedRows[rowIndex] = updatedRow;

    // Update the state with the new rows
    const userData = {
      assignment_description: updatedRow.assignment_description,
      assignment_deadline: updatedRow.assignment_deadline,
    };

    // if id exists in database
    if (Number.isInteger(updatedRow.id)) {
      updateAssignment(userData, updatedRow.id, session?.user.token);
      console.log(userData);
    }

    //createUser(userData);

    setRows(updatedRows);

    // Return the updated row to update the internal state of the DataGrid
    return updatedRow;
  };

  const handleCancel = () => {

    setOpen(false);
  };

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns: GridColDef[] = [
    {
      field: "assignment_description",
      headerName: "Description",
      width: 240,
      editable: false,
    },
    {
      field: "assignment_deadline",
      headerName: "Deadline",
      width: 190,
      editable: false,
      type: "string",
    },
    {
      field: "actions",
      type: "actions",
      headerName: "Actions",
      width: 100,
      cellClassName: "actions",
      getActions: ({ id }) => {
        const isInEditMode = rowModesModel[id]?.mode === GridRowModes.Edit;

        if (isInEditMode) {
          return [
            <GridActionsCellItem
              icon={<SaveIcon />}
              label="Save"
              sx={{
                color: "primary.main",
              }}
              onClick={handleSaveClick(id)}
            />,
            <GridActionsCellItem
              icon={<CancelIcon />}
              label="Cancel"
              className="textPrimary"
              onClick={handleCancelClick(id)}
              color="inherit"
            />,
          ];
        }

        return [
          <GridActionsCellItem
            icon={<EditIcon />}
            label="Delete"
            onClick={handleDeleteClick(id)}
            color="inherit"
          />,
        ];
      },
    },
  ];

  return (
    <>
      <h1>Assignment</h1>
      <div>
        <div style={{ display: "flex"}}>
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
            sx={{ width: 300, "& .MuiAutocomplete-inputRoot": {
              borderRadius: 0,
            },}}
            renderInput={(params) => <TextField {...params} label="Subject" />}
            renderOption={(props, option) => (
                
              <li {...props} key={option}>
                {option}
                
              </li>
            )}
            renderTags={(tagValue, getTagProps) =>
              tagValue.map((option, index) => (
                <Chip sx={{ width: 300}} {...getTagProps({ index })} key={option} label={option} />
              ))
            }
          />

          <Button
            sx={{
                borderRadius:0
            }}
            variant="contained"
            color="info"
            onClick={handleCurrentClick}
          >
            Current
          </Button>
          <Button
          sx={{
            borderRadius:0
        }}
            variant="contained"
            color="success"
            onClick={handleSubmitedClick}
          >
            Submited
          </Button>
        </div>
      </div>

      <React.Fragment>
        <Dialog open={open}>
          <DialogTitle> {value} Assignment </DialogTitle>
          <DialogContent>
            <DialogContentText>
              Carefully read the assignment information before submitting
            </DialogContentText>
            Submit your answer here:
            <TextField
              autoFocus
              margin="dense"
              id="text"
              label="Answer"
              type="text"
              fullWidth
              variant="standard"
              value={answer}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setAnswer(e.target.value)
              }
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleCancel}>Cancel</Button>
            <Button onClick={handleClose}>Submit</Button>
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
        sx={{borderRadius:0}}
          rows={rows}
          columns={columns}
          editMode="row"
          rowModesModel={rowModesModel}
          onRowModesModelChange={handleRowModesModelChange}
          onRowEditStop={handleRowEditStop}
          processRowUpdate={handleProcessRowUpdate}
        />
      </Box>
    </>
  );
}
