import * as signalR from '@microsoft/signalr';

// Connection for SignalR Hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub", {
        // Comment this line to use WebSocket transport protocol instead of ServerSentEvents
        transport: signalR.HttpTransportType.ServerSentEvents
    })
    //.configureLogging(signalR.LogLevel.Debug)
    .build();

const _afterOpen = []
const _afterConn = []
var _connectedWithPerson = false;

export default class ConnectionManager {

    open(navigate, callback, error) {
        connection.start().then(() => {
            if (callback != undefined) callback();
            for (let i in _afterOpen) {
                _afterOpen[i]();
            }
            _afterOpen.splice(0, _afterOpen.length);
            connection.on("Navigate", (path) => {
                navigate(path);
            });
            setInterval(() => connection.invoke("Ping"), 500);
        }).catch(err => {
            if (error != undefined) error(err);
        });
    }
    join(fetch, navigate, callback) {
        fetch(navigate, '/security/getguid', {}, (res) => {
            if (!res.Success) return console.error("cannot join to bridge"); // CLOG
            var guid = res.Guid;
            var count = 3;
            var send = () => {
                this.invoke('Join', guid);
            }
            var func = success => {
                if (!success) { if (--count > 0) send(); else this.off('Connection', func); return; }
                if (callback != undefined) callback();
                this.off('Connection', func);
                _connectedWithPerson = true;
                for (let i in _afterConn) {
                    _afterConn[i]();
                }
            };
            this.on('Connection', func);
            send();
        });
    }
    ready(callback) {
        if (_connectedWithPerson) callback();
        else _afterConn.push(callback);
    }
    afterOpen(callback) {
        if (connection._connectionState == "Connected") callback();
        else _afterOpen.push(callback);
    }
    invoke(name, ...params) {
        connection.invoke(name, ...params);
    }
    on(action, callback) {
        connection.on(action, callback);
    }
    off(action, callback) {
        connection.off(action, callback);
    }
}