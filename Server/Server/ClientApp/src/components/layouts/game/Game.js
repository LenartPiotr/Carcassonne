import React, { Component } from 'react';
import { useNavigate } from "react-router-dom";

import { Board } from './subparts/Board';
import { Game_panel } from './subparts/Game-panel';
import { GamePersonData } from './subparts/GamePersonData';
import { Pawn } from './subparts/Pawn';

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
                { name: 'Player 1', score: 15, data: { pawnsCount: 10 }, color: '#ff0000' }
            ],
            turn: 'Admin',
            board: [
                { bitmap: 'Bfff1100', x: -1, y: -1, r: 1 },
                { bitmap: 'Bfff0101', x: 0, y: -1, r: 0 },
                { bitmap: 'Bfff1100', x: 1, y: -1, r: 2 },
                { bitmap: 'Bfff1122', x: -1, y: 0, r: 3 },
                { bitmap: 'Bfft0022', x: 0, y: 0, r: 0 },
                { bitmap: 'Bfff1010', x: 1, y: 0, r: 0 },
                { bitmap: 'Bfft2200', x: -1, y: 1, r: 0 },
                { bitmap: 'Bfft2002', x: 0, y: 1, r: 0 },
                { bitmap: 'Bttt1000', x: 1, y: 1, r: 0 },
            ],
            currentImg: ''
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
        if (data.success) this.setState({ playersInfo: data.users });
    }

    placePiece(data) {
        this.setState({ currentImg: '/graphics/bitmap?name=' + data.bitmap, turn: data.playerTurnNick });
        /*fetch('/graphics/bitmap?name=' + data.bitmap)
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
            });*/
    }

    render() {
        return (
            <div className="main-game">
                <Game_panel img={this.state.currentImg} >
                    {this.state.playersInfo.map((p, i) => (
                        <GamePersonData name={p.name} color={p.color} score={p.score} pawns={p.data.pawnsCount} key={i} turn={this.state.turn == p.name} />
                    ))}
                </Game_panel>
                <Board pieces={this.state.board} currentImg={this.state.currentImg} />
            </div>
        );
    }
};