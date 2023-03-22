import React, { Component , useState} from "react";
import './LoginPage&SignUp.css' ;
import title_icon from './image/pacman-icon.png';
import { Helmet } from 'react-helmet';
export default function ChangePassword() {
    const [email, setEmail] = useState('');
    const [password,setPassword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email, password) ;
    };

    return (
  <div className="display-wrapper">
    <Helmet>
        <title>Pac-Man Change Password</title>
      </Helmet>
     <div className='img'>
        <img src={title_icon} width = "60%" ></img>
      </div> 
    <div className="display-box">
  
      <form onSubmit={handleSubmit}>
        <h3>Change Password</h3>
       
        <div className="mb-3">
          <label>Email address</label>
          <input
            type="email"
            className="form-control"
            placeholder="Enter email"
            value={email}
            onChange = {(e)=> setEmail(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label>Old Password</label>
          <input
            type="password"
            className="form-control"
            placeholder="Enter password"
            value={password}
            onChange = {(e)=> setPassword(e.target.value)}
          />
         </div>
         <div className="mb-3">
          <label>New Password</label>
          <input
            type="password"
            className="form-control"
            placeholder="Enter password"
            value={password}
            onChange = {(e)=> setPassword(e.target.value)}
          />
        </div>
        <div className="d-grid">
          <button type="submit" className="btn btn-primary">
            Confirm Change
          </button>
        </div>
        <p className="forgot-password text-right">
          Return to <a href="/sign-in">Login?</a>
        </p>
      </form>
    </div>
  </div>
        );
    }


