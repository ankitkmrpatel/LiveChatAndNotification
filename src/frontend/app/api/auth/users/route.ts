import { NextRequest, NextResponse } from "next/server";
import { fetch } from "@forge/api";
import { Agent } from "https";
import { client } from "@libs/APIClients";

export async function POST(req: NextRequest) {
  var loginRequest = await req.json();
  //console.log("User Received", loginRequest);

  try {
    //console.log("Client Resposne");

    var res = await client.post("/api/users", {
      username: loginRequest.username,
      password: loginRequest.password,
    });

    //console.log("Client Resposne", res);

    if (res.status == 200) {
      const userDetails = await res.data;

      //console.log("Client Resposne", userDetails);

      return NextResponse.json(
        {
          data: userDetails,
        },
        { status: 200 }
      );
    }

    // var res = await fetch("https://localhost:7072/api/users", {
    //   agent: new Agent({
    //     rejectUnauthorized: false,
    //   }),
    //   method: "POST",
    //   headers: {
    //     "Content-Type": "application/json",
    //   },
    //   body: JSON.stringify({
    //     username: loginRequest.username,
    //     password: loginRequest.password,
    //   }),
    // });

    // if (res.status == 200) {
    //   const jsonData = await res.json();

    //   var userDetails = {
    //     ...jsonData,
    //     name: jsonData.username,
    //   };

    //   var resData = {
    //     data: userDetails,
    //   };

    //   return NextResponse.json(resData, { status: 200 });
    // }

    return new NextResponse(
      JSON.stringify({
        Message: "Invalid Credentails!",
      }),
      { status: 500 }
    );
  } catch (err) {
    //console.log("Error", err);
    return new NextResponse(
      JSON.stringify({
        Message: "Something Went Wrong!",
      }),
      { status: 500 }
    );
  }
}
