
export async function getClassesFromUser(id: number, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Enrollment/ByUser/" + id, {
        method: "GET",
        headers: {
            Authorization: 'Bearer ' + token,
            
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to get classes from user");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }



  export async function getUsersFromClass(id: number, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Enrollment/ByClass/" + id, {
        method: "GET",
        headers: {
            Authorization: 'Bearer ' + token,
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to get users from class");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }

  export async function createEnrollment(enrollment: any,) {
    try {
      const response = await fetch("https://localhost:7136/api/Enrollment", {
        method: "POST",
        body: JSON.stringify(enrollment),
        headers: {
            "content-type": "application/json",
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

  export async function deleteEnrollment(id: number, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Enrollment/" + id, {
        method: "DELETE",
        headers: {
            Authorization: 'Bearer ' + token,
        },

      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to delete enrollment");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }

  




