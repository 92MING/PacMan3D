import React from 'react';
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import LoginPage from './LoginPage';
import SignUp from './SignUp';
function App() {
  return (
    <Router>
      <div className="App">
        
        <div className="display-wrapper">
          <div className="display-box">
            <Routes>
              <Route exact path="/" element={<LoginPage />} />
              <Route path="/sign-in" element={<LoginPage />} />
              <Route path="/sign-up" element={<SignUp />} />
            </Routes>
          </div>
        </div>
      </div>
    </Router>
    
  
  
  
  )
}

export default App;
