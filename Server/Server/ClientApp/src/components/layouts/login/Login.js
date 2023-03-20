import React, { Component } from 'react';
import { Login_login, Login_loginWithRoute } from './subparts/Login_login';
import { Login_register } from './subparts/Login_register';
import './Login.css'

export class Login extends Component {

    constructor() {
        super();
        this.state = {
            subpart: 'login'
        };
    }

    changeSubpart() {
        this.setState({
            subpart: this.state.subpart == 'login' ? 'register' : 'login'
        });
    }

    render() {
        return (
            <div className="main">
                <div className="content">
                    <img src="img/logo.png" className="logo"></img>
                    {
                        this.state.subpart == 'login' ?
                            (<Login_loginWithRoute changeSubpart={this.changeSubpart.bind(this)}></Login_loginWithRoute>) :
                            (<Login_register changeSubpart={this.changeSubpart.bind(this)}></Login_register>)
                    }
                </div>
            </div>
        );
    }
};