import {Card, CardBody, CardFooter, CardHeader, Center, Text, Image, Button, Flex, Box,
    Table, Thead, Tbody, Tr, Th, Td, TableContainer} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { getEventInfo, getMembersByEvent, subOnEvent } from "../../processing";
import { useLocation, useNavigate } from "react-router-dom";
import moment from "moment";
import AlertMessage from "../../Components/Alert/AlertMessage";

function MemberRow({name, surname})
{
    return(
    <Td>{name + ' ' + surname}</Td>
    )
}

function MembersTable({eventMembers})
{
    const Members=eventMembers.map(member=>
        <Tr key={member.id}>
            <MemberRow name={member.name} surname={member.surname}/>
        </Tr>
    )
    return(
        <TableContainer maxH={"160px"} overflowY={"scroll"}>
            <Table>
                <Thead>
                    <Tr>
                        <Th>Name</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {Members}
                </Tbody>
            </Table>
        </TableContainer>
    )
}

function EventCardBody({event, eventMembers})
{
    return(
        <Flex>
            <Box w={"50%"} marginRight={"2%"}>
                <Text fontSize={"20px"} marginBottom={"1%"}>Info:</Text>
                <Text fontSize={"18px"}>Category : <span style={{fontWeight:"bold"}}>{event.category}</span></Text>
                <Text fontSize={"18px"}>Description : {event.description}</Text>
                <Text fontSize={"18px"}>Place : {event.eventPlace}</Text>
                <Text fontSize={"18px"}>Date : {moment(event.eventDate).format("DD/MM/YYYY HH:mm")}</Text>
                <Text fontSize={"18px"}>Maximum amount : {event.maxAmountOfMembers}</Text>
                {!event.isAvailable&&<Text color={"red"} fontSize={"18px"} fontWeight={"bold"}>No space left!</Text>}
                {event.isExpired&&<Text color={"red"} fontSize={"18px"} fontWeight={"bold"}>The event have already passed!</Text>}
            </Box>
            <Box w={"50%"}>
                <Text fontWeight={"bold"} fontSize={"25px"}>Members: </Text>
                {eventMembers.length!==0?<MembersTable eventMembers={eventMembers}/>:<Text>No members!</Text>}
            </Box>
        </Flex>
    )
}

function EventInfo()
{

    async function SubcribeOnEvent()
    {
        var response=await subOnEvent(index);
        if (response.status===200)
        {
            setShowAlertonSub(true);
        }
        if (response.status===401)
        {
            setShowLogInErrorAlert(true);
        }
        if (response.status===406)
        {
            setShowAlreadySubbedErrorAlert(true);
        }
    }

    const navigate=useNavigate();

    const location=useLocation();
    const index=location.state.id;

    const[event,setEvent]=useState(null);
    const[isLoading, setIsLoading]=useState(true);
    const[eventMembers, setEventMembers]=useState([]);

    const [showAlertonSub, setShowAlertonSub]=useState(false);
    const [showLogInErrorAlert, setShowLogInErrorAlert]=useState(false);
    const [showAlreadySubbedErrorAlert, setShowAlreadySubbedErrorAlert]=useState(false);

    useEffect(()=>
    {
        const getData= async()=>
        {
            var data=await getEventInfo(index);
            var members= await getMembersByEvent(index)
            setEvent(data);
            setEventMembers(members);
            if (event)
            {
                setIsLoading(false)
            }
        }
        getData();
    },[!event])
    return(
        <>
        {!isLoading&&
        <Center h={"630px"}>
            <Card w={"95%"} h={"95%"}>
                <CardHeader borderBottom={"1px solid black"}>
                    {event.imageURL&&<Image boxSize={"100px"} src={event.imageURL}></Image>}
                    <Text fontWeight={"bold"} fontSize={"40px"}>{event.name}</Text>
                </CardHeader>
                <CardBody>
                    <EventCardBody event={event} eventMembers={eventMembers}/>
                </CardBody>
                <CardFooter>
                    <Flex justify={"space-between"} w={"100%"}>
                        <Button colorScheme="red" size={"lg"} isDisabled={!event.isAvailable || event.isExpired} onClick={()=> SubcribeOnEvent()}>Take a part</Button>
                        <Button bg={"gray.300"} size={"lg"} onClick={()=> navigate("/events")}>Go back</Button>
                    </Flex>
                </CardFooter>

                <AlertMessage
                isOpen={showAlertonSub}
                headMessage={"Congrats!"}
                bodyMessage={"You've subscribed on the event!"}
                onClose={setShowAlertonSub}
                onButtonClick={()=> navigate("/events")}/>

                <AlertMessage
                isOpen={showLogInErrorAlert}
                headMessage={"Warning"}
                bodyMessage={"You haven't logged in!"}
                onClose={setShowLogInErrorAlert}/>

                <AlertMessage
                isOpen={showAlreadySubbedErrorAlert}
                headMessage={"Warning"}
                bodyMessage={"You have already subscribed to the event!"}
                onClose={setShowAlreadySubbedErrorAlert}/>
            </Card>
        </Center>}
        </>
    )
}

export default EventInfo;