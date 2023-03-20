import React, { Component } from 'react';
import { MButton } from '../../../simple/MButton';
import './Lobby-Room.css'

export class Lobby_Room extends Component {
    render() {
        return (
            <div className="Lobby_Room">
                <img src="img/token.png"></img>
                <p>{this.props.name}</p>
                <p>{this.props.min + " / " + this.props.max}</p>
                <MButton text="Join" click={this.props.click}></MButton>
            </div>
        );
    }
}