import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";
import { Lobby_Room } from './subparts/Lobby-Room';
import { MScrollable } from '../../simple/MScrollable';
import './Lobby.css';
import { MButton } from '../../simple/MButton';

export function LobbyWithRoute(props) {
    const navigate = useNavigate();
    return (<Lobby navigate={navigate} {...props}></Lobby>);
}

export class Lobby extends Component {

    constructor() {
        super();
        var rooms = [];
        for (let i = 1; i < 5; i++) {
            rooms.push({ name: 'Lobby ' + i, min: 1, max: 4 + i, key: i });
        }
        this.state = {
            subpart: 'login',
            rooms: rooms
        };
        this.refresh();
    }

    changeSubpart() {
        this.setState({
            subpart: this.state.subpart == 'login' ? 'register' : 'login'
        });
    }

    clickJoinRoom(room) {
        this.props.navigate("/room");
    }

    refresh() {
        //
    }

    render() {
        return (
            <div className="main-lobby">
                <div className="content">
                    <img src="img/logo.png" className="logo"></img>
                    <MScrollable>
                        {
                            this.state.rooms.map(e => (
                                <Lobby_Room {...e} click={this.clickJoinRoom.bind(this, e)}></Lobby_Room>
                            ))
                        }
                    </MScrollable>
                    <MButton text="Create room" width="180"></MButton>
                </div>
            </div>
        );
    }
};