import React, { Component } from 'react';

export class MLogger_message extends Component {

    constructor() {
        super();
        this.hide = false;
        setTimeout(() => {
            this.hideElement();
        }, 10000)
    }

    hideElement() {
        this.props.removeMessage();
    }

    render() {
        return (
            <div className="MLogger-message" onClick={this.hideElement.bind(this)}>
                { this.props.message.text }
            </div>
        );
    }
};