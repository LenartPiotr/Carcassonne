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
            action: '',
            myName: '-',
            board: [],
            currentImg: '',
        };
        this.rawBitmapData = '';
        this.conn = new ConnectionManager();
        this.bindings = {
            "GetPlayersData": this.getPlayersData.bind(this),
            "PlacePiece": this.placePiece.bind(this),
            "GetNick": this.getNick.bind(this),
            "PlacePuzzle": this.placePuzzle.bind(this),
        };
    }

    componentDidMount() {
        for (let action in this.bindings) {
            this.conn.on(action, this.bindings[action]);
        }
        this.conn.afterOpen(() => {
            this.conn.invoke("GameAction", "GetPlayersData", []);
            this.conn.invoke("GameAction", "GetGameStage", []);
            this.conn.invoke("GetNick");
        });
    }

    componentWillUnmount() {
        for (let action in this.bindings) {
            this.conn.off(action, this.bindings[action]);
        }
    }

    clickOnPiece(x, y, rot) {
        if (this.state.action == 'placePiece' && this.state.turn == this.state.myName) {
            this.conn.invoke("GameAction", "PlacePuzzle", [this.rawBitmapData, x, y, rot]);
        }
    }

    // Responces

    getPlayersData(data) {
        if (data.success) this.setState({ playersInfo: data.users });
    }

    placePiece(data) {
        this.setState({ currentImg: '/graphics/bitmap?name=' + data.bitmap, turn: data.playerTurnNick, action: 'placePiece' });
        this.rawBitmapData = data.bitmap;
    }

    getNick(nick) {
        this.setState({ myName: nick });
    }

    placePuzzle(data) {
        console.log("place", data.bitmapData, data.x, data.y);
        if (!data.success) return;
        var board = this.state.board;
        board.push({ bitmap: data.bitmapData, x: data.x, y: data.y, r: 0 });
        this.setState({ board: board });
    }

    render() {
        return (
            <div className="main-game">
                <Game_panel img={this.state.currentImg} >
                    {this.state.playersInfo.map((p, i) => (
                        <GamePersonData name={p.name} color={p.color} score={p.score} pawns={p.data.pawnsCount} key={i} turn={this.state.turn == p.name} />
                    ))}
                </Game_panel>
                <Board
                    pieces={this.state.board}
                    currentImg={this.state.currentImg}
                    click={this.clickOnPiece.bind(this)}
                    ghost={this.state.action == 'placePiece' && this.state.turn == this.state.myName} />
            </div>
        );
    }
};