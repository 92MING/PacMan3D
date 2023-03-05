import '../LoginPage&SignUp.css' ;
import React, { Component, useState } from "react";
import PopupMenu from '../PopUpMenu/PopupMenu';
import title_icon from '../../image/pacman-icon.png';
import axios from 'axios';
function LoginPage() {
    const [data, setData] = useState(
      {username: "",
       password : "",
       email: "",
    })

    const handleSubmit = async (e) => {
      e.preventDefault();
      try {
        const url = "http://localhost:8080/api/auth";
        const { data: res } = await axios.post(url, data);
        localStorage.setItem("token", res.data);
        window.location = "/";
      } catch (error) {
       
      }
    };
    const handleChange = (e) =>{
      const { name, value } = e.target;
      setData(prevData=>({...prevData , [name]: value}) );
    };

    return (
    <div className='display-wrapper'>
      <div className='img'>
        <img src={title_icon} width = "460px" ></img>
      </div> 
      <div className='display-box'>
        <form  onSubmit={handleSubmit}>
            <PopupMenu/>
            <h3>Login</h3>
            <div className="mb-3">
              <label>Email address</label>
              <input
                type="email"
                name="email"
                className="form-control"
                placeholder="Enter email"
                value={data.email}
                  onChange= {handleChange}
              />
            </div>
            <div className="mb-3">
              <label>Password</label>
              <input
                type="password"
                name="password"
                className="form-control"
                placeholder="Enter password"
                value={data.password}
                onChange= {handleChange}
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
               
              </button> <a href='/'>home page</a>
            </div>
            <p className='signup-text'>
                New to PacMan3D? <a href='/sign-up'>Create an account</a>
            </p>
            <p className="forgot-password text-right">
              Forgot <a href="#">password?</a>
            </p>
          </form>
      </div>
    </div>
            );


    }


export default LoginPage;