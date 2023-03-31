import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';

import { FetchManager } from './components/managers/FetchManager';

import { Login } from "./components/layouts/login/Login";
import { LobbyWithRoute } from "./components/layouts/lobby/Lobby";
import { RoomWithRoute } from "./components/layouts/room/Room";
import { GameWithRoute } from "./components/layouts/game/Game";

import './App.css';

export default class App extends Component {

    constructor() {
        super();
    }

    static displayName = App.name;

    render() {
        return (
            <Routes>
                <Route index="1" element={(<Login fetch={FetchManager} />)} />
                <Route path="/lobby" element={(<LobbyWithRoute fetch={FetchManager} />)} />
                <Route path="/room" element={(<RoomWithRoute fetch={FetchManager} />)} />
                <Route path="/game" element={(<GameWithRoute fetch={FetchManager} />)} />
            </Routes>
        );
    }
}
