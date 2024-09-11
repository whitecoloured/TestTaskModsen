import { AlertDialog, AlertDialogBody, AlertDialogFooter, AlertDialogHeader, AlertDialogContent, AlertDialogOverlay, Button, useDisclosure} from '@chakra-ui/react'
import { useEffect, useRef } from 'react'

function AlertMessage({isOpen, headMessage, bodyMessage, onClose, onButtonClick})
{

    function HideMessage()
    {
        if (onButtonClick)
        {
            onButtonClick();
        }
        onClose(false);
    }

    const {onOpen}=useDisclosure();
    const closeRef=useRef();

    useEffect(()=>
    {
        onOpen();
    },[])

    return(
        <AlertDialog
        isOpen={isOpen}
        leastDestructiveRef={closeRef}
        onClose={()=> onClose(false)}
        isCentered>
            <AlertDialogOverlay/>
            <AlertDialogContent>
                <AlertDialogHeader>
                    {headMessage}
                </AlertDialogHeader>
                <AlertDialogBody>
                    {bodyMessage}
                </AlertDialogBody>
                <AlertDialogFooter>
                    <Button colorScheme='teal' size={"lg"} onClick={()=> HideMessage()}>
                        OK
                    </Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    )
}

export default AlertMessage;