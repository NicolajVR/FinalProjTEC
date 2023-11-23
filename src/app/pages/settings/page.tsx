"use client";
import * as React from "react";
import { getUsers, createUser, updateUser, deleteUser } from "@/app/lib/user";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import AddIcon from "@mui/icons-material/Add";
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
import { useSession } from "next-auth/react";
import { useEffect, useState } from "react";
import { redirect } from "next/navigation";
import {
  Autocomplete,
  Chip,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Hidden,
  Link,
  TextField,
} from "@mui/material";
import { create } from "domain";
import createProfile from "@/app/lib/createProfile";
import getUProfiles from "@/app/lib/getProfiles";
import { createEnrollment, deleteEnrollment, getClassesFromUser } from "@/app/lib/enrollment";
import { getClass } from "@/app/lib/class";

const initialRows: GridRowsProp = [
  {
    id: randomId(),
    surname: randomTraderName(),
    email: "Maria@gmail.com",
    password_hash: "asdasdasdasd",
    email_confirmed: true,
    lockout_confirmed: false,
    twofactor_enabled: false,
    try_failed_count: 5,
    lockout_end: 5,
    is_deleted: 0,
  },
];

let options: string[] = [];

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
    setRows((oldRows) => [
      ...oldRows,
      {
        id,
        surname: "",
        email: "",
        password_hash: "",
        is_deleted: false,
        role: 3,
        isNew: true,
      },
    ]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: "name" },
    }));
  };

  return (
    <GridToolbarContainer>
      <Button color="primary" startIcon={<AddIcon />} onClick={handleClick}>
        Add user
      </Button>
    </GridToolbarContainer>
  );
}

