let btn = document.querySelector("#btn");
let sidebar = document.querySelector(".sidebar");
let alertSale = document.getElementById("alert_sale");
let alertRefund = document.getElementById("alert_refund");
let toDate = document.querySelector(".toDate");
let fromDate = document.querySelector(".fromDate");
let showPassword = document.querySelector(".watch-password");


if (showPassword != undefined) {
    showPassword.addEventListener("click", function (e) {
        console.log(this.firstChild.getAttribute("class"))
        if (document.getElementById("Password").getAttribute("type") == "password") {
            this.firstChild.classList.remove;
            this.firstChild.setAttribute("class", "fa-solid fa-eye-slash")               
            document.getElementById("Password").setAttribute("type", "text")
        } else {
            document.getElementById("Password").setAttribute("type", "password")
            this.firstChild.classList.remove;
            this.firstChild.setAttribute("class", "fa-solid fa-eye")     
        }
    })
}


if (btn != undefined) {
    btn.onclick = function () {
        sidebar.classList.toggle("active");
    }
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

if (alertRefund != null) {
    opacityAlert();
    hideAlert();
}

function hideAlert() {
    setTimeout(() => {
        alertRefund.style.display = "none"
    }, "2500");
}

function opacityAlert() {
    setTimeout(() => {
        alertRefund.style.opacity = "0"
    }, "2000");
}

function HideAlertSale() {
    alertSale.style.opacity = "0";
}

function Remove() {
    alertSale.style.display = "none";
}







