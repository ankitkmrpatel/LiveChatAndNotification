import { NextRequest, NextResponse } from "next/server";
import { fetch } from "@forge/api";
import { Agent } from "https";

import getCurrentUser from "@actions/getCurrentUser";

export async function POST(
  req: NextRequest,
  { params }: { params: { id: string } }
) {
  //console.log("Shiment Session");
  const { id } = params;
  try {
    const session = await getCurrentUser();
    //console.log("Shiment Session ", session?.user?.apiToken);

    if (!session) {
      return NextResponse.json(
        { message: "Unauthorize, You mush be logged in." },
        { status: 401 }
      );
    }

    //console.log("Shipment Request Received");

    var apiRes = await fetch(`https://localhost:7072/api/shipments/${id}`, {
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

    if (apiRes.status == 200) {
      const jsonData = await apiRes.json();
      //console.log("Shipment Data", jsonData);

      var shipmentData = {
        message: `${jsonData.length} Shipment(s) Found.`,
        data: jsonData,
      };

      return NextResponse.json(shipmentData, { status: apiRes.status });
    }
  } catch (err) {
    console.log("Shipment [Id] Err", err);
  }

  return NextResponse.json(
    { message: "Failed to get shipment data." },
    { status: 500 }
  );
}
