import { NextRequest, NextResponse } from "next/server";
import { fetch } from "@forge/api";
import { Agent } from "https";

import getCurrentUser from "@actions/getCurrentUser";

export async function GET(req: NextRequest) {
  try {
    const session = await getCurrentUser();
    //console.log("Event Session ", session);

    if (!session) {
      return NextResponse.json(
        { message: "Unauthorize, You mush be logged in." },
        { status: 401 }
      );
    }

    //console.log("Event Request Received");

    var apiRes = await fetch("https://localhost:7072/api/users/events", {
      agent: new Agent({
        rejectUnauthorized: false,
      }),
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user?.apiToken}`,
      },
    });

    //console.log("Shipment Event Data", apiRes);

    if (apiRes.status == 200) {
      const jsonData = await apiRes.json();
      //console.log("Events Data", jsonData);

      var eventData = {
        message: `${jsonData.length} Event(s) Found.`,
        data: jsonData,
      };

      return NextResponse.json(eventData, { status: apiRes.status });
    }
  } catch (err) {
    console.log("Event Err", err);
  }

  return NextResponse.json(
    { message: "Failed to get Event Messages Data." },
    { status: 500 }
  );
}
