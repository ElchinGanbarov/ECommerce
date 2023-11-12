$(document).ready(function () {
    console.log("cards-script");
    $(function () {
        $(".heart").on("click", function () {
            $(this).toggleClass("is-active");
        });



        $(".single-products-heart").on("click", function () {
            console.log("adsa");
            $(this).toggleClass("is-active");
        });


        $("#dialog-basket").dialog({
            autoOpen: false
        });

        $("#downloaded-dialog").dialog({
            autoOpen: false,
        });

        $(".basket-icon-div").on("click", function (e) {
            var link = this;

            e.preventDefault();
            $("#dialog-basket").dialog({
                autoOpen: false,
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                buttons: {
                    "Continue to shopping": function () {
                        $(this).dialog("close");
                    },
                    "ViewCart": function () { window.location.href = "#basket"; },
                }
            });

            $("#dialog-basket").dialog('open');

        });



        $(".download-icon-div").on("click", function (e) {
            e.preventDefault();
            $("#downloaded-dialog").dialog({
                autoOpen: false,
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                buttons: {

                    "View Items": function () { window.location.href = "#itemlinks"; },

                    "OK": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#downloaded-dialog").dialog('open');

        });

        $("#dialog-wishlist").dialog({
            autoOpen: false
        });



        $(".heart").on("click", function (e) {

            e.preventDefault();
            $("#dialog-wishlist").dialog({
                autoOpen: false,
                resizable: false,
                height: "auto",
                width: 400,
                modal: true,
                buttons: {

                    "View Items": function () { window.location.href = "#WishList"; },

                    "OK": function () {
                        $(this).dialog("close");
                    }
                }
            });
            $("#dialog-wishlist").dialog('open');


        });


    });

});