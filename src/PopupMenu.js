import React, { Component, useState } from "react";
import { useNavigate } from "react-router-dom";
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import './PopupMenu.css';

export default function PopupMenu(){
    const navigate = useNavigate();
    
    return(
        <Popup trigger={<button className="home-page-btn">Trigger</button>} modal nested>
            
            <div className="home-page">
                <button className="home-page-btn">Home page</button>
            </div>
            <div className="sound-control">
                <button className="sound-control-btn">Sound</button>
            </div>
            <div className="about">
                <button className="about-btn">About</button>
            </div>
            <div className="quit">
                <button className="quit-btn">Quit</button>
            </div>
            <div className="goback">
                <button className="goback-btn" onClick={() => { navigate(-1);}}>Go Back</button>
            </div>
        </Popup>


    );



};

