import React, { Component } from 'react';
import { MButton } from '../simple/MButton';
import { MInput } from '../simple/MInput';
import './Login.css'

export class Login extends Component {

    constructor() {
        super();
        this.state = {
            login: "",
            password: ""
        };
    }

    updateText(e) {
        console.log(e);
    }

    render() {
        return (
            <div className="main">
                <div className="content">
                    <img src="img/logo.png" className="logo"></img>
                    <h1>Login</h1>
                    <div className="form-container">
                        <MInput text="Login" update={(t) => { this.setState({ login: t }); }} placeholder="Your login"></MInput>
                        <MInput text="Password" password="1" update={(t) => { this.setState({ password: t }); }} placeholder="Top-secret"></MInput>
                    </div>
                    <MButton text="Login" width="200"></MButton>
                </div>
            </div>
        );
    }
};