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
            nick: "",
            password: ""
        };
    }

    login() {
        this.props.fetch(this.props.navigate, '/security/login', {
            nick: this.state.nick,
            password: this.state.password
        }, (res) => {
            if (!res.Success) this.props.addMessage(res.Message);
        });
    }

    enterPress(event) {
        if (event.key === "Enter") {
            this.login();
        }
    }

    componentDidMount() {
        document.addEventListener("keydown", this.enterPress.bind(this), false);
    }

    componentWillUnmount() {
        document.removeEventListener("keydown", this.enterPress, false);
    }

    // this.props.navigate("/lobby");

    render() {
        return (
            <div>
                <h1>Login</h1>
                <div className="form-container">
                    <MInput text="Nick" update={(t) => { this.setState({ nick: t }); }} placeholder="Your nick"></MInput>
                    <MInput text="Password" password="1" update={(t) => { this.setState({ password: t }); }} placeholder="Top-secret"></MInput>
                </div>
                <MButton text="Login" width="200" click={ this.login.bind(this) }></MButton>
                <p onClick={ this.props.changeSubpart }>Or click here to register</p>
            </div>
        );
    }
}