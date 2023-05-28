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



let userId = document.getElementById("userId");
let selectUserForChat = document.querySelectorAll(".selectUser");

selectUserForChat.forEach(element => {
    element.addEventListener('click', function () {
        let userName = document.querySelector(".userName");
        if (userName == null || userName == undefined) {

            var li = document.createElement("li");
            document.getElementById("messagesList").appendChild(li);
            li.classList.add("userName");
            document.querySelector(".userName").innerHTML = "Chat start with :" + element.name;
        } else {

            document.querySelector(".userName").innerHTML = "Chat start with :" + element.name;

        }
        userId.value = element.nextElementSibling.id
    })
})

connection.on("ShowAlert", function (user, message, senderId) {

    userId.value = senderId;
    let userName = document.querySelector(".userName");

    if (userName == null || user == undefined) {

        var liSenderName = document.createElement("li");
        document.getElementById("messagesList").appendChild(liSenderName);
        liSenderName.textContent = `Chat start with :${user}`
        liSenderName.classList.add("userName")


    } else {
        document.querySelector(".userName").textContent = `Chat start with :${user}`;

    }
    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.classList.add("sms")
    li.textContent = `${user} : ${message}`;

});

function CreateMessage(user, message) {
    let li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `${user} : ${message}`;
}


document.getElementById("sendButton").addEventListener("click", function () {
    var id = userId.value;
    var message = document.getElementById("messageInput").value;
    var nameSender = document.getElementById("senderUser").value;
    var senderId = document.getElementById("senderId").value;
    var li = document.createElement("li");



    if (message.trim().length > 0) {
        document.getElementById("messagesList").appendChild(li);

        li.textContent = ` Me : ${message}`;

        connection.invoke("SendMessage", id, senderId, nameSender, message).catch(function (err) {
            return console.error(err.toString());
        });
        message = "";
        event.preventDefault();
    } else {
        alert("Bosh olmaz !");
    }

});










