
export async function GetAssignments() {
    try {
      const response = await fetch("https://localhost:7136/api/Assignment", {
        method: "GET",
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to create user");
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
        throw new Error("Failed to create user");
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
        throw new Error("Failed to create user");
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
        throw new Error("Failed to create user");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }