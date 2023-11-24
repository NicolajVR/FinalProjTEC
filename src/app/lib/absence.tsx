// Funktion til at oprette en fraværsregistrering ved at sende en POST-forespørgsel til en API-endepunkt.
export async function createAbsence(absence: any) {
    try {
      const response = await fetch("https://localhost:7136/api/Absence", {
        method: "POST", // Bruger HTTP-metoden "POST" til at oprette fravær.
        body: JSON.stringify(absence), // Konverterer fraværsdata til JSON-format og sender det som forespørgselens krop.
        headers: {
          "content-type": "application/json", // Angiver forespørgselens "content-type" som JSON.
        },
      });
      console.log(response); // Logger svaret fra serveren.
      // Hvis svaret ikke er 'ok' (dvs. har en status uden for området 200-299), kastes en fejl.
      if (!response.ok) {
        throw new Error("Failed to create absence");
      }

      // Returnerer JSON-indholdet fra svaret
      return await response.json();
    } catch (error) {
      console.log(error); // Hvis der opstår en fejl under udførelsen, logges fejlen.
    }
  }