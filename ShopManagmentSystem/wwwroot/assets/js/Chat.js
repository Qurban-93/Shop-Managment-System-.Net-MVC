"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();


connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.textContent = `${user} says ${message}`;
});

connection.on("Online", function (userId) {

    document.getElementById(userId).classList.remove("bg-offline");
    document.getElementById(userId).classList.add("bg-online");

});


connection.on("Offline", function (userId) {

    document.getElementById(userId).classList.remove("bg-online");
    document.getElementById(userId).classList.add("bg-offline");

});

connection.on("ShowAlert", function (message) {

    alert(`${message} say Hello !`)

});


