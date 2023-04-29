let btn = document.querySelector("#btn");
let sidebar = document.querySelector(".sidebar");
let alertSale = document.getElementById("alert_sale");

btn.onclick = function(){
  sidebar.classList.toggle("active");
}

setTimeout(() => {
    console.log("salam")
}, "3000");

setTimeout(() => {
    if (alertSale != undefined) {
        HideAlertSale();
    }
    HideAlertSale();
}, "2000");


function HideAlertSale() {
    alertSale.style.opacity = "0"; 
}

function Remove() {
    alertSale.remove;
}


