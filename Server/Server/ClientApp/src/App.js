import React, { Component } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';

import { FetchManager } from './components/managers/FetchManager';
import ConnectionManager from './components/managers/ConnectionManager';

import { Login } from "./components/layouts/login/Login";
import { LobbyWithRoute } from "./components/layouts/lobby/Lobby";
import { RoomWithRoute } from "./components/layouts/room/Room";
import { GameWithRoute } from "./components/layouts/game/Game";

import './App.css';

export function AppWithRoute(props) {
    const navigate = useNavigate();
    return (<App navigate={navigate} {...props}></App>);
}

export default class App extends Component {

    constructor(props) {
        super(props);
        this.conn = new ConnectionManager();
        this.conn.open(props.navigate);
    }

    static displayName = App.name;

    render() {
        return (
            <Routes>
                { /* fetch={FetchManager} */ }
                <Route index="1" element={(<Login fetch={FetchManager} />)} />
                <Route path="/lobby" element={(<LobbyWithRoute fetch={FetchManager} />)} />
                <Route path="/room" element={(<RoomWithRoute fetch={FetchManager} />)} />
                <Route path="/game" element={(<GameWithRoute fetch={FetchManager} />)} />
            </Routes>
        );
    }
}
