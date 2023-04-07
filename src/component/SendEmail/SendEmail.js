import React, { Component , useState} from "react";
import '../LoginPage&SignUp.css' ;
import title_icon from '../../image/pacman-icon.png';
import { Helmet } from 'react-helmet';
import axios from "axios";
import { useNavigate } from "react-router-dom";

export default function SendEmail() {
    const [email, setEmail] = useState('');
    const navigate = useNavigate();
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
          await axios.post("http://localhost:8080/password-recovery-email", { email });
          alert("Email sent");
          navigate('/sign-in');
        } catch (e) {
          alert("Failed to send email");
          console.log(e);
        }
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
                <h3 className="requestresetpassword">Recover Password</h3>
              
                <div className="sendemailFrom">
                  <label>Email address</label>
                  <input
                    type="email"
                    className="form-control"
                    placeholder="Enter email"
                    value={email}
                    onChange = {(e)=> setEmail(e.target.value)}
                  />
                </div>
                
                <div className="send-btn">
                  <button type="submit" className="btn btn-primary">
                    Send Email
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


