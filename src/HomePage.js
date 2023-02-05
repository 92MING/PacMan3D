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
            <div className="start-game">
                <button className="start-game-btn">Start New Game</button>
            </div>
        </div>
    );


}