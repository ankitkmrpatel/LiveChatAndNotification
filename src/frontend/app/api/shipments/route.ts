import { NextRequest, NextResponse } from "next/server";

import getCurrentUser from "@actions/getCurrentUser";
import { fetch } from "@forge/api";
import { Agent } from "https";
import { clientWithAuth } from "@libs/APIClients";

export async function POST(req: NextRequest) {
  //console.log("Shiment Session");

  const session = await getCurrentUser();

  try {
    //console.log("Shiment Session ", session);

    if (!session) {
      return NextResponse.json(
        { message: "Unauthorize, You mush be logged in." },
        { status: 401 }
      );
    }

    //console.log("Shipment Request Received");

    var apiRes = await fetch("https://localhost:7072/api/shipments", {
      agent: new Agent({
        rejectUnauthorized: false,
      }),
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${session?.user?.apiToken}`,
      },
    });

    //console.log("Shipment Data", apiRes);
    //const axiosAuth = await clientWithAuth(getCurrentUser);
    //const apiRes = await axiosAuth.get("/api/shipments");
    //console.log("Shipment Data", apiRes);

    if (apiRes.status == 200) {
      const jsonData = await apiRes.json();

      var shipmentData = {
        message: `${jsonData.length} Shipment(s) Found.`,
        data: jsonData,
      };

      return NextResponse.json(shipmentData, { status: apiRes.status });
    }
  } catch (err) {
    console.log("Shipment Err", err);
  }

  return NextResponse.json(
    { message: "Failed to get shipment data." },
    { status: 500 }
  );
}
