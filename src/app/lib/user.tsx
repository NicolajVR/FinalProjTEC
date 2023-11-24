
  export async function getUsers(token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/User", {
        method: "GET",
        headers: {
            Authorization: 'Bearer ' + token,
        },
      });
      console.log(response);
      if (!response.ok) {
        throw new Error("Failed to get user");
      }
      return await response.json();
    } catch (error) {
      console.log(error);
    }
  }
  
  export async function createUser(user: any, token: any) {
    try {
      const response = await fetch("https://localhost:7136/api/User", {
        method: "POST",
        body: JSON.stringify(user),
        headers: {
            Authorization: 'Bearer ' + token,
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
        throw new Error("Failed to update user");
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
          throw new Error("Failed to get userbyid");
        }
        return await response.json();
      } catch (error) {
        console.log(error);
      }
    }
  
    export async function login(user: any) {
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
            throw new Error("Failed to login");
          }
          return await response.json();
        } catch (error) {
          console.log(error);
        }
      }