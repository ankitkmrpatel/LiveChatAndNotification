import { getServerSession } from "next-auth/next";

import { authOptions } from "@api/auth/[...nextauth]/route";

export async function getSession() {
  return await getServerSession(authOptions);
}

export async function isAuthenticated() {
  const session = await getCurrentUser();
  return !session;
}

export default async function getCurrentUser() {
  try {
    const session = await getSession();

    if (!session?.user?.email) {
      return null;
    }

    return session;
  } catch (error: any) {
    return null;
  }
}
