"use client";

import React from "react";
import * as signalR from "@microsoft/signalr";
import toast from "react-hot-toast";
import { useSession } from "next-auth/react";
import { useRouter, redirect, RedirectType } from "next/navigation";

import { getMessageFromAPI } from "@actions/getNotifications";

import { EventMessageContext } from "@contexts/EventMessageContext";

import NavbarDashboard from "@components/Navbars/NavbarDashboard";
import SidebarDashboard from "@components/Sidebars/SidebarDashboard";

import SendPushNotification from "@libs/WebBrowerNotication";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const [isPending, startTransition] = React.useTransition();

  const { data: session, status } = useSession();
  const router = useRouter();

  const {
    setMessages,
    addNewMessages,
    socketConnection,
    setSocketConnection,
    setIsMessageLoaded,
    isMessageLoaded,
  } = React.useContext(EventMessageContext);

  const data = null;
  React.useEffect(() => {
    console.log("Hello Console - UseEffect - WOD");
    const getNotificationService = () => {
      try {
        const hubConnection = new signalR.HubConnectionBuilder()
          .withUrl("https://localhost:7072/notificationHub", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
            accessTokenFactory: () => session?.user?.apiToken ?? "",
          })
          //.configureLogging(signalR.LogLevel.Information)
          .withAutomaticReconnect()
          .build();

        hubConnection.on("SendNotification", (evtMessage: any) => {
          toast.success("New Message Received");
          //SendPushNotification("New Message Received");

          addNewMessages([evtMessage]);
        });

        // hubConnection.onreconnected(() => {
        //   toast.success("Notification Re-Re-Started.");
        // });

        // hubConnection.onclose(() => {
        //   toast.error("Notification Stopped.");
        // });

        hubConnection
          .start()
          .then(() => {
            toast.success("Notification Started.");
            SendPushNotification("Notification Started");
          })
          .catch((error: any) => {
            toast.error("Notification Failed : " + error);
          });

        return hubConnection;
      } catch (err) {
        toast.error("Failed to Get Notification Service :" + err);
      }

      return null;
    };

    startTransition(() => {
      if (!isMessageLoaded) {
        (async () => {
          try {
            const { isMsgLoaded, notifMessages } = await getMessageFromAPI(
              isMessageLoaded
            );

            console.log("Messages From API", isMsgLoaded, notifMessages);

            setIsMessageLoaded(isMsgLoaded);
            setMessages(notifMessages as any[]);
          } catch (err) {
            console.log("Start Transition Error", err);
          }
        })();
      }
    });

    const hubConnection = getNotificationService();
    setSocketConnection(hubConnection);

    return () => {
      // this now gets called when the component unmounts
      console.log("Dashboared Unmouned");
      socketConnection && socketConnection.stop();
    };
  }, []);

  if (status === "unauthenticated") {
    return redirect("/login", RedirectType.push);
  }

  return (
    <>
      <div className="antialiased bg-gray-50 dark:bg-gray-900">
        <NavbarDashboard />
        {/* Sidebar */}
        <SidebarDashboard />

        <main className="p-4 md:ml-64 h-auto pt-20">{children}</main>
      </div>
    </>
  );
}
