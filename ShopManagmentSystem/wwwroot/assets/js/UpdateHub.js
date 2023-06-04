"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/update").build();
connection.start();


connection.on("NewMessageCount", function (destinationUserId) {
    console.log(destinationUserId)
    let countIcon = document.getElementById(destinationUserId);
    
    let numCount = Number(countIcon.innerHTML);
    numCount++; 
    countIcon.innerHTML = numCount;

})