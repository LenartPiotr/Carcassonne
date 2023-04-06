import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";
import { Lobby_Room } from './subparts/Lobby-Room';
import { MScrollable } from '../../simple/MScrollable';
import { MLogger } from '../../simple/Logger/MLogger';
import './Lobby.css';
import { MButton } from '../../simple/MButton';

export function LobbyWithRoute(props) {
    const navigate = useNavigate();
    return (<Lobby navigate={navigate} {...props}></Lobby>);
}

export class Lobby extends Component {

    constructor(props) {
        super(props);
        var rooms = [];
        this.state = {
            subpart: 'login',
            rooms: rooms
        };
        this.addMessage = _ => _;
        this.refresh(props);
    }

    changeSubpart() {
        this.setState({
            subpart: this.state.subpart == 'login' ? 'register' : 'login'
        });
    }

    clickJoinRoom(room) {
        this.props.navigate("/room");
    }

    refresh(props) {
        props.fetch(props.navigate, '/lobby/getroomslist', {
        }, (res) => {
            if (!res.Success) { this.addMessage(res.Message); return; }
            this.setState({ rooms: res.Rooms.map((v, i) => { return { key: i, name: v.Name, min: v.Min, max: v.Max } })})
        });
    }

    render() {
        return (
            <div className="main-lobby">
                <MLogger message={(addMessage) => { this.addMessage = addMessage; }} />
                <div className="content">
                    <img src="img/logo.png" className="logo"></img>
                    <MButton text="Refresh" width="120" click={this.refresh.bind(this,this.props)}></MButton>
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