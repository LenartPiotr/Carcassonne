import * as signalR from '@microsoft/signalr';

// Connection for SignalR Hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hub", {
        // Comment this line to use WebSocket transport protocol instead of ServerSentEvents
        transport: signalR.HttpTransportType.ServerSentEvents
    })
    //.configureLogging(signalR.LogLevel.Debug)
    .build();

// console.log(connection);

const _afterOpen = []

export default class ConnectionManager {

    open(callback, error) {
        connection.start().then(() => {
            if (callback != undefined) callback();
            for (let i in _afterOpen) {
                _afterOpen[i]();
            }
            _afterOpen = [];
        }).catch(err => {
            if (error != undefined) error(err);
        });
    }
    join(fetch, navigate, callback) {
        fetch(navigate, '/security/getguid', {}, (res) => {
            if (!res.Success) return console.error("cannot join to bridge");
            var guid = res.Guid;
            var count = 3;
            var send = () => {
                this.invoke('Join', guid);
            }
            this.on('Connection', success => {
                if (!success) { if (--count > 0) send(); return; }
                if (callback != undefined) callback();
            });
            send();
        });
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