import React from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import LoginPage from './component/Login/LoginPage';
import SignUp from './component/signUp/SignUp';
import PopupMenu from './component/PopUpMenu/PopupMenu';
import HomePage from './component/HomePage/HomePage';
import ResetPassword from './component/ResetPassword/ResetPassword';
import OnlineForum from './component/OnlineForum/OnlineForum';
import MapMenu from './component/MapMenu/MapMenu';
import SendEmail from './component/SendEmail/SendEmail';
import { AuthProvider } from './component/AuthContext';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function App() {
  const user = localStorage.getItem("token");
  return (
    <Router>
      <div className="App">  
      <ToastContainer />  
        <AuthProvider>  
            <Routes>
              <Route exact path="/" element={<LoginPage />} />
              <Route path="/sign-in" element={<LoginPage />} />
              <Route path="/sign-up" element={<SignUp />} />
              <Route path="/popup-menu" element={<PopupMenu/>} />
              <Route path="/home-page" element={<HomePage/>} />
              <Route path="/send-recovery-email" element={<SendEmail/>}/>
              <Route path="/reset-password" element={<ResetPassword/>}/>
              <Route path="/home-page/map-menu" element={<MapMenu/>}/>
              <Route path="/home-page/online-forum" element={<OnlineForum/>}/>
            </Routes>
         </AuthProvider>         
      </div>
    </Router>
    
  
  
  
  )
}

export default App;
