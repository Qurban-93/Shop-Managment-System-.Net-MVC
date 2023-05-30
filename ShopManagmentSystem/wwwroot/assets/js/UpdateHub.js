"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/update").build();
connection.start();

connection.on("DeleteDisplacement", function (id) {

    let deleteElement = document.getElementById(id);
    console.log(deleteElement)
    deleteElement.remove();     
   
})