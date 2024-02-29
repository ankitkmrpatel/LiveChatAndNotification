import React from "react";

import AuthProvider from "@providers/AuthProvider";

import getCurrentUser from "@actions/getCurrentUser";

export default async function ScreenLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const session = await getCurrentUser();
  console.log("Layout Protected", session);

  return <AuthProvider session={session}>{children}</AuthProvider>;
}
