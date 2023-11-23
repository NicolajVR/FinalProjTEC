//POST absence
export async function createAbsence(absence: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Absence", {
        method: "POST",
        body: JSON.stringify(absence),
        headers: {
          "content-type": "application/json",
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to create absence");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }