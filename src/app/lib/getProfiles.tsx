
export default async function getUProfiles() {
    const response = await fetch("https://localhost:7136/api/User_information")

    if (!response.ok){
        throw new Error("failed to fetch users")

    }

    return await response.json()

}
