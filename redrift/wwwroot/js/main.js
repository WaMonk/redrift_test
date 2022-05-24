"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/message").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("AuthorizeOK", function (message) {
    console.log("Linked!");
});

connection.on("LobbyUpdate", function (message) {
    console.log("Event Update. Data: " + message);
});

connection.on("MatchUpdate", function (message) {
    console.log("Match Update. Data: " + message);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var token = document.getElementById("tokenInput").value;
    connection.invoke("Authorize", token).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});