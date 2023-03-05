import "./HomePage.css";
import React, { Component, useState } from "react";
import PopupMenu from '../PopUpMenu/PopupMenu';
import pacman_icon from '../../image/pacman-icon.png';
export default function HomePage() {
    const handleLogout = () => {
		localStorage.removeItem("token");
		window.location.reload();
	};
    return(
        <div className='display_wrapper'>
            <div className='img'>
                <img src={pacman_icon} width = "570px" ></img>
            </div> 
            <div className="start-game">
                <button className="start-game-btn">Start New Game</button>
                <button  onClick={handleLogout}>
					Logout
				</button>
            </div>

        </div>
    );


}