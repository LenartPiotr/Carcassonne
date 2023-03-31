import React, { Component } from 'react';

export function FetchManager(navigate, host, body, callback) {
    fetch(host, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: "same-origin",
        body: JSON.stringify(body)
    }).then(res => res.json()).then(res => {
        if (res.Navigate != undefined && res.Navigate.length > 0) {
            navigate(res.Navigate);
            return;
        }
        callback(res);
    });
}