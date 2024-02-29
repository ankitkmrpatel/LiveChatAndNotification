"use client";

import axios from "@libs/axiosClient";
import type { Session } from "next-auth";

export const useRefreshToken = () => {
  const refreshToken = async (session: Session | null) => {
    if (!session) throw new Error("Session NOT Found.");

    //console.log("Refresh Token Called", session);

    try {
      const res = await axios.post("/api/users/refreshToken", {
        refreshToken: session.user.refreshToken,
        username: session.user.email,
      });

      //console.log("Refresh Token Endpoint", res);
      if (res.status === 200) {
        if (session) {
          session.user.apiToken = res.data.token.accessToken;
          session.user.apiTokenValid = res.data.token.expiry;
          session.user.refreshToken = res.data.token.refreshToken;
        }
      }
    } catch (err) {
      console.log("Err at Refreshing Token ", err);
    }
  };

  return refreshToken;
};
