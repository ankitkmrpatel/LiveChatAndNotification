import axios, { AxiosInstance } from "axios";
import type { Session } from "next-auth";
import { Agent } from "https";
const BASE_API_URL = "https://localhost:7072";

export const client: AxiosInstance = axios.create({
  baseURL: BASE_API_URL,
  headers: { "Content-Type": "application/json" },
  httpsAgent: new Agent({
    rejectUnauthorized: false,
  }),
});

const getClientWAuthInstance = async (
  getSession: any
): Promise<AxiosInstance> => {
  let session = await getSession();
  //console.log("Client With Auth Calling", session);

  const refreshToken = async (getSession: any) => {
    const session = await getSession();

    let isSessionRefresh = false;
    try {
      const res = await client.post("/api/users/refreshToken", {
        refreshToken: session.user.refreshToken,
        username: session.user.email,
      });

      //console.log("Refresh Token Endpoint", res);
      if (res.status === 200) {
        if (session) {
          session.user.apiToken = res.data.token.accessToken;
          session.user.apiTokenValid = res.data.token.expiry;
          session.user.refreshToken = res.data.token.refreshToken;

          // await update({
          //   ...session,
          //   user: {
          //     ...session.user,
          //     apiToken: res.data.token.accessToken,
          //     apiTokenValid: res.data.token.expiry,
          //     refreshToken: res.data.token.refreshToken,
          //   },
          // });

          isSessionRefresh = true;
        }
      }
    } catch (err) {
      console.log("Err at Refreshing Token ", err);
    }
    const updatedSession = session;
    return { isSessionRefresh, updatedSession };
  };

  const clientWAuth: AxiosInstance = axios.create({
    baseURL: BASE_API_URL,
    headers: { "Content-Type": "application/json" },
    httpsAgent: new Agent({
      rejectUnauthorized: false,
    }),
  });

  clientWAuth.interceptors.request.use(
    (config) => {
      config.headers["Authorization"] = `Bearer ${session?.user?.apiToken}`;

      return config;
    },
    (error) => Promise.reject(error)
  );

  clientWAuth.interceptors.response.use(
    (response) => response,
    async (error) => {
      const prevRequest = error?.config;
      //console.log("Error Response", error.response);
      if (error?.response?.status === 401 && !prevRequest?.sent) {
        prevRequest.sent = true;

        //console.log("Session Obj : 1", session);
        //console.log("Error Response - Refreshing The Token");
        const { isSessionRefresh, updatedSession } = await refreshToken(
          getSession
        );

        console.log("Refresh Token Data", isSessionRefresh);
        //console.log("Session Obj : 2", updatedSession);

        if (isSessionRefresh) {
          //console.log("Prev Request 1", prevRequest);
          prevRequest.headers[
            "Authorization"
          ] = `Bearer ${updatedSession?.user.apiToken}`;

          //console.log("Prev Request 2", prevRequest);
          const resNew = clientWAuth(prevRequest);

          const resss = await resNew;
          //console.log("Prev Request 3", resss);

          return resNew;
        }
      }

      return Promise.reject(error);
    }
  );

  return clientWAuth;
};

export const clientWithAuth = getClientWAuthInstance;
