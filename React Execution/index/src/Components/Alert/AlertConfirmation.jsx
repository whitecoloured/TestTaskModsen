import { AlertDialog, AlertDialogBody, AlertDialogFooter, AlertDialogHeader, AlertDialogContent, AlertDialogOverlay, Button, useDisclosure} from '@chakra-ui/react'
import { useEffect, useRef } from 'react';


function AlertConfirmation({isOpen,headMessage,bodyMessage, onConfirm,onClose})
{

    function Confirmation()
    {
        onConfirm();
        onClose(false);
    }

    const {onOpen}=useDisclosure();
    const closeRef=useRef();
    
    useEffect(()=>
    {
        onOpen()
    },[])
    return(
        <AlertDialog
        isOpen={isOpen}
        leastDestructiveRef={closeRef}
        onClose={()=>onClose(false)}
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
                    <Button ref={closeRef} onClick={()=>onClose(false)} colorScheme='red' marginRight={"1%"}>
                        No
                    </Button>
                    <Button colorScheme='green' onClick={()=> Confirmation()}>
                        Yes
                    </Button>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    )
}

export default AlertConfirmation;