export default function FullFeaturedCrudGrid() {
  const [rows, setRows] = React.useState(initialRows);
  const [selected, setSelected] = React.useState("User not selected");
  const [selectedID, setSelectedID] = React.useState([
    { id: 1, surname: "User not selected" },
  ]);
  const [value, setValue] = React.useState<string | null>(null); // Set initial state to null
  const [open, setOpen] = React.useState(false);
  const [openRemove, setOpenRemove] = React.useState(false);
  const [inputValue, setInputValue] = React.useState("");

  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>(
    {}
  );

  const { data: session, status } = useSession();

  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    rows.forEach(async (row) => {
      if (row.isNew && row.surname && row.email) {
        let result = 1;

        switch (row.role) {
          case "Admin":
            result = 1;
            break;
          case "Teacher":
            result = 2;
            break;
          case "Student":
            result = 3;
            break;
          default:
            result = 3;
            break;
        }

        const userData = {
          surname: row.surname,
          email: row.email,
          password_hash: row.password_hash,
          is_deleted: row.is_deleted,
          role_id: result,
        };

        await createUser(userData, session?.user.token);

        const users = await getUsers(session?.user.token);
        // Extract the last event_id
        const lastEventId = await users[users.length - 1].user_id;

        const profileData = {
          user_information_id: lastEventId,
          name: row.surname,
          last_name: "",
          phone: "",
          date_of_birth: "",
          address: "",
          is_deleted: row.is_deleted,
          gender_id: 1,
          user_id: lastEventId,
        };

        console.log(profileData);
        await createProfile(profileData);

        row.isNew = false;
      }
    });
  }, [rows]);

  useEffect(() => {
    const fetchData = async () => {
      //console.log("TOKEN: ",session?.user.token);

      const classes = await getClass();
      options = classes.map((item: any) => item.className);

      setValue(options[0]);
      setInputValue(options[0]);

      const users = await getUsers(session?.user.token);
      //console.log("token: ", session?.user.token);
      users.forEach(
        (user: {
          user_id: any;
          surname: any;
          email: any;
          is_deleted: any;
          role: any;
        }) => {
          setRows(
            users.map((user: any) => ({
              id: user.user_id,
              surname: user.surname,
              email: user.email,
              password_hash: "",
              is_deleted: user.is_deleted,
              role: user.role_id,
            }))
          );
        }
      );
    };

    if (status === "authenticated") {
      setIsReady(true);
      console.log(rows[0]);
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
    const rowToDelete = rows.find((row) => row.id === id);
    // Find the index of the row to be deleted
    const rowIndex = rows.findIndex((row) => row.id === id);
    const updatedRows = [...rows];

    // Update the is_deleted property of the row to be soft-deleted
    updatedRows[rowIndex] = { ...updatedRows[rowIndex], is_deleted: true };

    setRows(updatedRows);

    var rowId: number = +id;
    console.log(rowId);
    console.log(rowToDelete?.is_deleted);

    deleteUser(rowId);
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

    let result = 1;

    switch (updatedRow.role) {
      case "Admin":
        result = 1;
        break;
      case "Teacher":
        result = 2;
        break;
      case "Student":
        result = 3;
        break;
      default:
        result = 3;
        break;
    }

    // Update the state with the new rows
    const userData = {
      user_id: updatedRow.id,
      surname: updatedRow.surname,
      email: updatedRow.email,
      password_hash: updatedRow.password_hash,
      is_deleted: updatedRow.is_deleted,
      role_id: result,
    };

    // if id exists in database
    if (Number.isInteger(updatedRow.id)) {
      updateUser(userData, updatedRow.id);
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

  const handleAddToClass = () => {
    console.log("after pressed button:", selectedID);
    setOpen(true);
    //await createEnrollment()
  };

  const handleRemove = () => {
    setOpenRemove(true);


  };

  const handleCancel = () => {
    setOpen(false);
    setOpenRemove(false);
  };

  const handleRemoveConfirm = async () => {

    const findIdByName = (
      classesArray: any[],
      classNameToFind: string
    ): number | undefined => {
      const foundClass = classesArray.find(
        (item) => item.className === classNameToFind
      );
      return foundClass?.id;
    };

    if (value != null) {
      const classes = await getClass();

      const userIds = selectedID.map((item: any) => item.id);

      //console.log("Enrollment submitted:", classData, userIds);
      let number = 0;


      const enrollmentPromises = userIds.map(async (userId: number) => {


        const enrollments = await getClassesFromUser(Number(userId), session?.user.token);

        if (enrollments)
        {
        const enrollmentIds = enrollments.map((enrollment: any) => enrollment.enrollment_Id);

        enrollmentIds.forEach(async (enrollmentId: any) => {
          // Perform actions for each enrollmentId
          console.log("Enrollment ID:", enrollmentId);
          await deleteEnrollment(enrollmentId, session?.user.token);
        });



        console.log("response send to database: ", enrollmentIds);
      }
      });
  
      await Promise.all(enrollmentPromises);

    }
    setOpenRemove(false);
  };

  const handlesubmit = async () => {
    const findIdByName = (
      classesArray: any[],
      classNameToFind: string
    ): number | undefined => {
      const foundClass = classesArray.find(
        (item) => item.className === classNameToFind
      );
      return foundClass?.id;
    };

    if (value != null) {
      const classes = await getClass();

      const classData = findIdByName(classes, value as string);
      const userIds = selectedID.map((item: any) => item.id);

      //console.log("Enrollment submitted:", classData, userIds);
      let number = 0;

      const enrollmentPromises = userIds.map(async (userId: number) => {
        
        const enrollmentData = {
          enrollmentId: 600,
          userId: Number(userId),
          ClasseId: classData,
        };
        console.log("response send to database: ",++number, enrollmentData);
        await createEnrollment(enrollmentData);
      });
  
      await Promise.all(enrollmentPromises);

      //await createEnrollment(enrollmentData);

      setOpen(false);
      //setNoChecked(false);
    }
  };

  const onRowsSelectionHandler = (ids: any[]) => {
    const selectedRowsData = ids.map((id) => rows.find((row) => row.id === id));
    setSelected(selectedRowsData[0]?.name);

    const selectedIDs = selectedRowsData.map((row) => ({
      id: row?.id.toString(),
      surname: row?.surname.toString(),
    }));

    setSelectedID(selectedIDs);
    console.log("this is the selected row: ", selectedIDs);
  };

  const columns: GridColDef[] = [
    { field: "surname", headerName: "Surname", width: 180, editable: true },
    {
      field: "email",
      headerName: "Email",
      width: 180,
      align: "left",
      headerAlign: "left",
      editable: true,
    },
    {
      field: "password_hash",
      renderCell: (params) => (
        <span>{'*'.repeat(params.value.toString().length)}</span>
      ),
      headerName: "Password",
      width: 180,
      editable: true,
    },
    {
      field: "is_deleted",
      headerName: "Is_deleted",
      width: 120,
      editable: true,
      type: "boolean",
      valueGetter: (params) => {
        if (params.value == null) {
          return false;
        }
        return params.value;
      },
    },
    {
      field: "role",
      headerName: "Role",
      width: 120,
      editable: true,
      type: "singleSelect",
      valueOptions: ["Admin", "Teacher", "Student"],
      valueGetter: (params) => {
        if (params.value == 1) {
          return "Admin";
        }
        if (params.value == 2) {
          return "Teacher";
        }
        if (params.value == 3) {
          return "Student";
        }
        return params.value;
      },
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
      <h1>Users administration</h1>

      <Button
        sx={{
          borderRadius: 0,
        }}
        variant="contained"
        color="info"
        onClick={handleAddToClass}
      >
        Add to Class
      </Button>

    
      <Button
        sx={{
          borderRadius: 0,
        }}
        variant="contained"
        color="error"
        onClick={handleRemove}
      >
        Remove from Class
      </Button>
      
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
        <React.Fragment>
          <Dialog open={open}>
            <DialogTitle>
              {" "}
              Selected class: <strong>{value}</strong>{" "}
            </DialogTitle>
            <DialogContent>
              <DialogContentText
                style={{ maxWidth: "400px", overflowWrap: "break-word" }}
              >
                {selectedID.map((item, index) => (
                  <span key={index}>{item.surname}, </span>
                ))}
              </DialogContentText>
              <br></br>
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
                renderInput={(params) => (
                  <TextField {...params} label="Select class" />
                )}
                renderOption={(props, option) => (
                  <li {...props} key={option}>
                    {option}
                  </li>
                )}
                renderTags={(tagValue, getTagProps) =>
                  tagValue.map((option, index) => (
                    <Chip
                      {...getTagProps({ index })}
                      key={option}
                      label={option}
                    />
                  ))
                }
              />
            </DialogContent>
            <DialogActions>
              <Button onClick={handleCancel}>Cancel</Button>
              <Button onClick={handlesubmit}>Add to class</Button>
            </DialogActions>
          </Dialog>
        </React.Fragment>

        <React.Fragment>
          <Dialog open={openRemove}>
            <DialogTitle>
              {" "}
              Selected class: <strong>{value}</strong>{" "}
            </DialogTitle>
            <DialogContent>
              <DialogContentText
                style={{ maxWidth: "400px", overflowWrap: "break-word" }}
              >
                {selectedID.map((item, index) => (
                  <span key={index}>{item.surname}, </span>
                ))}
              </DialogContentText>
              <br></br>
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
                renderInput={(params) => (
                  <TextField {...params} label="Select class" />
                )}
                renderOption={(props, option) => (
                  <li {...props} key={option}>
                    {option}
                  </li>
                )}
                renderTags={(tagValue, getTagProps) =>
                  tagValue.map((option, index) => (
                    <Chip
                      {...getTagProps({ index })}
                      key={option}
                      label={option}
                    />
                  ))
                }
              />
            </DialogContent>
            <DialogActions>
              <Button onClick={handleCancel}>Cancel</Button>
              <Button onClick={handleRemoveConfirm}>Remove to class</Button>
            </DialogActions>
          </Dialog>
        </React.Fragment>

        <DataGrid
          rows={rows}
          columns={columns}
          editMode="row"
          rowModesModel={rowModesModel}
          onRowModesModelChange={handleRowModesModelChange}
          onRowEditStop={handleRowEditStop}
          checkboxSelection
          onRowSelectionModelChange={(ids) => onRowsSelectionHandler(ids)}
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
