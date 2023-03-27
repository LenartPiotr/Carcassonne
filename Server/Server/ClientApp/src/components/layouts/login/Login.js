import React, { Component } from 'react';
import { Login_login, Login_loginWithRoute } from './subparts/Login_login';
import { Login_register } from './subparts/Login_register';
import { MLogger } from '../../simple/Logger/MLogger';
import './Login.css'

export class Login extends Component {

    constructor() {
        super();
        this.state = {
            subpart: 'login'
        };
        this.addMessage = _ => _;
    }

    changeSubpart() {
        this.setState({
            subpart: this.state.subpart == 'login' ? 'register' : 'login'
        });
    }

    log(text) {
        this.addMessage(text);
    }

    render() {
        return (
            <div className="main">
                <MLogger message={(addMessage) => { this.addMessage = addMessage; }}/>
                <div className="content">
                    <img src="img/logo.png" className="logo"></img>
                    {
                        this.state.subpart == 'login' ?
                            (<Login_loginWithRoute changeSubpart={this.changeSubpart.bind(this)} addMessage={this.log.bind(this)}></Login_loginWithRoute>) :
                            (<Login_register changeSubpart={this.changeSubpart.bind(this)} addMessage={this.log.bind(this)}></Login_register>)
                    }
                </div>
            </div>
        );
    }
};