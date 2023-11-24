export async function getsubjects() {
    try {
      const response = await fetch("https://localhost:7136/api/Subjects", {
        method: "GET",
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to get subjects");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }
  