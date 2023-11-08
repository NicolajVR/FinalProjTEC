"use client";
import { Scheduler } from "@aldabil/react-scheduler";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";
import { useEffect, useState } from "react";


const Dashboard = () => {
    const {data: session, status} = useSession();
    const checkin = !!session;
    
      // Use state to manage when to render the component
    const [isReady, setIsReady] = useState(false);
  
    useEffect(() => {
      if (status === "authenticated") {
        setIsReady(true);
      } else if (status === "unauthenticated") {
        redirect("/auth/signin");
      }
    }, [status]);
  
    // Only render the profile page if isReady is true
    if (!isReady) {
      return null;
    }

    return (
        <Scheduler
        

  view="week"
  month={{
    weekDays: [0, 1, 2, 3, 4, 5],
    weekStartOn: 6,
    startHour: 9,
    endHour: 17,
    navigation: false,
    disableGoToDay: false
  }}
  events={[
    {
      event_id: 1,
      title: "Matematik",
      start: new Date("2023/11/6 08:00"),
      end: new Date("2023/11/6 15:00"),
      editable: checkin,
      deletable: checkin
    },
    {
      event_id: 2,
      title: "Dansk",
      start: new Date("2023/11/6 8:00"),
      end: new Date("2023/11/6 15:00"),
      editable: checkin,
      deletable: checkin
    },
  ]}
/>

    )
}
export default Dashboard;

