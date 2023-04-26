$(document).ready(function () {
    let countBasket = parseInt($(".count").html());
    $(".order").on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        let countSpan = $(".count");

        
        $.ajax({
            method:"POST",
            url: "Sale/AddProduct/" + id,
            success: function (result) {
                parentElement.remove();
                countBasket++;
                countSpan.html(`${countBasket}`)
            }
        });

    })



    $(".deleteBasketItem").on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        let countSpan = $(".count");
        

        $.ajax({
            method: "POST",
            url: "/sale/delete/" + id,
            success: function (result) {
                parentElement.remove();
                if (countBasket > 0) {
                    countBasket--;
                    countSpan.html(`${countBasket}`)
                }
                if (countBasket == 0) {
                    let goHome = `<a asp-action="index" asp-controller="home" style="text-align:center; text-decoration:none;">
            <h1>Mehsul Elave Edilmeyib !</h1></a>`
                    $(".home_index").html(`${goHome}`)
                }
                console.log("ok" );
            }
        });

    })

});