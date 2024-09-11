import axios from "axios"


export const getEvents= async (filters)=>
{
    try
    {
        var response= await axios.get("https://localhost:44309/api/Events/GetAllEvents",
            {
            params:{
                SearchValue:filters?.SearchValue,
                SearchCategory:filters?.SearchCategory,
                SearchDate:filters?.SearchDate,
                SortItem:filters?.SortItem,
                OrderByAsc:filters.OrderByAsc,
                Page:filters.Page
            }
        }
    );
        return response.data;
    }
    catch(e)
    {
        console.error(e)
    }
}

export const getEvent = async(eventID)=>
{
    try{
        var response=await axios.get("https://localhost:44309/api/Events/GetEventById", 
            {
                params:
                {
                    id:eventID
                }
            }
        )
        return response.data
    }
    catch(e)
    {
        console.error(e)
    }
}

export const getEventInfo = async(eventID)=>
    {
        try{
            var response=await axios.get("https://localhost:44309/api/Events/GetEventInfoById", 
                {
                    params:
                    {
                        id:eventID
                    }
                }
            )
            return response.data
        }
        catch(e)
        {
            console.error(e)
        }
    }

export const createEvent=async(event)=>
{
    try{
        var response=await axios.post("https://localhost:44309/api/Events/AddEvent", event,
            {
                headers:{
                    'Content-Type':'application/json',
                    'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
                }
            }
        )
        return response;
    }
    catch(e)
    {
        console.error(e)
        return e;
    }
}

export const editEvent=async(event, eventID)=>
{
    try{
        var response=await axios.put("https://localhost:44309/api/Events/UpdateEvent", event,{
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            },
            params:
            {
                EventID:eventID
            }
        })
        return response;
    }
    catch(e)
    {
        console.error(e)
        return e;
    }
}

export const deleteEvent=async(eventID)=>
{
    try{
        var response=await axios.delete("https://localhost:44309/api/Events/DeleteEvent",{
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            },
            params:
            {
                EventID:eventID
            }
        })
        return response;
    }
    catch(e)
    {
        console.error(e)
        return e;
    }
}

export const getUserData=async ()=>
{
    try{
        var response=await axios.get("https://localhost:44309/api/Users/GetProfileInfo",{
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            }
        })
        return response.data;
    }
    catch(e)
    {
        console.error(e)
    }
}

export const getUserRole=async()=>
{
    try{
        var response=await axios.get("https://localhost:44309/api/Users/GetUserRole",{
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            }
        })
        return response.data
    }
    catch(e)
    {
        console.error(e)
    }
}

export const onLogin=async(user)=>
{
    try
    {
        var response=await axios.post("https://localhost:44309/api/Users/Login", user);
        return response;
    }
    catch(e)
    {
        console.error(e)
        return e;
    }
}

export const onRegister=async(user)=>
{
    try
    {
        var response=await axios.post("https://localhost:44309/api/Users/Register", user);
        return response;
    }
    catch(e)
    {
        console.error(e);
        return e;
    }
}

export const deleteProfile=async()=>
{
    try
    {
        var response=await axios.delete("https://localhost:44309/api/Users/DeleteUserProfile",{
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            }
        })
        return response.status
    }
    catch(e)
    {
        console.error(e)
    }
}

export const getMembersEvents=async()=>
{
    try
    {
        var response=await axios.get("https://localhost:44309/api/Attendings/GetMembersEvents", {
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            }
        })
        return response.data
    }
    catch(e)
    {
        console.error(e)
    }
}

export const getMembersByEvent=async(eventID)=>
{
    try{
        var response=await axios.get("https://localhost:44309/api/Attendings/GetMembersByEvent",{
            params:
            {
                EventID:eventID
            }
        })
        return response.data
    }
    catch(e)
    {
        console.error(e)
    }
}

export const subOnEvent= async(eventID)=>
{
    try
    {
        var response=await axios.post("https://localhost:44309/api/Attendings/SubscribeToEvent",null,
        {
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            },
            params:
            {
                EventID:eventID
            }
        })
        return response;
    }
    catch(e)
    {
        console.error(e)
        return e;
    }
}

export const cancelEvent=async(eventID)=>
{
    try
    {
        var response=await axios.delete("https://localhost:44309/api/Attendings/CancelEvent",{
            headers:{
                'Content-Type':'application/json',
                'Authorization': 'Bearer ' + localStorage.getItem("lolipop-token")
            },
            params:
            {
                EventID:eventID
            }
        })
        return response
    }
    catch(e)
    {
        console.error(e)
        return e
    }
}