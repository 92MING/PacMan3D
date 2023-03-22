import React, { Component , useState} from "react";
import './LoginPage&SignUp.css' ;
import title_icon from './image/pacman-icon.png';
export default function SignUp() {
    const [email, setEmail] = useState('');
    const [password,setPassword] = useState('');
    const [username, setUsername] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email, password) ;
    
    };

    return (
  <div className="display-wrapper">
     <div className='img'>
        <img src={title_icon} width = "60%" ></img>
      </div> 
    <div className="display-box">
  
      <form onSubmit={handleSubmit}>
        <h3>Sign Up</h3>
        <div className="mb-3">
          <label>Username</label>
          <input
            type="text"
            className="form-control"
            placeholder="Enter username"
            value={username}
            onChange = {(e)=> setUsername(e.target.value)}
          />
        </div>
       
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
          <label>Password</label>
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
            Sign Up
          </button>
        </div>
        <p className="forgot-password text-right">
          Already registered <a href="/sign-in">Login?</a>
        </p>
      </form>
    </div>
  </div>
        );
    }


