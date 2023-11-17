export async function getProfileById(id: number) {
    try {
      const response = await fetch("https://localhost:7136/api/User_information/" + id,);
      console.log(response);
      
      if (!response.ok) {
        throw new Error("Failed to create user");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }
