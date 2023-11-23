"use client";
import * as React from "react";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/DeleteOutlined";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Close";
import TextField from "@mui/material/TextField";
import Autocomplete from "@mui/material/Autocomplete";

import scss from "./opgave.module.scss";
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
import { Chip } from "@mui/material";
import { GetAssignments, createAssignment, deleteAssignment, updateAssignment } from "@/app/lib/Assignment";

const roles = ["Market", "Finance", "Development"];
const randomRole = () => {
  return randomArrayItem(roles);
};

let options: string[] = [];
let options2: string[] = [];

const initialRows: GridRowsProp = [
  {
    id: randomId(),
    name: randomTraderName(),
    age: 25,
    joinDate: randomCreatedDate(),
    role: randomRole(),
  },
];

interface EditToolbarProps {
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel
  ) => void;
}

function EditToolbar(props: EditToolbarProps) {
  const { setRows, setRowModesModel } = props;

  const handleClick = () => {
    const id = randomId();
    setRows((oldRows) => [...oldRows, { id, name: "", age: "", isNew: true }]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: "name" },
    }));
  };

  return (
    <GridToolbarContainer>
      <Button color="primary" startIcon={<AddIcon />} onClick={handleClick}>
        Add record
      </Button>
    </GridToolbarContainer>
  );
}

export default function FullFeaturedCrudGrid() {
  const [value, setValue] = React.useState<string | null>(null); // Set initial state to null
  const [value2, setValue2] = React.useState<string | null>(null); // Set initial state to null

  const [inputValue, setInputValue] = React.useState("");
  const [inputValue2, setInputValue2] = React.useState("");

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
    
          const subjectData = subjects.find( (item: any) => item.subjectname === value2);
          
          const userData = {
              classeId: classData,
              subjectId: subjectData.id,
              assignment_Deadline: row.assignment_deadline,
              assignment_Description: row.assignment_description
            
          };

          await createAssignment(userData,session?.user.token);

          row.isNew = false;
          //console.log("token: ", session?.user.token);
    
          console.log("here: ",classData);
          console.log("here subject: ",subjectData.id);
    
          //options2 = subjects.map((subject: any) => subject.subjectname);
        }
      });

      
  }, [rows]);

  React.useEffect(() => {

    
    const findIdByName = (
      classesArray: any[],
      classNameToFind: string
    ): number | undefined => {
      const foundClass = classesArray.find(
        (item) => item.enrollmentClassResponse.className === classNameToFind
      );
      return foundClass?.enrollmentClassResponse.id;
    };


    console.log("Autocomplete value changed:", value, value2);

    const fetchData = async () => {
      
      if (value != null && value2 != null) {
        const classes = await getClassesFromUser(
          session?.user.user_id as number,
          session?.user.token
        );
        const subjects = await getsubjects();
  
        const classData =  findIdByName(classes, value as string);
  
        const subjectData = await subjects.find( (item: any) => item.subjectname === value2);
  
  
        console.log("here: ", classData,subjectData.id);

        const assignments = await GetAssignments();

        const assignment_c_s = await assignments.filter((item: any) => item.classe.class_id == classData && item.subjects.subject_id == subjectData.id && item.is_deleted == false);

        console.log("FINAL: ",assignment_c_s)

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
    fetchData();
  }, [value, value2]);

  useEffect(() => {
    const fetchData = async () => {
      const classes = await getClassesFromUser(
        session?.user.user_id as number,
        session?.user.token
      );
      const subjects = await getsubjects();
      //console.log("token: ", session?.user.token);

      options = classes.map(
        (item: { enrollmentClassResponse: { className: any } }) =>
          item.enrollmentClassResponse.className
      );

      options2 = subjects.map((subject: any) => subject.subjectname);

      setValue(options[0]);
      setInputValue(options[0]);

      setValue2(options2[0]);
      setInputValue2(options2[0]);

      console.log(classes);
      console.log("look:", options);
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

  const handleEditClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.Edit } });
  };

  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } });
  };

  const handleDeleteClick = (id: GridRowId) => () => {
    setRows(rows.filter((row) => row.id !== id));

    var rowId: number = +id;

    deleteAssignment(rowId,session?.user.token);

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
    if (Number.isInteger(updatedRow.id))
    {
      updateAssignment(userData, updatedRow.id,session?.user.token);
      console.log(userData);
    }
    

    //createUser(userData);

    setRows(updatedRows);

    // Return the updated row to update the internal state of the DataGrid
    return updatedRow;
  };

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns: GridColDef[] = [
    {
      field: "assignment_description",
      headerName: "Description",
      width: 240,
      editable: true,
    },
    {
      field: "assignment_deadline",
      headerName: "Deadline",
      width: 190,
      editable: true,
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
            label="Edit"
            className="textPrimary"
            onClick={handleEditClick(id)}
            color="inherit"
          />,
          <GridActionsCellItem
            icon={<DeleteIcon />}
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
      <h1>Opgave</h1>

      <div>
        <div className={scss.inputsheader}>
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
            sx={{ width: 300 }}
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
          />

          <Autocomplete
            value={value2}
            onChange={(event: any, newValue2: string | null) => {
              setValue2(newValue2);
            }}
            inputValue={inputValue2}
            onInputChange={(event, newInputValue2) => {
              setInputValue2(newInputValue2);
            }}
            id="controllable-states-Subject"
            options={options2}
            sx={{ width: 300 }}
            renderInput={(params) => <TextField {...params} label="Subject" />}
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
          />
        </div>
      </div>

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
          editMode="row"
          rowModesModel={rowModesModel}
          onRowModesModelChange={handleRowModesModelChange}
          onRowEditStop={handleRowEditStop}
          processRowUpdate={handleProcessRowUpdate}
          slots={{
            toolbar: EditToolbar,
          }}
          slotProps={{
            toolbar: { setRows, setRowModesModel },
          }}
        />
      </Box>
    </>
  );
}
