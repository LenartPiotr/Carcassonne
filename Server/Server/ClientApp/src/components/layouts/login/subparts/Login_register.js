import React, { Component } from 'react';
import { MButton } from '../../../simple/MButton';
import { MInput } from '../../../simple/MInput';
import { useNavigate } from "react-router-dom";

export function Login_registerWithRoute(props) {
    const navigate = useNavigate();
    return (<Login_register navigate={navigate} {...props}></Login_register>);
}

export class Login_register extends Component {
    constructor() {
        super();
        this.state = {
            email: "",
            nick: "",
            password: "",
            password2: "",
        };
    }

    register() {
        if (this.state.password != this.state.password2) {
            this.props.addMessage("Password are not the same");
            return;
        }
        this.props.fetch(this.props.navigate, '/security/register', {
            nick: this.state.nick,
            email: this.state.email,
            password: this.state.password
        }, (res) => {
            if (!res.Success) this.props.addMessage(res.Message);
        });
    }

    render() {
        return (
            <div>
                <h1>Register</h1>
                <div className="form-container">
                    <MInput text="Email" update={(t) => { this.setState({ email: t }); }} placeholder="user@domain.com"></MInput>
                    <MInput text="Nick" update={(t) => { this.setState({ nick: t }); }} placeholder="Your nick"></MInput>
                    <MInput text="Password" password="1" update={(t) => { this.setState({ password: t }); }} placeholder="Top-secret"></MInput>
                    <MInput text="Confirm password" password="1" update={(t) => { this.setState({ password2: t }); }} placeholder="Yeach again"></MInput>
                </div>
                <MButton text="Register" width="200" click={this.register.bind(this)}></MButton>
                <p onClick={this.props.changeSubpart}>Or click here to login</p>
            </div>
        );
    }
}