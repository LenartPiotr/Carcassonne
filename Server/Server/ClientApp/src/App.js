import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';

import { FetchManager } from './components/managers/FetchManager';

import { Login } from "./components/layouts/login/Login";
import { LobbyWithRoute } from "./components/layouts/lobby/Lobby";
import { RoomWithRoute } from "./components/layouts/room/Room";
import { GameWithRoute } from "./components/layouts/game/Game";

import * as signalR from '@microsoft/signalr';

import './App.css';

// Connection for SignalR Hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .build();

export default class App extends Component {

    constructor() {
        super();
    }

    static displayName = App.name;

    render() {
        return (
            <Routes>
                { /* fetch={FetchManager} */ }
                <Route index="1" element={(<Login fetch={FetchManager} connection={connection} />)} />
                <Route path="/lobby" element={(<LobbyWithRoute fetch={FetchManager} connection={connection} />)} />
                <Route path="/room" element={(<RoomWithRoute fetch={FetchManager} connection={connection} />)} />
                <Route path="/game" element={(<GameWithRoute fetch={FetchManager} connection={connection} />)} />
            </Routes>
        );
    }
}
