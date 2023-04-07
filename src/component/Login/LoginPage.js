import '../LoginPage&SignUp.css' ;
import React, { Component, useState, useEffect,useContext } from "react";
import PopupMenu from '../PopUpMenu/PopupMenu';
import title_icon from '../../image/pacman-icon.png';
import axios from 'axios';
import { Helmet } from 'react-helmet';
import { useNavigate } from "react-router-dom";
import { AuthContext } from "../AuthContext";
import { toast } from "react-toastify";

export default function LoginPage() {
  const { user,setUser } = useContext(AuthContext);
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [rememberMe, setRememberMe] = useState(false);

  useEffect(() => {
    // Check if user is already logged in
    if (user != null) {
      navigate("/home-page");
    }else{
        // Check if remember me is enabled and load email and password from localStorage
        const rememberMe = localStorage.getItem('rememberMe') === 'true';
        setRememberMe(rememberMe);
        if (rememberMe) {
          const email = localStorage.getItem('email');
          const password = localStorage.getItem('password');
          if (email && password) {
            setEmail(email);
            setPassword(password);
          }
        }
    }

  }, []);

  const Login = async (e) => {
    e.preventDefault();
    const url = "http://localhost:8080/login";
    console.log('Email:', email);
    console.log('Password:', password);
    try {
      await axios.post(url, { email, password })
      .then(res => { 
        if (res.data.message === "Login successful") {
          setUser(res.data.username);
          console.log("username is ",res.data.username)
          localStorage.setItem("username", res.data.username);
          
          if (rememberMe) {
            localStorage.setItem('email', email);
            localStorage.setItem('password', password);
            localStorage.setItem('rememberMe', true);
          } else {
            localStorage.removeItem('email');
            localStorage.removeItem('password');
            localStorage.removeItem('rememberMe');
          }
          toast.success("Login successful!", {
            position: "top-center",
            autoClose: 1000, // Time to close the alert in milliseconds
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
          });
          navigate("/home-page");
        } else {
          toast.error('Invaild Email or Password!', {
            position: "top-center",
            autoClose: 1000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: "light",
            });
        }

      })
      .catch((e) => {
        toast.error('Invaild Email or Password!', {
          position: "top-center",
          autoClose: 1000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "light",
          });
        console.log(e);
      })
    } catch(e) {
      console.log(e)
    }
  };

  const handleRememberMeChange = (event) => {
    setRememberMe(event.target.checked);
  };
  
    

  return (
      <div className='display-wrapper'>
      <Helmet>
        <title>Pac-Man Login</title>
      </Helmet>
      <div className='img'>
        <img src={title_icon} width = "40%" ></img>
      </div> 
      <div className='display-box-login'>
        <form className='form' onSubmit={Login}>
            <h3>Login</h3>
            <div className="mb-3">
              <label >Email address</label>
              <input
                type="email"
                name="email"
                className="form-control"
                placeholder="Enter email"
                value={email}
                onChange= {e=>{setEmail(e.target.value)}}
              />
            </div>
            <div className="mb-3">
            <label style={{ display: 'flex', justifyContent: 'start' ,alignItems: 'center'}}>
                Password
                <a href='/send-recovery-email' className='change-password' > Forgot? </a>
                <a href='/reset-password' className='change-password' > Reset? </a>
            </label>
              
              <input
                type="password"
                name="password"
                className="form-control"
                placeholder="Enter password"
                value={password}
                onChange= {e=>{setPassword(e.target.value)}}
              />
            </div>
            <div className="mb-3" >
              <div style={{ display: 'flex', justifyContent: 'start' ,alignItems: 'center'}}className="custom-control custom-checkbox">
                <input
                  type="checkbox"
                  className="custom-control-input"
                  id="customCheck1"
                  checked={rememberMe}
                  onChange={handleRememberMeChange}
                />
                <label style={{marginLeft : '0.2em'}}className="custom-control-label" htmlFor="customCheck1">
                    Remember me
                </label>
              </div>
            </div>
            <div className="login-grid">
              <button type="submit" className="btn btn-primary">
                Login
              </button> 
              <p className='signup-text'>
                  New to Pac-Man? <a href='/sign-up'>Create an account</a>
              </p>
              {/* <p className="change-password">
                Forgot <a href='/change-password'>password?</a>
              </p> */}
            </div>
          </form>
      </div>
    </div>
            );


    }


