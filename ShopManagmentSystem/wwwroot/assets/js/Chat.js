"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();




connection.on("Online", function (userId) {

    document.getElementById(userId).classList.remove("offline");
    document.getElementById(userId).classList.add("online");
    document.querySelector(".user_status").innerHTML = "online";
    document.getElementById("user_last_seen").innerHTML = "Online";

});


connection.on("Offline", function (userId) {

    document.getElementById(userId).classList.remove("online");
    document.getElementById(userId).classList.add("offline");
    document.querySelector(".user_status").innerHTML = "offline";
    document.getElementById("user_last_seen").innerHTML = "offline";

});
























