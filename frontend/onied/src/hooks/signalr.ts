import { useEffect, useState } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  HttpTransportType,
} from "@microsoft/signalr";
import LoginService from "../services/loginService";
import { useProfile } from "./profile/useProfile";

export default function useSignalR(url: string) {
  const [profile, _] = useProfile();
  let [connection, setConnection] = useState<HubConnection | undefined>(
    undefined
  );

  useEffect(() => {
    let canceled = false;
    const connection = new HubConnectionBuilder()
      .withUrl(url, {
        withCredentials: false,
        transport: HttpTransportType.LongPolling,
        headers: {
          Authorization: `Bearer ${LoginService.getAccessToken()}`,
        },
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connection
      .start()
      .then(() => {
        if (!canceled) {
          setConnection(connection);
        }
      })
      .catch((error) => {
        console.log("signal error", error);
      });

    connection.onclose((_) => {
      if (canceled) {
        return;
      }
      console.log("signal closed");
      setConnection(undefined);
    });

    connection.onreconnecting((_) => {
      if (canceled) {
        return;
      }
      console.log("signal reconnecting");
      setConnection(undefined);
    });

    connection.onreconnected((_) => {
      if (canceled) {
        return;
      }
      console.log("signal reconnected");
      setConnection(connection);
    });

    // Clean up the connection when the component unmounts
    return () => {
      canceled = true;
      connection.stop();
    };
  }, [profile]);

  return { connection };
}
