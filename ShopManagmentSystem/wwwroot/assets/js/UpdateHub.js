"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/update").build();
connection.start();

connection.on("DeleteDisplacement", function (id) {

    let deleteElement = document.getElementById(id);
    console.log(deleteElement)
    deleteElement.remove();     
   
})

connection.on("NewMessageCount", function (destinationUserId) {
    let countIcon = document.querySelector(`.${destinationUserId}`);
    let numCount = Number(countIcon.innerHTML);
    numCount++;
    console.log(numCount)
    countIcon.innerHTML = numCount;

})