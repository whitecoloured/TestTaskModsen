import { Box, Button, Center, Flex, Text, Image, Card, CardHeader, CardBody, CardFooter } from "@chakra-ui/react";
import { deleteEvent, getEvents, getUserRole } from "../../processing";
import { useEffect, useState } from "react";
import {useNavigate} from "react-router-dom"
import AlertConfirmation from "../../Components/Alert/AlertConfirmation";
import AlertMessage from "../../Components/Alert/AlertMessage";
import EventCard from "../../Components/EventCard/EventCard";
import SquarePage from "../../Components/SquarePage/SquarePage"
import { useRef } from "react";


function AdminPage()
{


    async function DeleteFunction()
    {
        var response=await deleteEvent(currentEventID.current);
        if(response.status===200)
        {
            setShowMessageAfterDelete(true);
        }
        else setShowErrorAlert(true);
    }

    function setCurrentPage(value)
    {
        setFilters({...filters, Page:value})
    }

    function onDeleteButtonClick(EventID)
    {
        setShowAlertonDeleteEvent(true);
        currentEventID.current=EventID;
    }

    const [events, setEvents]=useState([]);
    const [filters, setFilters]=useState({
        Page:1
    });
    const [isLoading, setIsLoading]=useState(true)
    const [eventsAmount, setEventsAmount]=useState(0);
    const [role, setRole]=useState(null);

    const [showAlertonDeleteEvent, setShowAlertonDeleteEvent]=useState(false);
    const [showMessageAfterDelete, setShowMessageAfterDelete]=useState(false);
    const [showErrorAlert, setShowErrorAlert]=useState(false);

    const navigate=useNavigate();

    const currentEventID=useRef(null);

    useEffect(()=>
    {
        const getData=async()=>
        {
            var data=await getEvents(filters);
            var roleData=await getUserRole();
            setEvents(data.eventsList);
            setEventsAmount(data.eventsAmount);
            setRole(roleData);
            if (events)
            {
                setIsLoading(false)
            }
        }
        getData();
    },[filters])

    let pagesAmount=Math.ceil(eventsAmount/3);

    const eventsData=events.map(e=>
        <EventCard key={e.id} 
        EventName={e.name} 
        EventCategory={e.category}
        EventPlace={e.eventPlace}
        EventDate={e.eventDate}
        imageUrl={e.imageURL}
        footerButtons={
            <>
            <Button colorScheme="orange" marginRight={"1%"} onClick={()=> navigate("updateevent", {state:{id:e.id}})}>Edit</Button>
            <Button colorScheme="red" onClick={()=> onDeleteButtonClick(e.id)}>Delete</Button>
            </>
        }/>
    );
    const squarePages=Array(pagesAmount).fill(null).map((_, index)=>
        <SquarePage key={index+1} value={index+1} setCurrentPage={setCurrentPage} />
    )
    return(
        <>
        {!isLoading&&
            <>
            {role==="Admin"?
            <Box minHeight={"xl"}>
                <Flex w={"95%"} marginBottom={"1%"}>
                    <Button w={"13%"} size={"lg"} colorScheme="teal" marginLeft={"auto"} onClick={()=> navigate("createevent")}>
                        <Text fontWeight={"bold"}>Add Event</Text>
                    </Button>
                </Flex>
                <Center marginBottom={"1%"}>
                    <Box w={"90%"}>
                        {eventsData}
                    </Box>
                </Center>
                <Center marginBottom={"1.2%"}>
                    {squarePages}
                </Center>

                <AlertConfirmation
                isOpen={showAlertonDeleteEvent}
                headMessage={"Delete Event"}
                bodyMessage={"Are you sure you want to delete the event?"}
                onConfirm={()=>DeleteFunction()}
                onClose={setShowAlertonDeleteEvent}/>

                <AlertMessage
                isOpen={showMessageAfterDelete}
                headMessage={"Success"}
                bodyMessage={"The event has been deleted!"}
                onClose={setShowMessageAfterDelete}
                onButtonClick={()=> setCurrentPage(1)}/>

                <AlertMessage
                isOpen={showErrorAlert}
                headMessage={"Delete error"}
                bodyMessage={"The problem occured in deleting event"}
                onClose={setShowErrorAlert}/>
            </Box>
            :
            <Text fontWeight={"bold"} fontSize={"30px"}>403 Forbidden</Text>
            }
            </>
        }
        </>
    )
}

export default AdminPage;