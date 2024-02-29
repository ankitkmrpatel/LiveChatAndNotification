"use server";

import getCurrentUser from "@actions/getCurrentUser";
import { fetch } from "@forge/api";
import { headers } from "next/headers";

export async function getMessageFromAPI(isMessageLoaded) {
  let notifMessages = [];
  let isMsgLoaded = isMessageLoaded;

  const session = await getCurrentUser();
  if (session && !isMessageLoaded) {
    //console.log("get Messages From API Called", headers());
    //Call the api and get all messages
    const res = await fetch("http://localhost:3000/api/events", {
      method: "GET",
      headers: headers(),
      //next: { revalidate: 10 },
    });

    console.log("Notifications Request", res);

    if (!res.ok) {
      throw new Error("Failed to fetch data using forge/api");
    }

    const jsonData = await res.json();
    notifMessages = jsonData.data;
    isMsgLoaded = true;
  }

  return { isMsgLoaded, notifMessages };
}
