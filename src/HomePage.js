import "./HomePage.css";
import React, { Component, useState } from "react";
import PopupMenu from './PopupMenu';
import title_icon from './image/pacman-icon.png';
export default function HomePage() {

    return(
        <div className='display_wrapper'>
            <div className='img'>
                <img src={title_icon} width = "570px" ></img>
            </div> 
            <div className='display-box'>
            <div className="home-page"><PopupMenu/></div>
            <div className="home-page">
                <button className="home-page-btn">Start New Game</button>
            </div>
            <div className="sound-control">
                <button className="sound-control-btn">Create Map</button>
            </div>
            <div className="about">
                <button className="about-btn">Map Menu</button>
            </div>
            <div className="quit">
                <button className="quit-btn">Online Forum</button>
            </div>
            </div>
        </div>
    );


}