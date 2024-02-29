import { NextResponse } from "next/server";
import DbConnection, { Disconnect } from "@utils/db";
import Users from "@models/User";

export async function POST(req) {
  var newUser = await req.json();
  console.log("User Received", newUser);

  try {
    await DbConnection();

    var newUsrObj = new Users({
      name: newUser.name,
      email: newUser.email,
      password: newUser.password,
    });

    await newUsrObj.save();

    return new NextResponse("New User Register", { status: 201 });
  } catch (err) {
    console.log("Error", err);
    return new NextResponse("Something Went Wrong!", { status: 500 });
  } finally {
    await Disconnect();
  }
}
