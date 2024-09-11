import {Center, Text, Card, CardHeader, CardBody, Input, CardFooter, Button, VStack, Flex} from "@chakra-ui/react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { onRegister } from "../../processing";
import AlertMessage from "../../Components/Alert/AlertMessage";


function Inputs({user, setUser})
{
    return(
        <Flex>
        <VStack w={"35%"} spacing={7} marginRight={"4%"}>
            <Text fontSize={"25px"} color={"white"}>Email: </Text>
            <Text fontSize={"25px"} color={"white"}>Name: </Text>
            <Text fontSize={"25px"} color={"white"}>Surname: </Text>
            <Text fontSize={"25px"} color={"white"}>Birth Date: </Text>
        </VStack>
        <VStack w={"55%"} spacing={7} color={"black"}>
            <Input placeholder="Email" bg={"white"} onChange={(e)=> setUser({...user, Email:e.target.value.trim()})}></Input>
            <Input placeholder="Name" bg={"white"} onChange={(e)=> setUser({...user, Name:e.target.value.trim()})}></Input>
            <Input placeholder="Surname" bg={"white"} onChange={(e)=> setUser({...user, Surname:e.target.value.trim()})}></Input>
            <Input placeholder="Birth Date (ex. 2004-08-26)" bg={"white"} onChange={(e)=> setUser({...user, BirthDate:e.target.value})}></Input>
        </VStack>
        </Flex>
    )
}

function RegisterPage()
{
    async function RegisterFunction()
    {
        var response=await onRegister(user);
        if (response.status===200)
        {
            setShowAlertOnRegister(true);
            localStorage.setItem("lolipop-token", response.data);
        }
        else setShowErrorAlert(true);
    }

    const [user, setUser]=useState({
        Email:null,
        Name:null,
        Surname:null,
        BirthDate:null
    })

    const [showAlertOnRegister, setShowAlertOnRegister]=useState(false);
    const [showErrorAlert, setShowErrorAlert]=useState(false);

    const navigate=useNavigate();

    return(
        <Center h={"630px"}>
            <Card w={"50%"} h={"80%"} bg={"gray.900"} boxShadow={"2px 5px 5px 2px"}>
                <CardHeader borderBottom={"1px solid white"} marginBottom={"3%"}>
                    <Center>
                        <Text color={"white"} fontWeight={"bold"} fontSize={"45px"}>Register</Text>
                    </Center>
                </CardHeader>
                <CardBody>
                    <Inputs user={user} setUser={setUser}/>
                </CardBody>
                <CardFooter>
                    <Center w={"100%"}>
                        <Button colorScheme="teal" size={"lg"} onClick={()=> RegisterFunction()}>Submit</Button>
                    </Center>
                </CardFooter>
            </Card>

            <AlertMessage
            isOpen={showAlertOnRegister}
            headMessage={"Congrats!"}
            bodyMessage={"You've successfully registered!"}
            onClose={setShowAlertOnRegister}
            onButtonClick={()=> navigate("/events")}
            />

            <AlertMessage
            isOpen={showErrorAlert}
            headMessage={"Register error"}
            bodyMessage={"There is a problem occured in signing up. Check if you put valid data"}
            onClose={setShowErrorAlert}/>
        </Center>
    )
}

export default RegisterPage;