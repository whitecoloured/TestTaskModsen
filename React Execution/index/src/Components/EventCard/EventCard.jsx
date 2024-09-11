import {Card, CardBody, CardFooter, CardHeader, Center,Text, Image } from "@chakra-ui/react";
import moment from "moment"

function EventCard({
    EventName, 
    EventCategory,
    EventPlace, 
    EventDate, 
    imageUrl,
    footerButtons})
{
    return(
        <Center marginBottom={"0.5%"}>
            <Card w={"65%"}>
                <CardHeader borderBottom={"1px solid black"}>
                    {imageUrl&&<Image boxSize={"75px"} src={imageUrl}></Image>}
                    <Text fontWeight={"bold"} fontSize={"25px"}>{EventName}</Text>
                </CardHeader>
                <CardBody>
                    <Text fontWeight={"bold"}>{EventCategory}</Text>
                    <Text>{EventPlace}</Text>
                    <Text>{moment(EventDate).format("DD/MM/YYYY HH:mm")}</Text>
                </CardBody>
                <CardFooter>
                    {footerButtons}
                </CardFooter>
            </Card>
        </Center>
    )
}

export default EventCard;