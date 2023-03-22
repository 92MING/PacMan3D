import React from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import LoginPage from './LoginPage';
import SignUp from './SignUp';
import PopupMenu from './PopupMenu';
import HomePage from './HomePage';
import ChangePassword from './ChangePassword';

function App() {
  return (
    <Router>
      <div className="App">       
            <Routes>
              <Route exact path="/" element={<LoginPage />} />
              <Route path="/sign-in" element={<LoginPage />} />
              <Route path="/sign-up" element={<SignUp />} />
              <Route path="/popup-menu" element={<PopupMenu/>} />
              <Route path="/home-page" element={<HomePage/>} />
              <Route path="/change-password" element={<ChangePassword/>}/>
            </Routes>      
      </div>
    </Router>
    
  
  
  
  )
}

export default App;