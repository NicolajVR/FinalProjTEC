"use client";
import * as React from "react";
import {getUsers,createUser,updateUser,deleteUser} from "@/app/lib/user";
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
import { Link } from "@mui/material";
import { create } from "domain";
import createProfile from "@/app/lib/createProfile";

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
  const [rows, setRows] = React.useState(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>(
    {}
  );

  const { data: session, status } = useSession();

  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    rows.forEach(async (row) => {
      if (row.isNew && row.surname && row.email) {
        const userData = {
          surname: row.surname,
          email: row.email,
          password_hash: row.password_hash,
          is_deleted: row.is_deleted,
        };


        await createUser(userData);

        const users = await getUsers();
        // Extract the last event_id
        const lastEventId = await users[users.length - 1].user_id;

      const profileData = {
        user_information_id: lastEventId,
        name: row.surname,
        last_name: null,
        phone: null,
        date_of_birth: null,
        address: null,
        is_deleted: row.is_deleted,
        gender_id: 1,
        user_id: lastEventId
      };
      
      console.log(profileData);
        await createProfile(profileData);

        row.isNew = false;
      }
    });
  }, [rows]);

  useEffect(() => {
    const fetchData = async () => {
      if (status === "authenticated") {
        setIsReady(true);
        const users = await getUsers();
        console.log(users);
        users.forEach(
          (user: {
            user_id: any;
            surname: any;
            email: any;
            password_hash: any;
            is_deleted: any;
          }) => {
            setRows(
              users.map((user: any) => ({
                id: user.user_id,
                surname: user.surname,
                email: user.email,
                password_hash: user.password_hash,
                is_deleted: user.is_deleted
              }))
            );
          }
        );
      } else if (status === "unauthenticated") {
        redirect("/auth/signin");
      }
    };

    console.log(rows[0]);
    fetchData();
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

    // Update the state with the new rows
    const userData = {
      user_id: updatedRow.id,
      surname: updatedRow.surname,
      email: updatedRow.email,
      password_hash: updatedRow.password_hash,
      is_deleted: updatedRow.is_deleted,
    };

    // if id exists in database 
    if (Number.isInteger(updatedRow.id))
    {
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
      headerName: "Hash",
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
