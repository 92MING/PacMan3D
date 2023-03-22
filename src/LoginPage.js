import './LoginPage&SignUp.css' ;
import React, { Component, useState } from "react";
import PopupMenu from './PopupMenu';
import title_icon from './image/pacman-icon.png';
function LoginPage() {
    const [email, setEmail] = useState('');
    const [password,setPassword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email, password) ;
        
    };

    return (
    <div className='display-wrapper'>
      <div className='img'>
        <img src={title_icon} width = "570px" ></img>
      </div> 
      <div className='display-box'>
        <form  onSubmit={handleSubmit}>
            <h3>Login</h3>
            <div className="mb-3">
              <label>Email address</label>
              <input
                type="email"
                className="form-control"
                placeholder="Enter email"
                value={email}
                onChange= {(e)=> setEmail(e.target.value)}
              />
            </div>
            <div className="mb-3">
              <label>Password</label>
              <input
                type="password"
                className="form-control"
                placeholder="Enter password"
                value={password}
                onChange= {(e)=> setPassword(e.target.value)}
              />
            </div>
            <div className="mb-3">
              <div className="custom-control custom-checkbox">
                <input
                  type="checkbox"
                  className="custom-control-input"
                  id="customCheck1"
                />
                <label className="custom-control-label" htmlFor="customCheck1">
                  Remember me
                </label>
              </div>
            </div>
            <div className="d-grid">
              <button type="submit" className="btn btn-primary">
                Login
               
              </button> <a href='/home-page'>home page</a>
            </div>
            <p className='signup-text'>
                New to PacMan3D? <a href='/sign-up'>Create an account</a>
            </p>
            <p className="forgot-password text-right">
              Change <a href='/change-password'>password?</a>
            </p>
          </form>
      </div>
    </div>
            );


    }


export default LoginPage;