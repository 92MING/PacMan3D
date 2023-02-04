import React, { Component, useState } from "react";
import Popup from 'reactjs-popup';
import 'reactjs-popup/dist/index.css';
import './PopupMenu.css';

export default function PopupMenu(){
    
    
    return(
        <Popup trigger={<button>Trigger</button>} modal nested>
            <div className="home-page">
                <button> Home age</button>

            </div>
            <div className="sound-control">

            </div>
            <div>

            </div>
        </Popup>


    );



};

