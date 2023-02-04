import './LoginPage.css' ;
import React, { Component, useState } from "react";
function LoginPage() {
    const [email, setEmail] = useState('');
    const [password,setPassword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email, password) ;
        
    };


    return (
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
              </button>
            </div>
            <p className='signup-text'>
                New to PacMan3D? <a href='/sign-up'>Create an account</a>
            </p>
            <p className="forgot-password text-right">
              Forgot <a href="#">password?</a>
            </p>
          </form>
            );


    }


export default LoginPage;