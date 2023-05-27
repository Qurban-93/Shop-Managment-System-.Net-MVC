$(document).ready(function () {

    
    $('.accordion-list > li > .answer').hide();

    $('.accordion-list > li > h3').click(function (e) {
        let parent = $(e.currentTarget).parent();
        console.log(parent)
        if (parent.hasClass("active")) {
            parent.removeClass("active").find(".answer").slideUp();
        } else {
            /*$(".accordion-list > li.active .answer").slideUp();*/
            /*$(".accordion-list > li.active").removeClass("active");*/
            parent.addClass("active").find(".answer").slideDown();
        }
        return false;
    });

});