export async function getSubmissions(token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/UserSubmission", {
        method: "GET",
        headers: {
            
            Authorization: 'Bearer ' + token,
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to get submissions");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }

  export async function createSubmission(submission: any,) {
    try {
      const response = await fetch("https://localhost:7136/api/UserSubmission", {
        method: "POST",
        body: JSON.stringify(submission),
        headers: {
            "content-type": "application/json",
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to create submissions");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }
  


  export async function deleteSubmission(id: number) {
    try {
      const response = await fetch("https://localhost:7136/api/UserSubmission/" + id, {
        method: "DELETE",
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to delete submission");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }


  