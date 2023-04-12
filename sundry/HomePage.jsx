import "./HomePage.css";
import React, { Component, useState } from "react";
import PopupMenu from '../PopUpMenu/PopupMenu';
import title_icon from '../../image/pacman-icon.png';
import { Helmet } from 'react-helmet';

export default function HomePage() {
    const handleLogout = () => {
		localStorage.removeItem("token");
		window.location.reload();
	};
    return(
        <div className='display_wrapper'>
            <Helmet>
                <title>Pac-Man Home Page</title>
            </Helmet>
            <div className='img'>
                <img src={title_icon} width = "40%" ></img>
            </div>
            <div className='display-box'>
                <div ><PopupMenu/></div>
                <div className="start-game">
                    <button className="start-game-btn">Start New Game</button>
                </div>
                <div className="create-map">
                    <button className="create-map-btn">Create Map</button>
                </div>
                <div className="map-menu">
                    <button className="map-menu-btn">Map Menu</button>
                </div>
                <div className="quit">
                    <button className="quit-btn">Online Forum</button>
                </div>
            </div>
        </div>
    );


}