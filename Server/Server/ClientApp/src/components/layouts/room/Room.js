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
            persons: [
                { name: 'Player 1' },
                { name: 'MrStrategy' },
                { name: 'xxx' },
                {}
            ],
            current: 'Player 1'
        }
        this.conn = new ConnectionManager();
    }

    componentDidMount() {
        this.conn.on("UpdateUsers", this.onUpdateUsers);
    }

    componentWillUnmount() {
        this.conn.off("UpdateUsers", this.onUpdateUsers);
    }

    onUpdateUsers(tab) {
        console.log(tab);
    }

    kickSomeone(who) {
        // CHANGE
        if (who.name == undefined) return;
        if (who.name == this.state.current) {
            this.props.navigate('/lobby');
            return;
        }
        var persons = this.state.persons;
        persons.splice(persons.indexOf(who), 1);
        persons.push({});
        this.setState({ persons: persons });
    }

    play() {
        this.props.navigate('/game');
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
                        {this.state.persons.map((p,i) => (
                            <Room_person
                                key={ i }
                                name={ p.name ? p.name : 'Empty' }
                                button={ p.name ? (this.state.current == p.name ? 'Leave' : 'Kick') : false }
                                click={ this.kickSomeone.bind(this, p) }
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