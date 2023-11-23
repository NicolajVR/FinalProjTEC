export async function getClass() {
    const response = await fetch("https://localhost:7136/api/Classe");
  
    if (!response.ok) {
      throw new Error("failed to fetch users");
    }

    return await response.json();
  }
  
  //POST absence
export async function createClass(absence: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Classe", {
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