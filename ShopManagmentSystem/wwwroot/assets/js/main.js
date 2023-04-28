let btn = document.querySelector("#btn");
let sidebar = document.querySelector(".sidebar");
let alertSale = document.getElementById("alert_sale");

btn.onclick = function(){
  sidebar.classList.toggle("active");
}


setTimeout(() => {
    HideAlertSale();
}, "2000");

function HideAlertSale() {
    alertSale.style.opacity = "0";
    
}


