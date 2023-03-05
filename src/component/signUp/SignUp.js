import React, { Component , useState} from "react";
import '../LoginPage&SignUp.css'
import { useNavigate } from "react-router-dom";
import title_icon from '../../image/pacman-icon.png';
import axios from 'axios';
export default function SignUp() {
  const [data, setData] = useState({
    username:"",
     password :"",
     email:"",
  });
  const navigate = useNavigate();
    const handleSubmit = async(e) => {
      
        e.preventDefault();
        try{
          const url = "http://localhost:8080/api/users";
          const { data: res } = await axios.post(url, data);
		      navigate("/sign-in");
			    console.log(res.message);
        } catch (error) {
          console.log("error")
        }
    };

    const handleChange = (e) =>{
      const { name, value } = e.target;
      setData(prevData=>({...prevData , [name]: value}) );
    };


    return (
  <div className="display-wrapper">
     <div className='img'>
        <img src={title_icon} width = "460px" ></img>
      </div> 
    <div className="display-box">
  
      <form onSubmit={handleSubmit}>
        <h3>Sign Up</h3>
        <div className="mb-3">
          <label>Username</label>
          <input
            type="text"
            name = "username"
            className="form-control"
            placeholder="Enter username"
            value={data.username}
            onChange = {handleChange}
          />
        </div>
       
        <div className="mb-3">
          <label>Email address</label>
          <input
            type="email"
            name="email"
            className="form-control"
            placeholder="Enter email"
            value={data.email}
            onChange = {handleChange}
          />
        </div>
        <div className="mb-3">
          <label>Password</label>
          <input
            type="password"
            name ="password"
            className="form-control"
            placeholder="Enter password"
            value={data.password}
            onChange = {handleChange}
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


