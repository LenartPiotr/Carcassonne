import React, { Component } from 'react';
import './MScrollable.css'

export class MScrollable extends Component {
    render() {
        return (
            <div className="MScrollable">
                { this.props.children }
            </div>
        );
    }
}