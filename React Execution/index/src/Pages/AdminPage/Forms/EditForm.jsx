import {Center, Text, Card, CardHeader, CardBody, Input, CardFooter, Button, Select, NumberInput, NumberInputField, NumberInputStepper, NumberIncrementStepper, NumberDecrementStepper, Flex, VStack} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { getEvent, editEvent, getUserRole } from "../../../processing";
import { useNavigate, useLocation } from "react-router-dom";
import AlertMessage from "../../../Components/Alert/AlertMessage";

function Inputs({event, setEvent})
{
    return(
    <Flex>
        <VStack w={"35%"} spacing={4} fontSize={"25px"} marginRight={"4%"}>
            <Text>Name: </Text>
            <Text>Description: </Text>
            <Text>Event Date: </Text>
            <Text>Event Place: </Text>
            <Text>Category: </Text>
            <Text>Maximum Amount: </Text>
            <Text>Image URL: </Text>
        </VStack>
        <VStack w={"55%"} spacing={3.5}>
            <Input value={event.name} placeholder="Name" onChange={(e)=> setEvent({...event, name:e.target.value})}></Input>
            <Input value={event.description} placeholder="Descripton" onChange={(e)=> setEvent({...event, description:e.target.value})}></Input>
            <Input value={event.eventDate} placeholder="Event Date (ex. 2024-08-31T20:00)" onChange={(e)=> setEvent({...event, eventDate:e.target.value})}></Input>
            <Input value={event.eventPlace} placeholder="Event Place (ex. ул. Интернациональная 25)" onChange={(e)=> setEvent({...event, eventPlace:e.target.value})}></Input>
            <Select defaultValue={event.category} onChange={(e)=> setEvent({...event, category:e.target.value})}>
                <option value={-1}>--</option>
                <option value={0}>Games</option>
                <option value={1}>Politics</option>
                <option value={2}>Films</option>
                <option value={3}>Society</option>
                <option value={4}>Business</option>
                <option value={5}>Videos</option>
                <option value={6}>Internet</option>
                <option value={7}>History</option>
            </Select>
            <NumberInput defaultValue={event.maxAmountOfMembers} min={3} max={10} onChange={(_, numValue)=> setEvent({...event, maxAmountOfMembers:numValue})}>
                <NumberInputField />
                <NumberInputStepper>
                    <NumberIncrementStepper />
                    <NumberDecrementStepper />
                </NumberInputStepper>
            </NumberInput>
            <Input value={event.imageURL?event.imageURL:""} placeholder="Image URL" onChange={(e)=> setEvent({...event, imageURL:e.target.value})}></Input>
        </VStack>
    </Flex>
    )
}

function EditForm()
{

    async function EditFunction()
    {
        var response=await editEvent(event, event.id);
        if (response.status===200)
        {
            setShowAlertonEdit(true);
        }
        if (response.status===406)
        {
            setShowIfEventExistErrorAlert(true);
        }
        if (response.status===400)
        {
            setShowInvalidDataErrorAlert(true);
        }
    }

    const [event, setEvent]=useState({
        name:null,
        description:null,
        eventDate:null,
        eventPlace:null,
        category:null,
        maxAmountOfMembers:null,
        imageURL:null
    });
    const [isLoading, setIsLoading]=useState(true);

    const [role, setRole]=useState(null);

    const [showAlertonEdit, setShowAlertonEdit]=useState(false);
    const [showInvalidDataErrorAlert, setShowInvalidDataErrorAlert]=useState(false);
    const [showIfEventExistErrorAlert, setShowIfEventExistErrorAlert]=useState(false);

    const navigate=useNavigate();

    const location=useLocation();
    const index=location.state.id;

    useEffect(()=>
    {
        const getData=async()=>
        {
            var data=await getEvent(index);
            var roleData=await getUserRole();
            setEvent(data)
            setRole(roleData);
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
        <Center minH={"3xl"}>
            <Card w={"50%"} h={"80%"} bg={"white"} boxShadow={"2px 5px 5px 2px"}>
                <CardHeader borderBottom={"1px solid black"} marginBottom={"3%"}>
                    <Center>
                        <Text fontWeight={"bold"} fontSize={"45px"}>Update Event</Text>
                    </Center>
                </CardHeader>
                <CardBody>
                    <Inputs event={event} setEvent={setEvent}/>
                </CardBody>
                <CardFooter>
                    <Center w={"100%"}>
                        <Button colorScheme="teal" size={"lg"} marginRight={"1%"} onClick={()=> EditFunction()}>Submit</Button>
                        <Button colorScheme="gray" size={"lg"} onClick={()=> navigate("/admin")}>Go back</Button>
                    </Center>
                </CardFooter>
                
                <AlertMessage
                isOpen={showAlertonEdit}
                headMessage={"Congrats!"}
                bodyMessage={"The event has been updated!"}
                onClose={setShowAlertonEdit}
                onButtonClick={()=> navigate("/admin")}
                />

                <AlertMessage
                isOpen={showInvalidDataErrorAlert}
                headMessage={"Oops"}
                bodyMessage={"You have put invalid data! Try again."}
                onClose={setShowInvalidDataErrorAlert}
                />

                <AlertMessage
                isOpen={showIfEventExistErrorAlert}
                headMessage={"Update error"}
                bodyMessage={"You seems want to update event data that already exists or put a number in the 'Maximum Amount' gap less than the actual amount of members. Try to put another data."}
                onClose={setShowIfEventExistErrorAlert}
                />
            </Card>
        </Center>
        }
        </>
    )
}

export default EditForm;