
export default async function getUsers() {
    const response = await fetch("https://localhost:7136/api/Bruger")

    if (!response.ok){
        throw new Error("failed to fetch users")

    }

    return await response.json()

}
