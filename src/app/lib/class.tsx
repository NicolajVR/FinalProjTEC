

//GET Class
export async function getClass() {
    const response = await fetch("https://localhost:7136/api/Classe");
  
    if (!response.ok) {
      throw new Error("failed to fetch classes");
    }

    return await response.json();
  }
  
  //POST Class
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
        throw new Error("Failed to create classes");
      }
      return await response.json();
      
    } catch (error) {
      console.log(error);
    }
  }