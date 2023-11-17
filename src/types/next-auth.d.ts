import NextAuth from "next-auth";

declare module "next-auth"{
    interface Session{
        user:{
            user_id: number;
            surname: string;
            email: string;
            is_deleted: boolean;
            role_id: number;
            token: string;

        }

    }
} 