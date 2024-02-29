import { getSession } from "next-auth/react";
import { NextRequest } from "next/server";
//import { isAuthenticated } from "@actions/getCurrentUser";

export async function middleware(request: NextRequest) {
  console.log("Middlware Called.");
  const session = getSession();

  // Call our authentication function to check the request
  // if (!(await isAuthenticated())) {
  //   console.log("Middlware Called - NOT Authencated.");

  //   // Respond with JSON indicating an error message
  //   // return new Response("Auth Failed", {
  //   //   status: 200,
  //   // });

  //   // return Response.json(
  //   //   { success: false, message: "authentication failed" },
  //   //   { status: 401 }
  //   // );
  // }
}
