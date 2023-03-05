import React from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import LoginPage from './component/Login/LoginPage';
import SignUp from './component/signUp/SignUp';
import PopupMenu from './component/PopUpMenu/PopupMenu';
import HomePage from './component/HomePage/HomePage';
function App() {
  const user = localStorage.getItem("token");
  return (
    <Router>
      <div className="App">
        
   
        
            <Routes>
              {user && <Route exact path="/" element={<HomePage />} />}
              <Route path="/sign-in" element={<LoginPage />} />
              <Route path = "/" exact element = {<Navigate replace to = "/sign-in"/>}/>
              <Route path="/sign-up" element={<SignUp />} />
              <Route path="/popup-menu" element={<PopupMenu/>} />
            </Routes>
        
        
      </div>
    </Router>
    
  
  
  
  )
}

export default App;
