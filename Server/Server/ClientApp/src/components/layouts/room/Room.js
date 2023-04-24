import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";

import { MButton } from '../../simple/MButton';
import { MScrollable } from '../../simple/MScrollable';
import { Room_person } from './subparts/Room-person';

import ConnectionManager from '../../managers/ConnectionManager';

import './Room.css';

export function RoomWithRoute(props) {
    const navigate = useNavigate();
    return (<Room navigate={navigate} {...props}></Room>);
}

export class Room extends Component {

    constructor() {
        super();
        this.state = {
            people: [], // name, isAdmin, color
            current: ''
        }
        this.conn = new ConnectionManager();
    }

    componentDidMount() {
        this.conn.on("UpdateUsers", this.onUpdateUsers.bind(this));
        this.conn.on("GetNick", this.onGetNick.bind(this));
        this.conn.afterOpen(() => {
            this.conn.invoke("GetNick");
            this.conn.invoke("RoomAction", "GetUsers", []);
        })
    }

    componentWillUnmount() {
        this.conn.off("UpdateUsers", this.onUpdateUsers.bind(this));
        this.conn.off("GetNick", this.onGetNick.bind(this));
    }

    onGetNick(myNick) {
        this.setState({ current: myNick });
    }

    onUpdateUsers(tab, maxUsers) {
        while (tab.length < maxUsers) tab.push({});
        this.setState({ people: tab });
    }

    play() {
        this.conn.invoke("RoomAction", "StartGame", []);
    }

    imAdmin() {
        for (let i in this.state.people) {
            if (this.state.people[i].name == this.state.current) return this.state.people[i].isAdmin;
        }
        return false;
    }

    leaveRoom() {
        this.conn.invoke("LeaveRoom");
    }

    render() {
        return (
            <div className="main-room">
                <div className="content">
                    <header>
                        <img src="img/token.png" className="logo"></img>
                        <div>Room 2</div>
                    </header>
                    <MScrollable>
                        {this.state.people.map((p,i) => (
                            <Room_person
                                key={ i }
                                name={ p.name ? p.name : 'Empty' }
                                button={(p.name != undefined) ? (this.state.current == p.name ? 'Leave' : false) : false }
                                click={this.leaveRoom.bind(this) }
                            ></Room_person>
                        )) }
                    </MScrollable>
                </div>
                <div className="settings">
                    <h1>Settings</h1>
                    <MScrollable>
                    </MScrollable>
                    <MButton text="Play" click={ this.play.bind(this) } />
                </div>
            </div>
        );
    }
};