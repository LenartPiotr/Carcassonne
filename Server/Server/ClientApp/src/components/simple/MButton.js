import React, { Component } from 'react';
import './MButton.css'

export class MButton extends Component {
    constructor(props) {
        super();
        this.width = props.width ? Number(props.width) : 150;
        this.height = this.width / 2;
    }
    render() {
        return (
            <div className="MButton" style={{ width: this.width, height: this.height, minHeight: this.height }} onClick={ this.props.click }>
                <div>{this.props.text}</div>
            </div>
        );
    }
};