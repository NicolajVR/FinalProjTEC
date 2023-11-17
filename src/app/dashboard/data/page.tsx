"use client";
import * as React from "react";
import getUsers from "@/app/lib/getProfiles";
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
import updateUser from "@/app/lib/updateProfile";
import softdeleteUser from "@/app/lib/softdeleteProfile";
import createProfile from "@/app/lib/createProfile";
import getProfiles from "@/app/lib/getProfiles";
import updateProfile from "@/app/lib/updateProfile";

const initialRows: GridRowsProp = [
  {
    id: randomId(),
    name: randomTraderName(),
    last_name: 25,
    phone: 25,
    date_of_birth: "10/11/2023",
    address: randomCreatedDate(),
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

export default function FullFeaturedCrudGrid() {
  const [rows, setRows] = React.useState(initialRows);
  const [rowModesModel, setRowModesModel] = React.useState<GridRowModesModel>(
    {}
  );

  const { data: session, status } = useSession();

  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);

  /* 

  CREATE FUNCTION

  useEffect(() => {
    rows.forEach((row) => {
      
      if ( row.isNew && row.name && row.last_name) {

        const userData = {
          name: row.name,
          last_name: row.last_name,
          phone: row.phone,
          date_of_birth: row.date_of_birth,
          address: row.address,
          is_deleted: row.is_deleted,
          gender_id: row.gender_id,
        };

        createProfile(userData);
        row.isNew = false;

        console.log(userData);
      }

    });

  }, [rows]);

  */

  useEffect(() => {
    const fetchData = async () => {
      if (status === "authenticated") {
        setIsReady(true);
        const users = await getProfiles();
        console.log(users);
        users.forEach(
          (user: {
            user_information_id: any;
            name: any;
            last_name: any;
            phone: any;
            date_of_birth: any;
            address: any;
            is_deleted: any;
            gender_id: any;
            user_id: any;
          }) => {
            setRows(users.map((user: any) => ({
                id: user.user_information_id,
                name: user.name,
                last_name: user.last_name,
                phone: user.phone,
                date_of_birth: user.date_of_birth,
                address: user.address,
                is_deleted: user.is_deleted,
                gender_id: user.gender_id,
                user_id: user.user_id,
              })));
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

    softdeleteUser(rowId);

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



    switch (updatedRow.gender_id) {
      case "none":
        result =  1;
        break;
      case 'Male':
        result = 2;
        break;
      case 'Female':
        result = 3;
        break;
      default:
        result = 3;
        break;
    }

    console.log("check result: ",result);

    // Update the state with the new rows
    const userData = {
      user_information_id: updatedRow.id,
      name: updatedRow.name,
      last_name: updatedRow.last_name,
      phone: updatedRow.phone,
      date_of_birth: updatedRow.date_of_birth,
      address: updatedRow.address,
      is_deleted: updatedRow.is_deleted,
      gender_id: result,
      user_id: updatedRow.user_id,
    };

    updateProfile(userData,updatedRow.id);
    console.log("here",userData);

    //createUser(userData);

    setRows(updatedRows);

    // Return the updated row to update the internal state of the DataGrid
    return updatedRow;
  };

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns: GridColDef[] = [
    { field: "name", headerName: "Name", width: 120, editable: true },
    {
      field: "last_name",
      headerName: "Last_name",
      width: 180,
      align: "left",
      headerAlign: "left",
      editable: true,
    },
    {
      field: "phone",
      headerName: "Phone",
      width: 180,
      editable: true,
    },
    {
      field: "date_of_birth",
      headerName: "Date_of_birth",
      width: 180,
      editable: true,
    },
    {
      field: "address",
      headerName: "Address",
      width: 180,
      editable: true,
    },

    {
      field: "is_deleted",
      headerName: "Is_deleted",
      width: 90,
      editable: true,
      type: "boolean",
      valueGetter: (params) => {
        if (params.value == null) {
          return false;
        }
        return params.value
      },
    },
    {
      field: "gender_id",
      headerName: "Gender",
      width: 80,
      editable: true,
      type: 'singleSelect',
      valueOptions: ['none', 'Male', 'Female'],
      valueGetter: (params) => {
        if (params.value == 1) {
          return 'none';
        }
        if (params.value == 2) {
          return 'Male';
        }
        if (params.value == 3) {
          return 'Female';
        }
        return params.value;
      },
    },
    {
      field: "user_id",
      headerName: "User_id",
      width: 120,
      editable: false,
      type: "number",
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
      <h1>Bruger Oplysninger </h1>

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
          slotProps={{
            toolbar: { setRows, setRowModesModel },
          }}
        />
      </Box>
    </>
  );
}
