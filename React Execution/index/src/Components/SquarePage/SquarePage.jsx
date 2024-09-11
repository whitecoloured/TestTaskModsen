import { Button } from "@chakra-ui/react"

function SquarePage({value, setCurrentPage})
{
    return(
        <Button bg='gray.600' color='white' marginRight={"0.3%"} onClick={()=>setCurrentPage(value)}>
            {value}
        </Button>
    )
}

export default SquarePage;