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
import VerifiedIcon from '@mui/icons-material/Verified';
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
import { deleteSubmission, getSubmissions } from "@/app/lib/userSubmisson";

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
  const [isOnVerified, setisOnVerified] = React.useState(false);
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
      setIsReady(true);
      const submissions = await getSubmissions(session?.user.token);
      const submissionData = submissions.filter( (item: any) => item.is_deleted == false);
    
      submissionData.forEach(
        (submission: {
          userSubmission_Id: any;
          userSubmission_text: any;
          userSubmission_date: any;
          userSubmissionAssignmentResponse: {opgave_Description: any, opgave_Deadline: any };
          userSubmissionUserResponse: {surname: any, email: any}
        }) => {
          setRows(
            submissionData.map((submission: any) => ({
              id: submission.userSubmission_Id,
              submission: submission.userSubmission_text,
              userSubmission_date: submission.userSubmission_date,
              description: submission.userSubmissionAssignmentResponse.opgave_Description,
              deadline: submission.userSubmissionAssignmentResponse.opgave_Deadline,
              userSubmissionUserResponse: submission.userSubmissionUserResponse.surname,
            }))
          );
        }
      );
    };

    if (status === "authenticated") {
      setIsReady(true);
      fetchData();
    } else if (status === "unauthenticated") {
      redirect("/auth/signin");
    }

    console.log(rows[0]);
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

  const handlePendingclick = async () => {

    if(isOnVerified == true ){
    const submissions = await getSubmissions(session?.user.token);
    const submissionData = submissions.filter( (item: any) => item.is_deleted == false);

    console.log("here boi: ",submissionData);

    if (submissionData.length > 0)
    {
      submissionData.forEach(
        (submission: {
          userSubmission_Id: any;
          userSubmission_text: any;
          userSubmission_date: any;
          userSubmissionAssignmentResponse: {opgave_Description: any, opgave_Deadline: any };
          userSubmissionUserResponse: {surname: any, email: any}
        }) => {
          setRows(
            submissionData.map((submission: any) => ({
              id: submission.userSubmission_Id,
              submission: submission.userSubmission_text,
              userSubmission_date: submission.userSubmission_date,
              description: submission.userSubmissionAssignmentResponse.opgave_Description,
              deadline: submission.userSubmissionAssignmentResponse.opgave_Deadline,
              userSubmissionUserResponse: submission.userSubmissionUserResponse.surname,
            }))
          );
        }
      );

    }else{setRows([])}

      setisOnVerified(false);
    }

  };

  const handleVerifiedClick = async () => {

    if(isOnVerified == false ){
        const submissions = await getSubmissions(session?.user.token);
        const submissionData = submissions.filter( (item: any) => item.is_deleted == true);


        console.log("here boi: ",submissionData);

        if (submissionData.length > 0)
        {
          submissionData.forEach(
            (submission: {
              userSubmission_Id: any;
              userSubmission_text: any;
              userSubmission_date: any;
              userSubmissionAssignmentResponse: {opgave_Description: any, opgave_Deadline: any };
              userSubmissionUserResponse: {surname: any, email: any}
            }) => {
              setRows(
                submissionData.map((submission: any) => ({
                  id: submission.userSubmission_Id,
                  submission: submission.userSubmission_text,
                  userSubmission_date: submission.userSubmission_date,
                  description: submission.userSubmissionAssignmentResponse.opgave_Description,
                  deadline: submission.userSubmissionAssignmentResponse.opgave_Deadline,
                  userSubmissionUserResponse: submission.userSubmissionUserResponse.surname,
                }))
              );
            }
          );
        }else{
            setRows([]);
        }
    
          setisOnVerified(true);
        }
  };





  const handleSaveClick = (id: GridRowId) => () => {
    setRowModesModel({ ...rowModesModel, [id]: { mode: GridRowModes.View } });
  };

  const handleDeleteClick = (id: GridRowId) => () => {
    setRows(rows.filter((row) => row.id !== id));

    var rowId: number = +id;

    deleteSubmission(rowId);
    
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

  const handleRowModesModelChange = (newRowModesModel: GridRowModesModel) => {
    setRowModesModel(newRowModesModel);
  };

  const columns: GridColDef[] = [
    { field: "submission", headerName: "Submission", width: 240, editable: false },
    {
      field: "userSubmission_date",
      headerName: "Date",
      width: 170,
      align: "left",
      headerAlign: "left",
      editable: false,
    },
    {
        field: "description",
        headerName: "Description",
        width: 190,
        align: "left",
        headerAlign: "left",
        editable: false,
      },
      {
        field: "deadline",
        headerName: "Deadline",
        width: 170,
        align: "left",
        headerAlign: "left",
        editable: false,
      },
      {
        field: "userSubmissionUserResponse",
        headerName: "Student",
        width: 170,
        align: "left",
        headerAlign: "left",
        editable: false,
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
            icon={<VerifiedIcon />}
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
      <h1>Submitted assignments </h1>
      <Button variant="contained" color="error" onClick={handlePendingclick}>
  Pending
</Button>
      <Button variant="contained" color="success" onClick={handleVerifiedClick}>
  Verified
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
        <DataGrid
          rows={rows}
          columns={columns}
          editMode="row"
          rowModesModel={rowModesModel}
          onRowModesModelChange={handleRowModesModelChange}
          slotProps={{
            toolbar: { setRows, setRowModesModel },
          }}
        />
      </Box>
    </>
  );
}
