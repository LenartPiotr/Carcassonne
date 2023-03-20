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
                </MScrollable>
            </div>
        );
    }
};