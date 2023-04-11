import React, { Component , useState} from "react";
import '../LoginPage&SignUp.css' ;
import title_icon from '../../image/pacman-icon.png';
import { Helmet } from 'react-helmet';
import axios from "axios";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export default function ResetPassword() {
    const [email, setEmail] = useState('');
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const navigate = useNavigate();
    const handleSubmit = async (e) => {
      e.preventDefault();
      const url = 'http://localhost:3000/api/user/reset'
      await axios.post(url, {
                          email: email,
                          oldPassword: oldPassword,
                          newPassword: newPassword
                        })
      .then(res => { 
        if (res.data.isReset) {
          toast.success("Password Reset Successful!", {
            position: "top-center",
            autoClose: 500, // Time to close the alert in milliseconds
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
          });
          navigate('/sign-in');
        }
        else{
          toast.error("Password does not match!", {
            position: "top-center",
            autoClose: 1000, // Time to close the alert in milliseconds
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
          });
        }
      })
      .catch((e) => {
        console.log(e);
      })
    };

    return (
          <div className="display-wrapper">
              <Helmet>
                <title>Pac-Man Reset Password</title>
              </Helmet>

              <div className='img'>
                  <img src={title_icon} width = "40%" ></img>
                </div> 
              <div className="display-box-login">
          
              <form onSubmit={handleSubmit}>
                <h3>Reset Password</h3>
              
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
                    value={oldPassword}
                    onChange = {(e)=> setOldPassword(e.target.value)}
                  />
                </div>
                <div className="mb-3">
                  <label>New Password</label>
                  <input
                    type="password"
                    className="form-control"
                    placeholder="Enter password"
                    value={newPassword}
                    onChange = {(e)=> setNewPassword(e.target.value)}
                  />
                </div>
                <div className="d-grid">
                  <button type="submit" className="btn btn-primary">
                    Reset Password
                  </button>
                </div>
                <p className="forgot-password-text-right">
                  Return to <a href="/sign-in">Login?</a>
                </p>
              </form>
            </div>
          </div>
        );
    }


