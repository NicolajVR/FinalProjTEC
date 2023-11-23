"use client";
import {
  updateSchedule,
  createSchedule,
  deleteSchedule,
  getSchedule,
  getScheduleById,
} from "@/app/lib/Schedule";
import { Scheduler } from "@aldabil/react-scheduler";
import {
  EventActions,
  ProcessedEvent,
  SchedulerRef,
} from "@aldabil/react-scheduler/types";
import { Schedule } from "@mui/icons-material";
import { Console } from "console";
import { useSession } from "next-auth/react";
import { redirect } from "next/navigation";
import { useEffect, useRef, useState } from "react";

const Dashboard = () => {
  const { data: session, status } = useSession();
  let checkin = true;

  // Use state to manage when to render the component
  const [isReady, setIsReady] = useState(false);
  const [EVENTS, setEVENTS] = useState([]);

  useEffect(() => {

    const fetchData = async () => {
      if (status === "authenticated") {
        setIsReady(true);
        const schedules = await getSchedule();
        console.log(schedules);


        if (session.user.role_id == 3)
        {

          checkin = false;

        }


        let events = schedules.map((schedule: any) => ({
          event_id: schedule.schedule_id,
          title: schedule.subject_name,
          start: new Date(schedule.start_time),
          end: new Date(schedule.end_time),
          editable: checkin,
          deletable: checkin
        }));

        console.log("test: ", events);
        setEVENTS(events);

      }
    };

    if (status === "authenticated") {
      setIsReady(true);
      fetchData();
    } else if (status === "unauthenticated") {
      redirect("/auth/signin");
    }
    //console.log(rows[0]);
  }, [status]);

  // Only render the profile page if isReady is true
  if (!isReady) {
    return null;
  }

  const handleDeleteClick = async (deletedId: string): Promise<string> => {
    console.log("here: ", deletedId);
    deleteSchedule(parseInt(deletedId));

    return new Promise((res, rej) => {
      res(deletedId);
    });
  };

  const handleSaveClick = async (
    event: ProcessedEvent,
    action: EventActions
  ): Promise<ProcessedEvent> => {
    const currentDate = new Date();
    const currentDayOfWeek = currentDate.getDay();
    const daysOfWeek = [
      "Sunday",
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
    ];
    const currentDayName = daysOfWeek[currentDayOfWeek];



    const userData = {
      subject_id: 1,
      day_of_week: currentDayName,
      subject_name: event.title,
      start_time: event.start,
      end_time: event.end,
      class_id: 1,
    };

    let lastEventId: 1;

    if (action === "edit") {
      const updateData = await getScheduleById(event.event_id as number);

      const newData = {
        class_id: updateData.class_id,
        subject_id: updateData.subject_id,
        day_of_week: updateData.day_of_week,
        subject_name: event.title,
        start_time: event.start,

        end_time: event.end,
      };

      console.log("work: ", newData);

      updateSchedule(newData, event.event_id as number);
    } else if (action === "create") {

      if (session?.user.role_id == 1 || 2 )
      {
      await createSchedule(userData, session?.user.token)

      const schedules: any[] = await getSchedule();

      if (schedules.length > 0) {
        // Extract the last event_id
        lastEventId = await schedules[schedules.length - 1].schedule_id;
  
        // lastEventId as needed
        console.log("Last Event ID:", lastEventId);
      }

    }


    }



    return new Promise( (res, rej) => {
          res({
            ...event,
            event_id: event.event_id || lastEventId,
          });
    });
  };

  return (
    <Scheduler
      view="week"
      month={{
        weekDays: [0, 1, 2, 3, 4, 5],
        weekStartOn: 6,
        startHour: 8,
        endHour: 17,
        navigation: false,
        disableGoToDay: false,
      }}
      events={EVENTS}
      onConfirm={handleSaveClick}
      onDelete={handleDeleteClick}
    />
  );
};
export default Dashboard;
