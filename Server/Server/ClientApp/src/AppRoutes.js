import { Login } from "./components/layouts/login/Login";
import { Lobby, LobbyWithRoute } from "./components/layouts/lobby/Lobby";
import { Room, RoomWithRoute } from "./components/layouts/room/Room";
import { Game, GameWithRoute } from "./components/layouts/game/Game";

const AppRoutes = [
    {
        index: true,
        element: <Login />
    },
    {
        path: '/lobby',
        element: <LobbyWithRoute />
    },
    {
        path: '/room',
        element: <RoomWithRoute />
    },
    {
        path: '/game',
        element: <GameWithRoute />
    }
];

export default AppRoutes;
