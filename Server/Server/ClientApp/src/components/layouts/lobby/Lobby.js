import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";
import { Lobby_Room } from './subparts/Lobby-Room';
import { MScrollable } from '../../simple/MScrollable';
import { MLogger } from '../../simple/Logger/MLogger';
import './Lobby.css';
import { MButton } from '../../simple/MButton';
import { MInput } from '../../simple/MInput';

import ConnectionManager from '../../managers/ConnectionManager';

export function LobbyWithRoute(props) {
    const navigate = useNavigate();
    return (<Lobby navigate={navigate} {...props}></Lobby>);
}

export class Lobby extends Component {

    constructor(props) {
        super(props);
        var rooms = [];
        this.state = {
            rooms: rooms,
            showPanel: false

        };
        this.addMessage = _ => _;
        this.refresh(props);

        this.conn = new ConnectionManager();
        this.conn.afterOpen(this.conn.join.bind(this.conn, props.fetch, props.navigate, () => {
            this.conn.invoke('Username');
        }));
        this.conn.on('Username', (u, k) => console.log(u, k));
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

    createRoom() {
        console.log(this.roomName)
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
                    <MButton text="Create room" width="180" click={() => this.setState({showPanel:true})}></MButton>
                    <div className="panel" style={{ display: this.state.showPanel ? "flex" : "none" }}>
                        <div>
                            <div>Create room</div>
                            <MInput placeholder="Room name" update={t => { this.roomName = t }} />
                            <div className="row">
                                <MButton text="Create" width="120" click={() => { this.setState({ showPanel: false }); this.createRoom(); }} />
                                <MButton text="Cancel" width="120" click={() => this.setState({ showPanel: false })} />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
};