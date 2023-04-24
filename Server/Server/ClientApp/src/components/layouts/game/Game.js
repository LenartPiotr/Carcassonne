import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";

import { Board } from './subparts/Board';
import { Game_panel } from './subparts/Game-panel';

import ConnectionManager from '../../managers/ConnectionManager';

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
        this.conn = new ConnectionManager();
        this.bindings = {
            "GetPlayersData": this.getPlayersData.bind(this),
            "PlacePiece": this.placePiece.bind(this)
        };
    }

    componentDidMount() {
        for (let action in this.bindings) {
            this.conn.on(action, this.bindings[action]);
        }
        this.conn.afterOpen(() => {
            this.conn.invoke("GameAction", "GetPlayersData", []);
            this.conn.invoke("GameAction", "GetGameStage", []);
        });
    }

    componentWillUnmount() {
        for (let action in this.bindings) {
            this.conn.off(action, this.bindings[action]);
        }
    }

    getPlayersData(data) {
        console.log(data);
    }

    placePiece(data) {
        console.log(data);
        fetch('/graphics/bitmap?name=' + data.bitmap)
            .then(response => {
                if (response.ok) {
                    return response.blob();
                }
                throw new Error('Network response was not ok.');
            })
            .then(blob => {
                const url = URL.createObjectURL(blob);
                console.log(url);
                this.setState({ currentImg: url });
            })
            .catch(error => {
                console.error('There has been a problem with your fetch operation:', error);
            });
    }

    render() {
        return (
            <div className="main-game">
                <Game_panel img={ this.state.currentImg } />
                <Board />
            </div>
        );
    }
};