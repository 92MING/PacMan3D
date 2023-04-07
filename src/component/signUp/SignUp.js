import React, { Component , useState, useEffect,useContext} from "react";
import '../LoginPage&SignUp.css'
import { useNavigate } from "react-router-dom";
import title_icon from '../../image/pacman-icon.png';
import { Helmet } from 'react-helmet';
import axios from 'axios';
import{ toast} from 'react-toastify';
import { AuthContext } from "../AuthContext";

export default function SignUp() {
  const [username, setUsername]= useState('');
  const [email, setEmail]= useState('');
  const [password, setPassword]= useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [passwordsMatch, setPasswordsMatch] = useState(true);
  const navigate = useNavigate();
  const { user,setUser } = useContext(AuthContext);

  useEffect(() => {
        setPasswordsMatch(confirmPassword === password);
      }, [confirmPassword, password]);
  
  const url = 'http://localhost:8080/register'; 
  const registerUser = async (e) => {
    e.preventDefault();
    if (!passwordsMatch) {
      toast.warn('Password does not match!', {
        position: "top-center",
        autoClose: 1000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
        progress: undefined,
        theme: "light",
        });
      return;
    }
    try {
      const res = await axios.post(url, {
        username,
        email,
        password,
      });
      if (res.data === "exist") {
        toast.warn('User has been registered!', {
          position: "top-center",
          autoClose: 1000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "light",
          });

      } else {
        toast.success('User created!', {
          position: "top-center",
          autoClose: 1000,
          hideProgressBar: false,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: true,
          progress: undefined,
          theme: "light",
          });
          
        setUser(username);
        localStorage.setItem("username", username);
        navigate("/home-page");
      }
    } catch (e) {
      console.error(e);
    }
  };
          
  const cantReg = ()=>{};

    return (
  <div className="display-wrapper">
      <Helmet>
        <title>Pac-Man Sign Up</title>
      </Helmet>
      <div className='img'>
        <img src={title_icon} width = "30%" ></img>
      </div> 
    
    <div className="display-box-signup">
  
      <form onSubmit={passwordsMatch ?registerUser : cantReg}>
        <h3>Sign Up</h3>
        <div className="mb-3">
          <label>Username</label>
          <input
            type="text"
            name = "username"
            className="form-control"
            placeholder="Enter username"
            value={username}
            onChange = {e => setUsername(e.target.value)}
          />
        </div>
       
        <div className="mb-3">
          <label>Email address</label>
          <input
            type="email"
            name="email"
            className="form-control"
            placeholder="Enter email"
            value={email}
            onChange = {e => setEmail(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label>Password</label>
          <input
            type="password"
            name ="password"
            className="form-control"
            placeholder="Enter password"
            value={password}
            onChange = {e => setPassword(e.target.value)}
          />
        </div>
        <div className="mb-3">
          <label>Confirm Password</label>
          <input
            type="password"
            name ="password"
            className={passwordsMatch ?"form-control" : 'error'}
            placeholder="Enter password"
            value={confirmPassword}
            onChange = {e => {setConfirmPassword(e.target.value)}}
          />
        </div>
        <div className="signup-grid">
          <button type="submit" className="btn btn-primary">
            Sign Up
          </button>
        </div>
        <p className="forgot-password-text-right">
          Already registered <a href="/sign-in">Login?</a>
        </p>
      </form>
    </div>
  </div>
        );
    }


