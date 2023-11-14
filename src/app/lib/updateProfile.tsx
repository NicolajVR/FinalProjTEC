export default async function updateProfile(user: any, id: number) {
    try {
    const response = await fetch("https://localhost:7136/api/User_information/" + id, {
      method: 'PUT',
      body: JSON.stringify(user),
      headers: {
        'content-type': 'application/json'
      }

    });
    console.log(response);
    if (!response.ok) {
      throw new Error("Failed to create user");
    }
    return await response.json();

}catch (error){
    console.log(error)

}
  
  }
  