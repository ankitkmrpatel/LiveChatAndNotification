import { axiosAuth } from "@libs/axiosClient";
import type { Session } from "next-auth";
import { useEffect } from "react";
import { useRefreshToken } from "./useRefreshToken";
import { AxiosInstance } from "axios";

const useAxiosAuth = (session: Session | null): AxiosInstance => {
  if (!session) throw new Error("User Session not found.");
  console.log("Axios Auth Called", session);

  const refreshToken = useRefreshToken();

  useEffect(() => {
    const requestIntercept = axiosAuth.interceptors.request.use(
      (config) => {
        if (!config.headers["Authorization"]) {
          config.headers["Authorization"] = `Bearer ${session?.user?.apiToken}`;
        }

        return config;
      },
      (error) => Promise.reject(error)
    );

    const responseIntercept = axiosAuth.interceptors.response.use(
      (response) => response,
      async (error) => {
        const prevRequest = error?.config;
        if (error?.response?.status === 401 && !prevRequest?.sent) {
          prevRequest.sent = true;

          await refreshToken(session);

          prevRequest.headers[
            "Authorization"
          ] = `Bearer ${session?.user.apiToken}`;
          return axiosAuth(prevRequest);
        }

        return Promise.reject(error);
      }
    );

    return () => {
      axiosAuth.interceptors.request.eject(requestIntercept);
      axiosAuth.interceptors.response.eject(responseIntercept);
    };
  }, [session, refreshToken]);

  return axiosAuth;
};

export default useAxiosAuth;
