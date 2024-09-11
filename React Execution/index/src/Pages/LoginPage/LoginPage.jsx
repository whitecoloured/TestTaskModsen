import {Center, Text, Card, CardHeader, CardBody, Input, CardFooter, Button} from "@chakra-ui/react";
import { useState } from "react";
import { onLogin } from "../../processing";
import { useNavigate } from "react-router-dom";
import AlertMessage from "../../Components/Alert/AlertMessage";


function LoginPage()
{
    async function LoginFunction()
    {
        var response=await onLogin(user);
        if (response.status!==200)
        {
            setShowErrorAlert(true);
            return;
        }
        if (response.data)
        {
            localStorage.setItem("lolipop-token", response.data);
        }
        if (localStorage.getItem("lolipop-token"))
        {
            navigate("/events");
        }
    }

    const [user, setUser]=useState({
        Email:null
    })

    const[showErrorAlert, setShowErrorAlert]=useState(false);

    const navigate=useNavigate();

    return(
        <Center h={"630px"}>
            <Card w={"50%"} h={"50%"} bg={"gray.900"} boxShadow={"2px 5px 5px 2px"}>
                <CardHeader borderBottom={"1px solid black"} borderColor={"white"} marginBottom={"3%"}>
                    <Center>
                        <Text color={"white"} fontWeight={"bold"} fontSize={"45px"}>Login</Text>
                    </Center>
                </CardHeader>
                <CardBody>
                    <Center>
                        <Text marginRight={"2%"} fontSize={"20px"} color={"white"}>Email: </Text>
                        <Input id="email" placeholder="Email" w={"50%"} color={"black"} bg={"white"} onChange={(e)=> setUser({...user,Email:e.target.value})}/>
                    </Center>
                </CardBody>
                <CardFooter>
                    <Center w={"100%"}>
                        <Button colorScheme="teal" size={"lg"} onClick={()=> LoginFunction()}>Submit</Button>
                    </Center>
                </CardFooter>
            </Card>

            <AlertMessage
            isOpen={showErrorAlert}
            headMessage={"Login error"}
            bodyMessage={"There is a problem occured in signing in!"}
            onClose={setShowErrorAlert}
            />
        </Center>
    )
}

export default LoginPage;