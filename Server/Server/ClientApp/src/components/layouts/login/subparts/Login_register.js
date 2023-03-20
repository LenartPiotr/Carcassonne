import React, { Component } from 'react';
import { MButton } from '../../../simple/MButton';
import { MInput } from '../../../simple/MInput';

export class Login_register extends Component {
    constructor() {
        super();
        this.state = {
            email: "",
            login: "",
            password: "",
            password2: "",
        };
    }

    render() {
        return (
            <div>
                <h1>Register</h1>
                <div className="form-container">
                    <MInput text="Email" update={(t) => { this.setState({ email: t }); }} placeholder="user@domain.com"></MInput>
                    <MInput text="Login" update={(t) => { this.setState({ login: t }); }} placeholder="Your login"></MInput>
                    <MInput text="Password" password="1" update={(t) => { this.setState({ password: t }); }} placeholder="Top-secret"></MInput>
                    <MInput text="Confirm password" password="1" update={(t) => { this.setState({ password2: t }); }} placeholder="Yeach again"></MInput>
                </div>
                <MButton text="Register" width="200"></MButton>
                <p onClick={this.props.changeSubpart}>Or click here to login</p>
            </div>
        );
    }
}