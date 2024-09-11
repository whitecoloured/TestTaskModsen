import {Center, Text, Card, CardHeader, CardBody, Input, CardFooter, Button, Select, NumberInput, NumberInputField, NumberInputStepper, NumberIncrementStepper, NumberDecrementStepper, Flex, VStack} from "@chakra-ui/react";
import { useState } from "react";
import { createEvent } from "../../../processing";
import { useNavigate } from "react-router-dom";
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
            <Input placeholder="Name" onChange={(e)=> setEvent({...event, Name:e.target.value})}></Input>
            <Input placeholder="Descripton" onChange={(e)=> setEvent({...event, Description:e.target.value})}></Input>
            <Input placeholder="Event Date (ex. 2024-08-31T20:00)" onChange={(e)=> setEvent({...event, EventDate:e.target.value})}></Input>
            <Input placeholder="Event Place (ex. ул. Интернациональная 25)" onChange={(e)=> setEvent({...event, EventPlace:e.target.value})}></Input>
            <Select onChange={(e)=> setEvent({...event, Category:e.target.value})}>
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
            <NumberInput min={3} max={10} onChange={(_, numValue)=> setEvent({...event, MaxAmountOfMembers:numValue})}>
                <NumberInputField />
                <NumberInputStepper>
                    <NumberIncrementStepper />
                    <NumberDecrementStepper />
                </NumberInputStepper>
            </NumberInput>
            <Input placeholder="Image URL" onChange={(e)=> setEvent({...event, ImageURL:e.target.value})}></Input>
        </VStack>
    </Flex>
    )
}

function CreateForm()
{

    async function CreateFunction() {
        var response=await createEvent(event);
        if(response.status===200)
        {
            setShowAlertonCreate(true);
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
        Name:null,
        Description:null,
        EventDate:null,
        EventPlace:null,
        Category:null,
        MaxAmountOfMembers:null,
        ImageURL:null
    })
    
    const[showAlertonCreate, setShowAlertonCreate]=useState(false);
    const[showInvalidDataErrorAlert, setShowInvalidDataErrorAlert]=useState(false);
    const[showIfEventExistErrorAlert, setShowIfEventExistErrorAlert]=useState(false);

    const navigate=useNavigate();

    return(
        <Center minH={"3xl"}>
            <Card w={"50%"} h={"80%"} bg={"white"} boxShadow={"2px 5px 5px 2px"}>
                <CardHeader borderBottom={"1px solid black"} marginBottom={"3%"}>
                    <Center>
                        <Text fontWeight={"bold"} fontSize={"45px"}>Create Event</Text>
                    </Center>
                </CardHeader>
                <CardBody>
                    <Inputs event={event} setEvent={setEvent}/>
                </CardBody>
                <CardFooter>
                    <Center w={"100%"}>
                        <Button colorScheme="teal" size={"lg"} marginRight={"1%"} onClick={()=> CreateFunction()}>Submit</Button>
                        <Button colorScheme="gray" size={"lg"} onClick={()=> navigate("/admin")}>Go back</Button>
                    </Center>
                </CardFooter>

                <AlertMessage
                isOpen={showAlertonCreate}
                headMessage={"Congrats!"}
                bodyMessage={"You've created an event!"}
                onClose={setShowAlertonCreate}
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
                headMessage={"Create error"}
                bodyMessage={"You seems want to create an event that already exists. Try to put another data."}
                onClose={setShowIfEventExistErrorAlert}
                />
            </Card>
        </Center>
    )
}

export default CreateForm;