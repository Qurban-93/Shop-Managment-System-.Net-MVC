$(document).ready(function () {

    let countBasket = parseInt($(".count").html());
    let totalPrice = parseInt($(".total_price").html());   
    let itemCount = parseInt($(".item_count").html());
    let refundNotif = $("#alert_refund")
    let orderDeleteBtn = $('.order_delete_Btn');
    let order = $(".order");
    let deleteBasketItem = $(".deleteBasketItem");
    let selectModel = $(".prod_model");
    let selectBrand = $(".prod_brand");
    let selecCategory = $(".prod_category");

    console.log(selectModel, selectBrand, selecCategory)


    orderDeleteBtn.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = this.parentElement.parentElement;

        console.log(parentElement)
         
        $.ajax({
            method: "DELETE",
            url: "Refund/OrderDelete/" + id,
            success: function (result) {
                parentElement.remove();              
            }
        });

    })

    order.on("click", function (e) {
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

    deleteBasketItem.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        let countSpan = $(".count");
        let total = $(".total_price");
        let itemCountHtml = $(".item_count");
        

        $.ajax({
            method: "POST",
            url: "/sale/delete/" + id,
            success: function (result) {
                parentElement.remove();
                console.log(result)
                if (countBasket > 0) {
                    countBasket--;
                    totalPrice = totalPrice - result;
                    itemCount--;
                    itemCountHtml.html(`${itemCount}`);
                    countSpan.html(`${countBasket}`)
                    total.html(`${totalPrice}`);
                }
            //    if (countBasket == 0) {
            //        let goHome = `<a asp-action="index" asp-controller="home" style="text-align:center; text-decoration:none;">
            //<h1>Mehsul Elave Edilmeyib !</h1></a>`
            //        $(".home_index").html(`${goHome}`)
            //    }              
            }
        });

    })


    
});