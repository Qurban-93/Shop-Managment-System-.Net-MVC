let btn = document.querySelector("#btn");
let sidebar = document.querySelector(".sidebar");
let alertSale = document.getElementById("alert_sale");
let alertRefund = document.getElementById("alert_refund");

btn.onclick = function(){
  sidebar.classList.toggle("active");
}


setTimeout(() => {
    if (alertSale != undefined || alertSale != null) {
        HideAlertSale();
    }
}, "2000");

setTimeout(() => {
    if (alertSale != undefined || alertSale != null) {
        Remove();
    }   
}, "3000");



//setTimeout(() => {
//    alertRefund.style.display
//}, "2000");

function HideAlertRefund() {
    alertRefund.style.display = "none";
}


function HideAlertSale() {
    alertSale.style.opacity = "0"; 
}

function Remove() {
    alertSale.style.display = "none";
}


