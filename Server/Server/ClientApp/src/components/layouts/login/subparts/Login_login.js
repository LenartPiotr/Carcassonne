import React, { Component } from 'react';
import { MButton } from '../../../simple/MButton';
import { MInput } from '../../../simple/MInput';
import { useNavigate } from "react-router-dom";

export function Login_loginWithRoute(props) {
    const navigate = useNavigate();
    return (<Login_login navigate={navigate} {...props}></Login_login>);
}

export class Login_login extends Component {
    constructor() {
        super();
        this.state = {
            login: "",
            password: ""
        };
    }

    render() {
        return (
            <div>
                <h1>Login</h1>
                <div className="form-container">
                    <MInput text="Login" update={(t) => { this.setState({ login: t }); }} placeholder="Your login"></MInput>
                    <MInput text="Password" password="1" update={(t) => { this.setState({ password: t }); }} placeholder="Top-secret"></MInput>
                </div>
                <MButton text="Login" width="200" click={() => { this.props.navigate("/lobby"); }}></MButton>
                <p onClick={ this.props.changeSubpart }>Or click here to register</p>
            </div>
        );
    }
}