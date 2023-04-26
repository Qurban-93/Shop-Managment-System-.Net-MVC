$(document).ready(function () {
    let countBasket = 0;
    $(".order").on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent(); 
        let countSpan = $(".count");

        console.log(id)
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

    function Count() {

    }

});