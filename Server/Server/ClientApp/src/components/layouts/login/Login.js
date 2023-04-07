import React, { Component } from 'react';
import { Login_login, Login_loginWithRoute } from './subparts/Login_login';
import { Login_register, Login_registerWithRoute } from './subparts/Login_register';
import { MLogger } from '../../simple/Logger/MLogger';
import './Login.css'
import { MButton } from '../../simple/MButton';

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

    componentDidMount() {
        this.props.connection.on('Send', message => {
            console.log(message);
        });
        // this.props.connection.invoke('SendToAll', 'user', 'message')
    }

    componentWillUnmount() {
        //
    }

    render() {
        return (
            <div className="main">
                <MLogger message={(addMessage) => { this.addMessage = addMessage; }}/>
                <div className="content">
                    <img src="img/logo.png" className="logo"></img>
                    {
                        this.state.subpart == 'login' ?
                            (<Login_loginWithRoute fetch={this.props.fetch} changeSubpart={this.changeSubpart.bind(this)} addMessage={this.log.bind(this)}></Login_loginWithRoute>) :
                            (<Login_registerWithRoute fetch={this.props.fetch} changeSubpart={this.changeSubpart.bind(this)} addMessage={this.log.bind(this)}></Login_registerWithRoute>)
                    }
                    {/*<MButton text="test" click={() => {
                        this.props.connection.invoke('SendToAll', 'user', 'message')
                    }} />
                    <MButton text="connect" click={() => {
                        this.props.connection.start()
                            .then(() => console.log('Połączono z serwerem SignalR'))
                            .catch(err => console.error(err));
                    }} />
                    <MButton text="check" click={() => {
                        console.log(this.props.connection)
                    }} />*/}
                </div>
            </div>
        );
    }
};