$(document).ready(function () {


    $(".toggle-checbox").change(function () {
        var text = "";
        if ($(this).is(":checked")) {
            text = $(this).next().data("true-text");

        } else {
            text = $(this).next().data("false-text");
        }

        $(this).next().find("p").text(text);
    });

    $(".deleteitem").click(function (e) {
        e.preventDefault();
        let href = $(this).attr("href");
        $.confirm({
            title: 'Məlumat silinməsi',
            content: 'Silməyə əminsiniz mi?',
            buttons: {
                'Bəli': {
                    btnClass: "btn-danger",
                    action: function () {
                        location.href = href;
                    }
                },
                'Xeyr': function () {

                }
            }
        });
    });

    var toast = Cookies.get("toast");
    if (toast) {
        toast = toast.split("|");

        switch (toast[0]) {
            case "info":
                $.toast({
                    heading: 'Məlumat',
                    text: toast[1],
                    icon: 'info',
                    loader: true,
                    bgColor: '#3988ff',
                    loaderBg: '#f7d40d',
                    position: 'bottom-right'
                });
                break;
            case "success":
                $.toast({
                    heading: 'Müvəffəqiyyət',
                    text: toast[1],
                    icon: 'success',
                    bgColor: '#00cf69',
                    loader: true,
                    loaderBg: '#f7d40d',
                    position: 'bottom-right'
                });
                break;
            case "danger":
                $.toast({
                    heading: 'Tamamlanmadı',
                    text: toast[1],
                    icon: 'warning',
                    bgColor: '#ff045b',
                    loader: true,
                    loaderBg: '#ffffff',
                    position: 'bottom-right'
                });
                break;
        }

        Cookies.remove("toast");
    }

    if ($(".search-more").length) {

        $(".search-more .toogle-more").click(function () {
            $(this).next().slideToggle();

            if ($(this).find('i').hasClass("fa-chevron-down")) {
                $(this).find('i').removeClass("fa-chevron-down").addClass("fa-chevron-up");
            } else {
                $(this).find('i').removeClass("fa-chevron-up").addClass("fa-chevron-down");
            }
        });
    }

    if ($(".tab-menu").length) {
        $(".tab-menu .tab-link").click(function (e) {
            e.preventDefault();
            $(this).siblings().removeClass("active");
            $(this).addClass("active");

            $(this).parents(".tab-menu").find(".tab-item").removeClass("active");

            $($(this).attr("href")).addClass("active");
        });
    }

    $("#declaration-form").submit(function (e) {
        e.preventDefault();
        var formData = new FormData();

        $(this).serializeArray().forEach(function (item) {
            formData.append(item.name.replace("Declaration.", ""), item.value);
        });

        formData.append('upload', $(this).find("input[name='Declaration.Upload']")[0].files[0]);

        $.ajax({
            url: '/customers/checkout/declaration',
            type: 'POST',
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            //beforeSend: function () {
            //    $("#declarationClick").prop('disabled', true)
            //},
            success: function (res) {
                if (res.status === 200) {
                    window.location.href = res.url;
                } else if (res.status === 400) {
                    $.toast({
                        heading: 'Diqqət',
                        text: res.message,
                        icon: 'warning',
                        loader: true,
                        bgColor: '#e7c129',
                        loaderBg: '#f7d40d',
                        position: 'bottom-right'
                    });
                }
            }
        });
    });
    //Edit Order
    if ($(".edit-order-item").length) {

        var data = $(".order-form-item-wrapper").data();


        $(document).on("click", ".order-form-item .remove", function () {
            $(this).parents(".order-form-item").remove();
        });

        $(document).on("input", ".total-changer", function (e) {
            var orderPercentage = Number($(".order-form-item-wrapper").data("orderpercentage"));
            var total = 0;
            var priceCount = $(".priceCount-total").val();

            $(this).parents(".order-form-item").find(".total-changer").each(function () {
                total += Number($(this).val());
            });

            var subTotal = (priceCount * (total + (total * orderPercentage / 100))).toFixed(2);

            $(this).parents(".order-form-item").find(".total").val(subTotal + " " + $(".order-form-item-wrapper").data("currency"));
        });
        $(document).on("input", ".priceCount-total", function (e) {
            var orderPercentage = Number($(".order-form-item-wrapper").data("orderpercentage"));
            var total = 0;
            var priceCount = $(".priceCount-total").val();
            $(this).parents(".order-form-item").find(".total-changer").each(function () {
                total += Number($(this).val());
            });
            var subTotal = (priceCount * (total + (total * orderPercentage / 100))).toFixed(2);

            $(this).parents(".order-form-item").find(".total").val(subTotal + " " + $(".order-form-item-wrapper").data("currency"));
        });
    }
    // Package Added
    if ($(".add-order-package-item").length) {

        var datapackage = $(".order-form-package-wrapper").data();

        $(".add-order-package-item").click(function () {
            $.ajax({
                url: "/customers/checkout/getpackageitem",
                data: datapackage,
                dataType: "html",
                type: "Get",
                success: function (res) {
                    data.count++;
                    $(".order-form-package-wrapper").data("count", data.count);
                    $(".order-form-package-wrapper").append(res);

                    $("form").removeData("validator");
                    $("form").removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse("form");

                }
            });
        });
        $(document).on("click", ".order-form-package-item .remove", function () {
            $(this).parent(".order-form-package-item").remove();
        });
    }


    if ($(".add-order-item").length) {

        var data = $(".order-form-item-wrapper").data();

        $(".add-order-item").click(function () {
            $.ajax({
                url: "/customers/checkout/getorderitem",
                data: data,
                dataType: "html",
                type: "Get",
                success: function (res) {


                    data.count++;
                    $(".order-form-item-wrapper").data("count", data.count);
                    $(".order-form-item-wrapper").append(res);

                    $("form").removeData("validator");
                    $("form").removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse("form");
                }
            });
        });

        $(document).on("click", ".order-form-item .remove", function () {
            $(this).parents(".order-form-item").remove();
        });

        $(document).on("input", ".total-changer", function (e) {
            var orderPercentage = Number($(".order-form-item-wrapper").data("orderpercentage"));
            var total = 0;
            var priceCount = $(this).parents(".order-form-item").find(".priceCount-total").val();
            $(this).parents(".order-form-item").find(".total-changer").each(function () {
                total += Number($(this).val());
            });

            var subTotal = (priceCount * (total + (total * orderPercentage / 100))).toFixed(2);

            $(this).parents(".order-form-item").find(".total").val(subTotal + " " + $(".order-form-item-wrapper").data("currency"));


        });
        $(document).on("input", ".priceCount-total", function (e) {

            var orderPercentage = Number($(".order-form-item-wrapper").data("orderpercentage"));
            var total = 0;
            var priceCount = $(this).parents(".order-form-item").find(".priceCount-total").val();
            $(this).parents(".order-form-item").find(".total-changer").each(function () {
                total += Number($(this).val());
            });
            var subTotal = (priceCount * (total + (total * orderPercentage / 100))).toFixed(2);
            $(this).parents(".order-form-item").find(".total").val(subTotal + " " + $(".order-form-item-wrapper").data("currency"));
        });


        //Input currency click input value 0.00
        $(document).on("click", ".valuecurrency", function (e) {

            if ($(this).parents(".order-form-item").find(".valuecurrency").val() == "0.00") {
                $(this).parents(".order-form-item").find(".valuecurrency").val("");
            }
        });
        $(document).on("focusout", ".valuecurrency", function (e) {
            if ($(this).parents(".order-form-item").find(".valuecurrency").val() == "") {
                $(this).parents(".order-form-item").find(".valuecurrency").val("0.00");
            }
        });
        $(document).on("click", ".valuecargo", function (e) {
            if ($(this).parents(".order-form-item").find(".valuecargo").val() == "0.00") {
                $(this).parents(".order-form-item").find(".valuecargo").val("");
            }
        });
        $(document).on("focusout", ".valuecargo", function (e) {
            if ($(this).parents(".order-form-item").find(".valuecargo").val() == "") {
                $(this).parents(".order-form-item").find(".valuecargo").val("0.00");
            }
        });

    }



    $("#url-order-form").submit(function (e) {
        e.preventDefault();

        if (!$("#url-order-form").valid()) {
            return;
        }

        if (!$("#remote-sales").is(":checked")) {

            $.toast({
                heading: 'Diqqət',
                text: "MƏSAFƏLİ SATIŞ SÖZLƏŞMƏSİNİ QƏBUL EDİN",
                icon: 'warning',
                loader: true,
                bgColor: '#e7c129',
                loaderBg: '#f7d40d',
                position: 'bottom-right'
            });

            return;
        }

        var dataArr = $(this).serializeArray();
        var data = {};

        dataArr.forEach(function (item) {
            if (!item.name.includes("Order.") && !item.name.startsWith("_")) {
                data[item.name] = item.value;
            } else {
                data[item.name.replace("Order.", "")] = item.value;
            }
        });

        $.ajax({
            url: '/customers/checkout/orderurl',
            type: 'POST',
            dataType: "json",
            data: data,
            success: function (res) {
                if (res.status === 200) {
                    window.location.href = res.url;
                } else if (res.status === 400) {
                    $.toast({
                        heading: 'Diqqət',
                        text: res.message,
                        icon: 'warning',
                        loader: true,
                        bgColor: '#e7c129',
                        loaderBg: '#f7d40d',
                        position: 'bottom-right'
                    });
                }
            }
        });
    });

    if ($("#select-form").length) {
        $("#select-form #CountryId").change(function () {
            var data = {
                countryId: Number($(this).val()),
                type: $("#select-form").find("#Type").val()
            };

            $("#select-form #CompanyId").html(`<option selected="" disabled="">Seçin</option>`);

            $.ajax({
                url: "/customers/checkout/getpartners",
                data: data,
                dataType: "json",
                type: "get",
                success: function (res) {
                    res.forEach(function (item) {
                        $("#select-form #CompanyId").append(`<option value="${item.id}">${item.name}</option>`);
                    });
                }
            });
        });

        $("#select-form").submit(function (e) {
            e.preventDefault();


            let countryId = $("#CountryId").val();
            let companyId = $("#CompanyId").val();

            if (countryId === null || companyId === null) {
                $.toast({
                    heading: 'Məlumat',
                    text: "Ölkə və daşıma şirkəti seçin",
                    icon: 'warning',
                    loader: true,
                    bgColor: '#3988ff',
                    loaderBg: '#f7d40d',
                    position: 'bottom-right'
                });
                return;
            }

            $.ajax({
                url: "/customers/checkout/geturl",
                data: $(this).serialize(),
                dataType: "json",
                type: "get",
                success: function (res) {
                    window.location.href = res.url;
                }
            });
        });


    }

    if ($("#select-for-address-form").length) {
        $("#select-for-address-form #CountryId").change(function () {
            var data = {
                countryId: Number($(this).val())
            };

            $("#select-for-address-form #CompanyId").html(`<option selected="" disabled="">Seçin</option>`);

            $.ajax({
                url: "/customers/pages/getpartners",
                data: data,
                dataType: "json",
                type: "get",
                success: function (res) {
                    res.forEach(function (item) {
                        $("#select-for-address-form #CompanyId").append(`<option value="${item.id}">${item.name}</option>`);
                    });
                }
            });
        });
    }

    if ($('.copy-btn').length) {
        var clipboard = new ClipboardJS('.copy-btn');

        clipboard.on('success', function (e) {
            $.toast({
                heading: 'Müvəffəqiyyət',
                text: "Kopyalandı",
                icon: 'success',
                bgColor: '#00cf69',
                loader: true,
                loaderBg: '#f7d40d',
                position: 'bottom-right'
            });
        });
    }

    if ($("#buybasket-form").length) {
        $("#ShopLoginPasswordId").change(function () {
            $("#PartnerId").html(`<option selected disabled>Seçin</option>`);
            $.getJSON("/customers/mybaskets/GetCompanies/" + $(this).val(), function (res) {
                res.forEach((item) => {
                    $("#PartnerId").append(`<option data-percentage="${item.orderPercentage}" value="${item.id}">${item.name}</option>`);
                });
            });
        });

        $("#PartnerId").change(function () {
            let per = $(this).find("option:selected").data("percentage");

            $(".percentage").html(` (${per}%)`);
        });
    }

    if ($("[data-href]").length) {
        $("[data-href]").click(function () {
            window.location.href = $(this).data("href");
        });
    }

    if ($("#notify").length) {
        $("#notify").click(function () {
            if ($(this).hasClass("active")) {
                $(this).find(".notify-icon").removeClass("animated").removeClass("unread");
                let ids = [];

                setTimeout(function () {
                    $(".notify .list li").each(function () {
                        if ($(this).hasClass('unread')) {
                            $(this).removeClass("unread");
                            ids.push($(this).data('id'));
                        }
                    });
                }, 500);


                setTimeout(function () {
                    if (ids.length > 0) {
                        $.getJSON('/customers/notifcations/seenIds?ids=' + ids.toString(), function () { });
                    }
                }, 501);
            }
        });
    }

    if ($(".tabs-titles").length) {
        $(".tabs-titles .item-link").click(function (ev) {
            ev.preventDefault();

            $(".tabs-titles .item-link.active").removeClass("active");
            $(this).addClass("active");

            let id = $(this).attr("href");

            $(".tab-contents .content-item.active").removeClass("active");
            $(id).addClass("active");
        });
    }

    $.fn.sortChildren = function (sortingFunction) {

        return this.each(function () {
            const children = $(this).children().get();
            children.sort(sortingFunction);
            $(this).append(children);
        });

    };

    function orderByResult(value, type) {
        if (value === "price" && type === "asc") {
            $(".search-results .companies")
                .sortChildren((a, b) => Number(a.dataset.price) > Number(b.dataset.price) ? 1 : -1);
        } else if (value === "price" && type === "desc") {
            $(".search-results .companies")
                .sortChildren((a, b) => Number(a.dataset.price) < Number(b.dataset.price) ? 1 : -1);
        } else if (value === "ranking" && type === "asc") {
            $(".search-results .companies")
                .sortChildren((a, b) => Number(a.dataset.ranking) > Number(b.dataset.ranking) ? 1 : -1);
        } else if (value === "ranking" && type === "desc") {
            $(".search-results .companies")
                .sortChildren((a, b) => Number(a.dataset.ranking) < Number(b.dataset.ranking) ? 1 : -1);
        } else {
            $(".search-results .companies")
                .sortChildren((a, b) => Number(a.dataset.price) > Number(b.dataset.price) ? 1 : -1);
        }
    }

    if ($("#filter-result-form").length) {

        orderByResult("price", "asc");

        $(".calc-orderby").change(function () {
            let orderby = $(this).val().split('-');
            orderByResult(orderby[0], orderby[1]);
        });

        $(".calc-company").change(function () {
            let id = Number($(this).val());

            if (id === 0) {
                $(".search-results .companies .item").show();
                return;
            }

            $(".search-results .companies .item").hide("fast");

            $(".search-results .companies").find(`[data-id=${id}]`).fadeIn();
        });
    }

    if ($(".filter-balace-company").length) {
        $(".filter-balace-company .action a").click(function (e) {
            e.preventDefault();
            $(".total-price .totalpricespan").text($(this).data("price"));
        });
    }


    $(window).on("resize", function () {
        if ($(this.window).width() > 500 && $(this.window).width() < 1200 && $(".tabledatares").find("td").hasClass("dataTables_empty") && $(".tabledatares th").length < 8) {

            $(".tabledatares").css("display", "inline-table");
        } else if ($(window).width() > 1200 && ($(".tabledatares th").length > 12 || ($(".tabledatares th").length === 12 && !$(".tabledatares").find("td").hasClass("dataTables_empty")))) {
            $(".tabledatares").css("display", "block");
            $(".tabledatares").css("overflow", "auto");
        } else if ($(window).width() > 992 && $(window).width() < 1200 && $(".tabledatares th").length < 8 && !$(".tabledatares").find("td").hasClass("dataTables_empty")) {
            $(".tabledatares ").css("display", "inline-table");
        }
    });

    $(".CourierPaymentSelect").change(function () {

        var data = {
            id: $(this).data("id"),
            type: type = $(this).val()
        };
        var isCash = false;
        if (type === 0) {
            isCash = false;
        } else {
            isCash = true;
        }
        $.ajax({
            url: "/customers/CourierRequests/ChangePaymentType",
            method: "POST",
            data: data,
            success: function (res) {
                if (res.status) {
                    if (isCash) {
                        $(".courierPay" + data.id).addClass("d-none");
                    } else {
                        $(".courierPay" + data.id).removeClass("d-none");
                    }
                    $.toast({
                        heading: 'Müvəffəqiyyət',
                        text: "Ödəniş növü dəyişdirildi",
                        icon: 'success',
                        bgColor: '#00cf69',
                        loader: true,
                        loaderBg: '#f7d40d',
                        position: 'bottom-right'
                    });
                }
            }
        });
    });

    if ($(".total-pay-all").length) {
        $(document).on("change", ".total-pay-all input", function () {
            let checked = $(this).is(":checked");
            $(".total-pay-checkbox input").each(function () {
            if (checked) {
                if (!$(this).is(":checked")) {
                    $(this).trigger("click");
                }
            }else {
                if ($(this).is(":checked")) {
                    $(this).trigger("click");
                }
            }
            });
        });
    }

    //if ($(".total-pay-checkbox").length) {
        let checkedPackages = [];
        $(document).on("change", ".total-pay-checkbox input", function () {
            let id = $(this).val();

            if ($(this).is(":checked")) {
                checkedPackages.push({
                    id: id,
                    price: $(this).data("price")
                });
            } else {
                let index = checkedPackages.findIndex(c => c.id === id);
                checkedPackages.splice(index, 1);
            }

            packageChanged();

            if ($(".total-pay-checkbox input").length === $(".total-pay-checkbox input:checked").length) {
                $(".total-pay-all input").prop("checked", "checked");
            } else {
                $(".total-pay-all input").prop("checked", null);
            }
        });

        $(document).on("click", ".total-pay-actions .deliveryPrice", function (ev) {
            ev.preventDefault();

            let ids = $(this).data("ids");

            location.href = "/customers/packages/paymulti?packages=" + ids.toString();
        });

        function packageChanged() {
            if (checkedPackages.length > 0) {
                $(".total-pay-actions").removeClass("d-none");

                let total = 0;
                let ids = [];
                checkedPackages.forEach(c => {
                    total += c.price;
                    ids.push(Number(c.id));
                });

                $(".total-pay-actions").find("a").data("ids", ids);

                $(".total-pay-actions").find(".total span").text(total.toFixed(2));
            } else {
                $(".total-pay-actions").addClass("d-none");
                $(".total-pay-actions").find(".total span").text(0);
            }
        }
    //}


    //if ($(".total-pay-order-checkbox").length) {
        let checkedItems = [];
        $(document).on("change", ".total-pay-order-checkbox input", function () {
            console.log("tset")
            let id = Number($(this).val());

            if ($(this).is(":checked")) {
                let canAdd = true;

                let item = {
                    id: Number(id),
                    price: $(this).data("price"),
                    currency: $(this).data("currency"),
                    partnerId: $(this).data("partnerid")
                };

                if (checkedItems.length > 0) {
                    canAdd = checkedItems[0].currency === item.currency && checkedItems[0].partnerId === item.partnerId;
                }

                if (canAdd) {
                    checkedItems.push(item);
                } else {
                    $(this).prop("checked", null);
                    $.toast({
                        heading: 'Məlumat',
                        text: "Bir şirkətə və ölkəyə aid sifarişləri cəmləyə bilərsiniz",
                        icon: 'info',
                        loader: true,
                        bgColor: '#3988ff',
                        loaderBg: '#f7d40d',
                        position: 'bottom-right'
                    });
                }

            } else {
                let index = checkedItems.findIndex(c => c.id === id);
                checkedItems.splice(index, 1);
            }

            toggleCheckbox();
        });
    

        function toggleCheckbox() {
            if (checkedItems.length > 0) {
                $(".total-pay-order-checkbox").each((index, elem) => {
                    let value = {
                        partnerId: $(elem).find("input").data("partnerid"),
                        currency: $(elem).find("input").data("currency")
                    };

                    if (value.partnerId !== checkedItems[0].partnerId || value.currency !== checkedItems[0].currency) {
                        $(elem).addClass("d-none");
                    }
                });

                $(".total-pay-actions").removeClass("d-none");

                let total = 0;
                let ids = [];
                checkedItems.forEach(c => {
                    total += c.price;
                    ids.push(Number(c.id));
                });

                $(".total-pay-actions").find("a").data("ids", ids);
                $(".total-pay-actions").find("a").data("partnerid", checkedItems[0].partnerId);

                $(".total-pay-actions").find(".total span").eq(0).text(total.toFixed(2));
                $(".total-pay-actions").find(".total span").eq(1).text(checkedItems[0].currency);
            } else {
                $(".total-pay-order-checkbox").removeClass("d-none");
                $(".total-pay-actions").addClass("d-none");
                $(".total-pay-actions").find(".total span").eq(0).text(0);
                $(".total-pay-actions").find(".total span").eq(1).text("");
            }
        }

        $(".total-pay-actions .orderPrice").click(function (ev) {
            ev.preventDefault();

            let ids = $(this).data("ids");
            let partnerId = $(this).data("partnerid");

            let params = "";

            ids.forEach((value, index) => {
                params += "orderitemids=" + value;

                if (index !== ids.length - 1) {
                    params += "&";
                }
            });

            location.href = `/customers/payment/paymes-cart/${partnerId}?${params}`;
        });
    //}

    $(document).on("click", "#customerId", function () {
        $.confirm({
            title: 'Profilinizi tamamlayın..',
            content: 'Tamamlamaq istəyirsinizmi.?',
            buttons: {
                'Bəli': {
                    btnClass: "btn-success",
                    action: function () {
                        window.location.href = "/customers/settings";
                    }
                },
                'Xeyr': function () {
                }
            }
        });

    });
    $(document).on("click", ".returnPage", function () {
        $(".menu").removeClass("show");
    });

    $(document).on("click", "#exampleDataTable .img-show-detail", function () {
        var data = $(this).data();
        let i = 0;
        $.ajax({
            url: "/customers/packages/getpackage",
            data: data,
            dataType: "html",
            type: "get",
            success: function (res) {
                $(".modal-added-content").append(res);
            }
        });
        $(".modal-added-content").empty();
        $('#tableModal').modal('show');
    });


    //$('#exampleDataTable .img-show-detail').on('click', function () {
    //    var data = $(this).data();
    //    let i = 0;
    //    $.ajax({
    //        url: "/customers/packages/getpackage",
    //        data: data,
    //        dataType: "html",
    //        type: "get",
    //        success: function (res) {
    //            $(".modal-added-content").append(res);
    //        }
    //    });
    //    $(".modal-added-content").empty();
    //    $('#tableModal').modal('show');
    //});

    //$(".img-show-detail").click(function () {
    
    //});
    //$(function () {
    //        $('.js-example-basic-single').select2({
    //            placeholder: 'Seçin..',
    //            allowClear: true
    //        });
    //    });
});
