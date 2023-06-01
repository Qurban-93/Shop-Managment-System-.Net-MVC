"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();

var months = ['January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December'];

function formatAMPM(Date) {
    
    
    var hours = Date.getHours();
    var minutes = Date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}


connection.on("Online", function (userId) {

    document.getElementById(userId).classList.remove("offline");
    document.getElementById(userId).classList.add("online");
    document.getElementById(userId).nextElementSibling.innerHTML = "online";
    if (document.getElementById("user_name").innerHTML != "Choose User") {
        document.getElementById("user_last_seen").innerHTML = "Online";
        document.getElementById("user_last_seen").style.color = "#86c541";
    }

});

connection.on("Offline", function (userId) {

    document.getElementById(userId).classList.remove("online");
    document.getElementById(userId).classList.add("offline");
    document.getElementById(userId).nextElementSibling.innerHTML = "offline";

    var d = new Date();
    var day = d.getDate().toString();
    day = day < 10 ? '0' + day : day
    var month = months[d.getMonth().toString()];
    var year = d.getFullYear().toString();
    var fullDate = day + " " + month + " " + year + " , " + formatAMPM(d)


    document.getElementById("user_last_seen").innerHTML = fullDate;
    document.getElementById("user_last_seen").style.color = "#464746";

});

document.getElementById("send_btn").addEventListener("click", function (e) {
    let message = document.getElementById("message_content").value;
    let destinationUserId = document.getElementById("user_name").getAttribute("data-id");
    let senderUserId = document.getElementById("sender_id").value;
    if (message.trim().length == 0) {
        alert("Bosh Olmaz !");
        return;
    }
    connection.invoke("SendMessage", message, senderUserId, destinationUserId).catch(function (err) {
        return console.error(err.toString());
    }); 

    var d = new Date();
    var day = d.getDate().toString();
    var month = months[d.getMonth().toString()];
    var year = d.getFullYear().toString();
    var fullDate = day + " " + month + " " + year + " , " + formatAMPM(d)

    let myMessage = `<li class="clearfix text-start">
            <div class="message-data text-end">
                <span class="message-data-time">${fullDate}</span>
            </div>
            <div class="message other-message float-right"> ${message} </div>
        </li>`

    let chatList = document.querySelector(".chat_list");
    chatList.innerHTML += myMessage
    document.getElementById("message_content").value = "";
    DeleteIcon();
    
    
})

function DeleteIcon() {
    var icon = document.querySelector(".fa-solid fa-envelope");
    console.log(icon)
    if (icon != undefined || icon != null) { icon.remove() }
}

connection.on("ShowMessage", function (senderUserId, message, destinationUserId) {
    
    let UserId = document.getElementById("user_name").getAttribute("data-id");
    
    if (UserId != null && senderUserId == UserId) {

        var d = new Date();
        var day = d.getDate().toString();
        var month = months[d.getMonth().toString()];
        var year = d.getFullYear().toString();
        var fullDate = day + " " + month + " " + year + " , " + formatAMPM(d)

        let myMessage = `<li class="clearfix text-start">
            <div class="message-data">
                <span class="message-data-time">${fullDate}</span>
            </div>
            <div class="message my-message"> ${message} </div>
        </li>`

        let chatList = document.querySelector(".chat_list");
        chatList.innerHTML += myMessage
        return;
    }
    let name = document.querySelectorAll(".name");
        name.forEach((key, value, element) => {          

            if (key.parentElement.parentElement.getAttribute("data-id") == senderUserId) {
                key.innerHTML += `<i class="fa-solid fa-envelope text-danger"></i>`;
            }
            

        })
    

})

























