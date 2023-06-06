"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/update").build();
connection.start();


connection.on("NewMessageCount", function (destinationUserId) {   
    let countIcon = document.getElementById(destinationUserId);

    if (countIcon.classList.length<2) {
        console.log("salam")
        let numCount = Number(countIcon.innerHTML);
        numCount++;
        countIcon.innerHTML = numCount;
        playSound();
    } 
})

connection.on("DeleteDisplacement", function (id) {
    let element = document.getElementById(id);
    if (element != undefined || element != null) {
        element.remove();
    }
})


function playSound() {
    document.getElementById("notification").play();
}