export async function getUsers() {
    const response = await fetch("https://localhost:7136/api/User");
  
    if (!response.ok) {
      throw new Error("failed to fetch users");
    }
  
    return await response.json();
  }
  
  export async function createUser(user: any) {
    try {
      const response = await fetch("https://localhost:7136/api/User", {
        method: "POST",
        body: JSON.stringify(user),
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
  
  export async function deleteUser(id: number) {
    try {
      const response = await fetch("https://localhost:7136/api/User/" + id, {
        method: "DELETE",
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to delete user");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }
  
  export async function updateUser(user: any, id: number) {
    try {
      const response = await fetch("https://localhost:7136/api/User/" + id, {
        method: "PUT",
        body: JSON.stringify(user),
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
  
  export async function getUserById(id: number) {
      try {
        const response = await fetch("https://localhost:7136/api/User/" + id,);
        console.log(response);
        
        if (!response.ok) {
          throw new Error("Failed to create user");
        }
        return await response.json();
      } catch (error) {
        console.log(error);
      }
    }
  