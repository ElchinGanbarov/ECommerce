$(function () {

    $("#dialog-wishlist").dialog({
        autoOpen: false
    });



    $("#heart-btn").on("click", function () {

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


    $("#dialog-basket").dialog({
        autoOpen: false
    });

    $("#downloaded-dialog").dialog({
        autoOpen: false,
    });

    $("#add-to-cart-btn").on("click", function (e) {
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



    $("#download-btn").on("click", function (e) {
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





    var mainImg = document.getElementById("MainImg");
    var smallImg = document.getElementsByClassName("small-img");

    smallImg[0].onclick = function () {
        mainImg.src = smallImg[0].src;
    }

    smallImg[1].onclick = function () {
        mainImg.src = smallImg[1].src;;
    }

    smallImg[2].onclick = function () {
        mainImg.src = smallImg[2].src;
    }

    smallImg[3].onclick = function () {
        mainImg.src = smallImg[3].src;
    }

    smallImg[4].onclick = function () {
        mainImg.src = smallImg[4].src;
    }

    smallImg[5].onclick = function () {
        mainImg.src = smallImg[5].src;
    }

})