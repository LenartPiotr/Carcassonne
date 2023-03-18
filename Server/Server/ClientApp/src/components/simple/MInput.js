import React, { Component } from 'react';
import './MInput.css'

export class MInput extends Component {

    render() {
        return (
            <div className="MInput">
                <div>{this.props.text}</div>
                <input
                    type={this.props.password != "1" ? "text" : "password"}
                    onInput={(e) => { this.props.update(e.target.value); }}
                    placeholder={this.props.placeholder}
                ></input>
            </div>
        );
    }
};