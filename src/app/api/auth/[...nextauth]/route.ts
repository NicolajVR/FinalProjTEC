// Importér nødvendige moduler og værktøjer fra NextAuth og CredentialsProvider
import NextAuth, { NextAuthOptions } from 'next-auth';
import CredentialsProvider from 'next-auth/providers/credentials';

export const authOptions: NextAuthOptions = {

  // Angiv en 'credentials'-provider til at håndtere brugerlogin ved brug af brugernavn og adgangskode
  // Custom provider
  providers: [
    CredentialsProvider({
      name: 'Credentials',
      credentials: {
        surname: { label: 'username', type: 'text' },
        password_hash: { label: 'password', type: 'password' },
      },

      // Funktionen 'authorize' udføres, når en bruger forsøger at logge ind
      async authorize(credentials, req) {
        try {
          // Send en POST-anmodning til den angivne URL med de indtastede legitimationsoplysninger
          const res = await fetch('https://localhost:7136/api/User/authenticate', {
            method: 'POST', //mere sikkert
            body: JSON.stringify(credentials),
            headers: { 'Content-Type': 'application/json' },
          });

          // Vent på og få resultatet i JSON-format
          const user = await res.json();

          // Hvis svaret er 'OK' (200) og der findes en bruger, returnér brugerdataene
          if (res.ok && user) {
            return user;
          } else {
            // Hvis svaret ikke er 'OK' eller der ikke findes en bruger, returnér null
            return Promise.resolve(null);
          }
        } catch (error) {
          // Håndter fejl og log dem i konsollen, returnér null
          console.error('Error validating credentials:', error);
          return Promise.resolve(null);
        }
      },
    }),
  ],

  // Konfigurér callbacks, der ændrer token- og session-opførsel
  callbacks: {
    // JWT-callback - sammensæt token og brugerdata til JWT
    async jwt({ token, user }) {
      return { ...token, ...user };
    },

    // Session-callback - sæt brugerdata på sessionen
    async session({ session, token, user }) {
      session.user = token as any;
      return session;
    },
  },
};

// Eksporter en handler ved hjælp af NextAuth med de definerede options
export const handler = NextAuth(authOptions);

// Eksporter GET- og POST-handlere som handler
export { handler as GET, handler as POST };
