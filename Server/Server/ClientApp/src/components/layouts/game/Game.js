import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";

import { Board } from './subparts/Board';
import { Game_panel } from './subparts/Game-panel';

import './Game.css';

export function GameWithRoute(props) {
    const navigate = useNavigate();
    return (<Game navigate={navigate} {...props}></Game>);
}

export class Game extends Component {

    constructor() {
        super();
        this.state = {
            playersInfo: [
                { name: 'Player 1', score: 15, equipment: ['p', 'p', 'p', 'p'], color: 'red' },
                { name: 'Player 1', score: 12, equipment: ['p', 'p', 'p'], color: 'yellow' },
                { name: 'Player 1', score: 32, equipment: ['p', 'p', 'p', 'p', 'p'], color: 'green' },
                { name: 'Player 1', score: 9, equipment: ['p'], color: 'blue' }
            ]
        };
    }

    render() {
        return (
            <div className="main-game">
                <Game_panel />
                <Board />
            </div>
        );
    }
};