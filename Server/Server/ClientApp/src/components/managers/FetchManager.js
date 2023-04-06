import React, { Component } from 'react';

export function FetchManager(navigate, host, body, callback) {
    fetch(host, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: "same-origin",
        body: JSON.stringify(body)
    }).then(res => res.text()).then(res => {
        try {
            res = JSON.parse(res);
        } catch {
            console.log("Cannot convert to JSON", res);
            return;
        }
        if (res.Navigate != undefined && res.Navigate.length > 0) {
            navigate(res.Navigate);
            return;
        }
        callback(res);
    });
}