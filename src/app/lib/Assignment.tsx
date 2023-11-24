
/*
    GetAssignments: Denne funktion henter opgaveoplysninger ved at foretage en GET-forespørgsel til serveren.

    deleteAssignment: Sletter en specifik opgave ved at sende en DELETE-forespørgsel med opgave-ID og en autentificeringstoken.

    updateAssignment: Opdaterer en eksisterende opgave ved at sende en PUT-forespørgsel med opgaveoplysninger, opgave-ID og en autentificeringstoken.

    createAssignment: Opretter en ny opgave ved at sende en POST-forespørgsel med opgaveoplysninger og en autentificeringstoken.
*/

export async function GetAssignments() {
    try {
      const response = await fetch("https://localhost:7136/api/Assignment", {
        method: "GET",
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to get Assignment");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }




  export async function deleteAssignment(assignment_id: number, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Assignment/" + assignment_id, {
        method: "DELETE",
        headers: {
            Authorization: 'Bearer ' + token,
        },

      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to delete assignment");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }




export async function updateAssignment(assignment: any, assignment_id: number, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Assignment/" + assignment_id, {
        method: "PUT",
        body: JSON.stringify(assignment),
        headers: {
            "content-type": "application/json",
            Authorization: 'Bearer ' + token,
        },

      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to update assignment");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }

  export async function createAssignment(assignment: any, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Assignment", {
        method: "POST",
        body: JSON.stringify(assignment),
        headers: {
            "content-type": "application/json",
            Authorization: 'Bearer ' + token,
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to create assignment");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }