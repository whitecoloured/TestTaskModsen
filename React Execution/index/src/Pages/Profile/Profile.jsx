import { Box, Button, Card, CardBody, CardFooter, CardHeader, Center, Container, Flex, Text,
 Table, Thead, Tbody, Tr, Th, Td, TableCaption, TableContainer } from "@chakra-ui/react";
import { deleteProfile, getUserData, getMembersEvents, cancelEvent, getUserRole } from "../../processing";
import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import moment from "moment";
import AlertConfirmation from "../../Components/Alert/AlertConfirmation";
import AlertMessage from "../../Components/Alert/AlertMessage";



function MembersEvents({events, setEvents})
{


    function onClickCancelButton(EventID)
    {
        setShowAlertonCancel(true);
        currentID.current=EventID;
    }
    
    async function CancelEventFunction()
    {
        var response = await cancelEvent(currentID.current);
        if (response.status!==200)
        {
            setShowErrorAlert(true);
        }
    }

    const [showAlertonCancel, setShowAlertonCancel]=useState(false);
    const [showErrorAlert, setShowErrorAlert]=useState(false);
    const currentID=useRef(null);

    const userEvents=events.map(event=>
        <Tr key={event.id}>
            <Td>{event.name}</Td>
            <Td>{moment(event.registrationDate).format("DD/MM/YYYY HH:mm")}</Td>
            <Td><Button colorScheme="red" onClick={()=> onClickCancelButton(event.id)}>Cancel</Button></Td>
        </Tr>
    )
    return(
        <TableContainer bg={"white"} maxH={"224px"} overflowY={"scroll"}>
            <Table>
                <TableCaption placement="top">
                    <Text>All events you've subscribed on</Text>
                </TableCaption>
                <Thead>
                    <Tr>
                        <Th>Name</Th>
                        <Th>Registration Date</Th>
                        <Th>Cancel Event</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    {userEvents}
                </Tbody>
            </Table>
            
            <AlertConfirmation
            isOpen={showAlertonCancel}
            headMessage={"Cancel event"}
            bodyMessage={"Are you sure you want to cancel the event?"}
            onClose={setShowAlertonCancel}
            onConfirm={()=>CancelEventFunction()}
            />

            <AlertMessage
            isOpen={showErrorAlert}
            headMessage={"Oops"}
            bodyMessage={"Something went wrong..."}
            onClose={setShowErrorAlert}
            />
            
        </TableContainer>
    )
}


function ProfilePage()
{

    function LogoutFunction()
    {
        localStorage.removeItem("lolipop-token");
        navigate("/")
    }

    async function DeleteProfileFunction()
    {
        await deleteProfile();
        localStorage.removeItem("lolipop-token");
        navigate("/");
    }

    const [user, setUser]=useState(null);
    const [isLoading, setIsLoading]=useState(true);
    const [events, setEvents]=useState([])
    const [role, setRole]=useState(null)

    const [showAlertonDelete,setShowAlertonDelete]=useState(false);
    const [showAlertonLogout,setShowAlertonLogout]=useState(false);

    const navigate=useNavigate();

    useEffect(()=>
    {
        const getUser=async()=>
        {
            var data=await getUserData();
            var eventsData=await getMembersEvents();
            var roleData=await getUserRole();
            setUser(data);
            setEvents(eventsData);
            setRole(roleData);
            if (user)
            {
                setIsLoading(false)
            }
        }
        getUser()
    },[!user])
    return(
        <>
        {!isLoading&&
        <Center h={"630px"}>
                <Card bg={"gray.900"} boxShadow={"2px 5px 5px 2px"} w={"60%"}>
                    <CardHeader borderBottom={"1px solid white"}>
                        <Center w={"100%"}>
                            <Text color={"white"} fontSize={"40px"} fontWeight={"bold"}>Profile</Text>
                        </Center>
                    </CardHeader>
                    <CardBody>
                        <Text color={"white"} fontSize={"25px"} marginBottom={"1%"}>Name: {user.name}</Text>
                        <Text color={"white"} fontSize={"25px"} marginBottom={"1%"}>Surname: {user.surname}</Text>
                        {user.birthDate&&<Text color={"white"} fontSize={"25px"} marginBottom={"1%"}>Birth Date: {moment(user.birthDate).format("MMMM Do YYYY")}</Text>}
                        {events.length!==0&&<MembersEvents events={events} setEvents={setEvents}/>}
                    </CardBody>
                    <CardFooter>
                        <Flex marginLeft={"auto"}>
                            <Button colorScheme="red" size={"lg"} marginRight={"3.5%"} onClick={()=> setShowAlertonLogout(true)}>Log Out</Button>
                            {role==="Admin"?<Button colorScheme="teal" size={"lg"} onClick={()=> navigate("/admin")}>Go To Admin</Button>:
                            <Button colorScheme="red" size={"lg"} onClick={()=> setShowAlertonDelete(true)}>Delete Profile</Button>}
                        </Flex>
                    </CardFooter>

                    {/* Alert message on delete */}
                    <AlertConfirmation
                    isOpen={showAlertonDelete}
                    headMessage={"Delete profile"} 
                    bodyMessage={"Are you sure you want to delete your profile?"}
                    onClose={setShowAlertonDelete}
                    onConfirm={()=>DeleteProfileFunction()}/>

                    {/* Alert message on logout */}
                    <AlertConfirmation 
                    isOpen={showAlertonLogout}
                    headMessage={"Logout"} 
                    bodyMessage={"Are you sure you want to logout?"}
                    onClose={setShowAlertonLogout}
                    onConfirm={()=>LogoutFunction()}/>
                </Card>
        </Center>}
        </>
    )
}

export default ProfilePage;