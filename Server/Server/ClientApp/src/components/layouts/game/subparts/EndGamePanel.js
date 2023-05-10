import React, { Component } from 'react';

import { MScrollable } from '../../../simple/MScrollable';
import { MButton } from '../../../simple/MButton';

import './EndGamePanel.css';

export class EndGamePanel extends Component {

    constructor() {
        super();
    }

    render() {
        return (
            <div className="EndGamePanel">
                <div>End Game</div>
                <MScrollable>
                    {this.props.users.map((u, i) => {
                        return (<div className="user" key={i}>
                            <div>{u.nick}</div>
                            <div>{u.score}</div>
                        </div>);
                    })}
                </MScrollable>
                <MButton text="Return" click={() => { this.props.navigate("/room"); }} />
            </div>
        );
    }
};