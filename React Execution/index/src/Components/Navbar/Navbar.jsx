import { Box, Button, Center, Container, Text } from "@chakra-ui/react";
import { Link, useNavigate } from "react-router-dom";


function GuestButtons({navigate})
{
    return(
        <>
        <Button colorScheme={"teal"} size={"lg"} marginRight={"3%"} onClick={()=> navigate("login")}>
            Log in
        </Button>
        <Button colorScheme={"teal"} size={"lg"} onClick={()=> navigate("register")}>
            Register
        </Button>
        </>
    )
}


function UserButton()
{
    return(
        <Text fontSize={"30px"} color={"white"} marginRight={"3%"}>
            <Link to={"profile"}>
            Profile
            </Link>
        </Text>
    )
}



function Navbar()
{
    

    const navigate=useNavigate();

    return(
        <Container maxW={"9xl"} h={"6rem"} backgroundColor={"#718096"} marginBottom={"0.3%"} centerContent>
            <Box h={"100%"} w={"90%"}>
                <Center float={"left"} h={"100%"} w={"25%"}>
                    <Text fontWeight={"bold"} fontSize={"35px"} color={"white"}>Events</Text>
                </Center>
                <Center float={"right"} h={"100%"} w={"60%"}>
                    <Text fontSize={"30px"} color={"white"} marginRight={"3%"}>
                        <Link to={"/"}>
                            Home
                        </Link>
                    </Text>
                    <Text fontSize={"30px"} color={"white"} marginRight={"3%"}>
                        <Link to={"events"}>
                            Events
                        </Link>
                    </Text>
                {localStorage.getItem("lolipop-token")?
                <>
                <UserButton/>
                </>
                :
                <GuestButtons navigate={navigate}/>}
                </Center>
            </Box>
        </Container>
    )
}

export default Navbar;