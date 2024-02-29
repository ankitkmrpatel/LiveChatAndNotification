import NextAuth, { NextAuthOptions } from "next-auth";
import GithubProvider from "next-auth/providers/github";
import GoogleProvider, { GoogleProfile } from "next-auth/providers/google";
import CredentialsProvider from "next-auth/providers/credentials";
import { JWT } from "next-auth/jwt";
import { fetch } from "@forge/api";
import { Agent } from "https";

export const authOptions: NextAuthOptions = {
  providers: [
    GoogleProvider({
      clientId: process.env.AUTH_CLIENT_ID_GOOGLE,
      clientSecret: process.env.AUTH_CLIENT_SECRET_GOOGLE,
    }),
    CredentialsProvider({
      name: "Credentails",
      //credentials: {},
      credentials: {
        email: {
          label: "Email",
          type: "text",
          placeholder: "example@example.com",
        },
        password: {
          label: "Password",
          type: "password",
          placeholder: "***********",
        },
      },
      async authorize(credentails: any) {
        console.log("Credentails Request Received", {
          username: credentails.email,
          password: credentails.password,
        });

        let apiRes;

        try {
          apiRes = await fetch("http://localhost:3000/api/auth/users", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              username: credentails.email,
              password: credentails.password,
            }),
          });

          if (apiRes.status == 200) {
            const jsonData = await apiRes.json();

            //console.log("User Data", jsonData);
            return jsonData.data;
          }
        } catch (err: any) {
          //console.log("Login Error : Credentails", err);
          throw new Error("Something Went Wrong!", err);
        }

        throw new Error("Invalid Credentails!");
      },
    }),
  ],
  callbacks: {
    async session({ session, token, user }) {
      //console.log("Session Console", session, token, user);
      //Send properties to the client, like an access_token and user id from a provider.
      session.user.id = token.jti as any;
      session.user.apiToken = (token.user as any).token.accessToken;
      session.user.apiTokenValid = (token.user as any).token.expiry;
      session.user.refreshToken = (token.user as any).token.refreshToken;

      return session;
    },
    async jwt({ token, session, account, profile, trigger, user }) {
      //console.log("Session Console", token, user, trigger);
      //console.log("Session Console Trigger", trigger);

      // Persist the OAuth access_token and or the user id to the token right after signin
      // if (account) {
      //   token.accessToken = account.access_token
      //   token.id = profile.id
      // }

      try {
        const expiryDate = (token.user as any).token.expiry;

        console.log(
          "Session Expiry ",
          new Date(new Date(expiryDate).toUTCString()),
          new Date()
        );

        if (new Date(new Date(expiryDate).toUTCString()) < new Date()) {
          console.log("Token Expired ");
          return await refreshAccessToken(token);
        }
      } catch (err) {}

      user && (token.user = user);
      return token;
    },
  },
  pages: {
    signIn: "/login",
    signOut: "/",
    error: "/login", // Error code passed in query string as ?error=
    //verifyRequest: "/auth/verify-request", // (used for check email message)
    //newUser: '/auth/new-user' // New users will be directed here on first sign in (leave the property out if not of interest)
  },
  session: {
    strategy: "jwt",
  },
};

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST };

/**
 * Takes a token, and returns a new token with updated
 * `accessToken` and `accessTokenExpires`. If an error occurs,
 * returns the old token and an error property
 */
async function refreshAccessToken(token: JWT) {
  //console.log("Refresh Called", token);

  try {
    const response = await fetch(
      "https://localhost:7072/api/users/refreshToken",
      {
        agent: new Agent({ rejectUnauthorized: false }),
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          username: (token.user as any).username,
          refreshToken: (token.user as any).token.refreshToken,
        }),
      }
    );

    //console.log("Refresh Called", response);

    if (!response.ok) {
      throw new Error("Error At Refreshing Token");
    }

    const refreshedTokens = await response.json();

    return {
      ...token,
      user: refreshedTokens,
    };
  } catch (error) {
    //console.log(error);

    return {
      ...token,
      error: "RefreshAccessTokenError",
    };
  }
}

// We recommend doing your own environment variable validation
declare global {
  namespace NodeJS {
    export interface ProcessEnv {
      NEXTAUTH_SECRET: string;

      AUTH_CLIENT_ID_GOOGLE: string;
      AUTH_CLIENT_SECRET_GOOGLE: string;
    }
  }
}
