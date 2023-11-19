import NextAuth, { NextAuthOptions } from 'next-auth';
import CredentialsProvider from 'next-auth/providers/credentials';
export const authOptions: NextAuthOptions = {
  providers: [
    CredentialsProvider({
      name: 'Credentials',
      credentials: {
        surname: { label: 'username', type: 'text' },
        password_hash: { label: 'password', type: 'password' },
      },
      async authorize(credentials, req) {
        try {
          
          const res = await fetch('https://localhost:7136/api/User/authenticate', {
            method: 'POST',
            body: JSON.stringify(credentials),
            headers: { 'Content-Type': 'application/json' },
          });

          const user = await res.json();

          //console.log('here:', user);

          if (res.ok && user) {
            return user;
          } else {
            return Promise.resolve(null);
          }
        } catch (error) {
          console.error('Error validating credentials:', error);
          return Promise.resolve(null);
        }
      },
    }),
  ],

  callbacks:{

    async jwt({token, user}){

      return { ...token, ...user}; 

    },

    async session ({session,token,user}){
      session.user = token as any;
      return session
    }

  }




};

export const handler = NextAuth(authOptions);

export { handler as GET, handler as POST };
