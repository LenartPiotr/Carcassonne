import React, { Component } from 'react';

import { MScrollable } from '../../../simple/MScrollable';

import './Game-panel.css';

export class Game_panel extends Component {

    constructor() {
        super();
    }

    render() {
        return (
            <div className="Game-panel">
                <MScrollable>
                    { this.props.children }
                </MScrollable>
                <div>Current piece:</div>
                <img src={this.props.img ?? ""} alt="" style={{ width: "90%", height: "auto" }} />
            </div>
        );
    }
};