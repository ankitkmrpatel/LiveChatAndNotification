"use client";

import React from "react";
import { signIn } from "next-auth/react";
import { useSearchParams, useRouter } from "next/navigation";
import { toast } from "react-hot-toast";

import LoginPageLoadingComponent from "./components/LoginPageLoadingComponent";
import LoginPageComponent from "./components/LoginPageComponent";

export default function Login() {
  console.log("Login Page Called");
  const router = useRouter();
  const [isLoading, setIsLoading] = React.useState(false);

  const params = useSearchParams();
  const callbackUrl = params.get("callbackUrl") || "/dashboard";

  const handleLogin = (e: any) => {
    e.preventDefault();
    var username = e.target[0].value;
    var password = e.target[1].value;

    //console.log("Login Request", username, password);

    setIsLoading(true);
    signIn("credentials", {
      redirect: true,
      email: username,
      password: password,
      callbackUrl,
    })
      .then((callback: any) => {
        if (callback?.ok) {
          toast.success("Logged in.");
        }

        if (callback?.error) {
          toast.error(callback?.error);
        }
      })
      .catch((error) => {
        toast.error("Something Went Wrong!");
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  React.useEffect(() => {
    const loginError = params.get("error");
    loginError && toast.error(loginError);

    const loginSucces = params.get("success");
    loginSucces && toast.success(loginSucces);
  }, []);

  return (
    <React.Suspense fallback={<LoginPageLoadingComponent />}>
      <LoginPageComponent handleLogin={handleLogin} isLoading={isLoading} />;
    </React.Suspense>
  );
}
