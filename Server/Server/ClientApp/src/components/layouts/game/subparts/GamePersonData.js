import React, { Component } from 'react';

import { Pawn } from './Pawn';

import './GamePersonData.css';

export class GamePersonData extends Component {
    constructor(props) {
        super(props);
        var pawnsTable = [];
        for (let i = 0; i < props.pawns; i++) {
            pawnsTable.push(1);
        }
        this.state = {
            pawnsTable: pawnsTable
        };
    }
    render() {
        return (
            <div className="GamePersonData">
                <div className={"turn" + (this.props.turn ? " active" : "")}></div>
                <div className="data">
                    <div>
                        <div style={{backgroundColor: this.props.color}}></div>
                        {this.props.name}
                    </div>
                    <div>{this.state.pawnsTable.map((_, i) => (<Pawn color={this.props.color} key={i} size={20} />))}</div>
                </div>
                <div className="score">{this.props.score}</div>
            </div>
        );
    }
}