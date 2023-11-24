export default async function softdeleteProfile(id: number) {
    try {
    const response = await fetch("https://localhost:7136/api/User_information/" + id, {
      method: 'DELETE',
    });
    console.log(response);
    if (!response.ok) {
      throw new Error("Failed to delete profile");
    }
    return await response.json();

}catch (error){
    console.log(error)

}
  
  }
  