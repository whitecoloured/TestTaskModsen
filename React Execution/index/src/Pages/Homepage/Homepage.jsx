import {Box, Button, Center, Text} from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";

function Homepage()
{
    const navigate=useNavigate();
    return(
        <Box h={"630px"}>
            <Center h={"50%"}>
                <Text
                fontSize={"50px"}
                fontWeight={"bold"}
                fontFamily={"Arial"}
                >Welcome to our events!</Text>
            </Center>
            <Center h={"15%"}>
                <Button height={"100%"} width={"25%"} bg={"teal.900"} color={"white"} _hover={{bg:"teal.500"}} onClick={()=> navigate("events")}>
                    <Text fontSize={"135%"}>Get to events</Text>
                </Button>
            </Center>
        </Box>
    )
}

export default Homepage;