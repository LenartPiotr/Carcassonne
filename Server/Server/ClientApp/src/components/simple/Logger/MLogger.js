import React, { Component } from 'react';
import { v4 as uuidv4 } from 'uuid';
import { MLogger_message } from './MLogger-message';
import './MLogger.css'

export class MLogger extends Component {

    constructor(props) {
        super();
        props.message(this.addMessage.bind(this));
        this.state = {
            messages: []
        };
    }

    addMessage(text) {
        var messages = this.state.messages;
        messages.push({ text: text, uuid: uuidv4() });
        this.setState({ messages: messages });
    }

    removeMessage(message) {
        var messages = this.state.messages;
        var index = messages.indexOf(message);
        if (index !== -1) {
            messages.splice(index, 1);
            this.setState({ messages: messages });
        }
    }

    render() {
        return (
            <div className="MLogger">
                {
                    this.state.messages.map((v, i) => (
                        <MLogger_message key={i} message={v} removeMessage={ this.removeMessage.bind(this, v) } />
                    ))
                }
            </div>
        );
    }
};