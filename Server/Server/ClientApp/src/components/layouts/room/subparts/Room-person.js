import React, { Component } from 'react';
import { MButton } from '../../../simple/MButton';
import './Room-person.css';

export class Room_person extends Component {

    constructor() {
        super();
    }

    render() {
        return (
            <div className="Room_person">
                <img src="img/person.png"></img>
                <div>{this.props.name}</div>
                {this.props.button ? (<MButton text={this.props.button} click={this.props.click}></MButton>) : ''}
            </div>
        );
    }
};