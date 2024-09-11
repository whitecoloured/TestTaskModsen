import Navbar from "./Components/Navbar/Navbar"
import Content from "./Pages/Content/Content"
import Homepage from "./Pages/Homepage/Homepage"
import EventInfo from "./Pages/EventInfo/EventInfo"
import LoginPage from "./Pages/LoginPage/LoginPage"
import RegisterPage from "./Pages/RegisterPage/RegisterPage"
import ProfilePage from "./Pages/Profile/Profile"
import AdminPage from "./Pages/AdminPage/AdminPage"
import CreateForm from "./Pages/AdminPage/Forms/CreateForm"
import EditForm from "./Pages/AdminPage/Forms/EditForm"
import {BrowserRouter, Routes, Route} from "react-router-dom"

function App() {

  return (
    <BrowserRouter>
    <Navbar/>
    <Routes>
      <Route path="/">
        <Route index element={<Homepage/>}/>
        <Route path="events">
          <Route index element={<Content/>}/>
          <Route path="info" element={<EventInfo/>}/>
        </Route>
        <Route path="login" element={<LoginPage/>}/>
        <Route path="register" element={<RegisterPage/>}/>
        <Route path="profile" element={<ProfilePage/>}/>
        <Route path="admin">
          <Route index element={<AdminPage/>}/>
          <Route path="createevent" element={<CreateForm/>}/>
          <Route path="updateevent" element={<EditForm/>}/>
        </Route>
      </Route>
    </Routes>
    </BrowserRouter>
  )
}

export default App
