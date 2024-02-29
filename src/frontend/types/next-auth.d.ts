import { DefaultSession } from "next-auth";

declare module "next-auth" {
  interface Session {
    user: {
      id: string;
      apiToken: string;
      apiTokenValid: string;
      refreshToken: string;
    } & DefaultSession["user"];
  }
}
