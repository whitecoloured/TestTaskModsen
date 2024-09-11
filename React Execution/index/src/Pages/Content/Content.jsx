import { Box, Button, Card, CardBody, CardFooter, CardHeader, Center, Flex, Input, Select, Text, Image, Square } from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {getEvents} from "../../processing";
import EventCard from "../../Components/EventCard/EventCard";
import SquarePage from "../../Components/SquarePage/SquarePage";


function Filters({filters, setFilters})
{
    return(
        <>
            <Input w={'30%'} marginRight={"10%"} placeholder="Search for..." 
            onChange={(e)=> setFilters({...filters, SearchValue:e.target.value})}></Input>
            <Select w={'13%'} marginRight={"5%"} 
            onChange={(e)=> 
            {
            const[sortItem, byAsc]=e.target.value.split(',');
            setFilters({...filters, SortItem:sortItem, OrderByAsc:byAsc})}
            }>
                <option value={["", true]}>Sort by:</option>
                <option value={["category", true]}>By ascending: by category</option>
                <option value={["category", false]}>By descending: by category</option>
                <option value={["place", true]}>By ascending: by place</option>
                <option value={["place", false]}>By descending: by place</option>
            </Select>
            <Select w={'15%'} onChange={(e)=> setFilters({...filters, SearchCategory:e.target.value})}>
                <option value={""}>Search by category:</option>
                <option value={0}>Games</option>
                <option value={1}>Politics</option>
                <option value={2}>Films</option>
                <option value={3}>Society</option>
                <option value={4}>Business</option>
                <option value={5}>Videos</option>
                <option value={6}>Internet</option>
                <option value={7}>History</option>
            </Select>
        </>
    )
}

function Content()
{
    function setCurrentPage(value)
    {
        setFilters({...filters, Page:value})
    }

    const [events, setEvents]=useState([]);
    const [isLoading, setIsLoading]=useState(true);
    const [eventsAmount,setEventsAmount]=useState(null);
    const [filters, setFilters]=useState({
        SearchValue:null,
        SearchCategory:null,
        SearchDate:null,
        SortItem:null,
        OrderByAsc:true,
        Page:1
    })

    const navigate=useNavigate();

    useEffect(()=>
    {
        const getData=async()=>
        {
            var data=await getEvents(filters);
            setEvents(data.eventsList);
            setEventsAmount(data.eventsAmount);
            if (events)
            {
                setIsLoading(false);
            }
        }
        getData();
    },[filters])

    let pagesAmount=Math.ceil(eventsAmount/3);

    const eventsData=events.map(e=>
        <EventCard key={e.id} 
        EventID={e.id}
        EventName={e.name} 
        EventCategory={e.category}
        EventPlace={e.eventPlace}
        EventDate={e.eventDate}
        imageUrl={e.imageURL}
        isAvailable={e.isAvailable}
        footerButtons={
        <Button colorScheme="gray" onClick={()=> navigate("info", {state:{id:e.id}})}>See info</Button>
        }/>
    );
    
    const squarePages=Array(pagesAmount).fill(null).map((_, index)=>
        <SquarePage key={index+1} value={index+1} setCurrentPage={setCurrentPage} />
    )

    if (eventsAmount<=3 && filters.Page!=1)
    {
        setCurrentPage(1);
    }

    return(
    <>
    {!isLoading&&
    <Box minHeight={"xl"} maxWidth={"9xl"}>
        <Center marginBottom={"1%"}>
            <Filters filters={filters} setFilters={setFilters}/>
        </Center>
        <Center marginBottom={"1%"}>
            <Box w={"90%"}>
                {eventsData}
            </Box>
        </Center>
        <Center marginBottom={"1.2%"}>
            {squarePages}
        </Center>
    </Box>}
    </>
    )
}

export default Content;