﻿$(document).ready(function () {

    let countBasket = parseInt($(".count").html());
    let totalPrice = parseInt($(".total_price").html());
    let itemCount = parseInt($(".item_count").html());
    let orderDeleteBtn = $('.order_delete_Btn');
    let order = $(".order");
    let deleteBasketItem = $(".deleteBasketItem");
    let deleteBrandBtn = $(".delete_brand");
    let deleteProdCategory = $(".delete_prod_category");
    let deletePosition = $(".delete_position");
    let deleteEmployee = $(".delete_employee");
    let deleteProdModel = $(".delete_prod_model");
    let deleteColor = $(".delete_color");
    let deletePunishment = $(".delete_punishment");
    let addList = $(".add_move");
    let deleteResendProd = $(".displace_list");
    let deleteBranch = $(".delete_branch");
    let chatHistory = $(".chat-history");
    let userChat = $(".select_user");
    let deleteProduct = $(".delete-product");
    let deleteUser = $(".delete-user");
    let prodBrandInput = $(".prod-brand");
    let prodBrandInputEdit = $(".prod-brand-edit");
    let checkboxInput = $(".form-check-input");
    let deleteDisplacement = $(".delete-displacement");
    var skip;

    function DisableSeriesInput(result) {
        if (result) {
            $(".series-input").prop("disabled", false)           
        } else {
            $(".series-input").prop("disabled", true)
        } 
    }


    $(".prod-model").change(function (e) {
        let id = $(".prod-model").val();
        $.ajax({
            method: "GET",
            url: "GetUniqueSeries/" + id,
            success: function (result) {

                DisableSeriesInput(result);
            },
            error: function (result) { }
        });
    })

    deleteDisplacement.on("click", function (e) {
        let id = $(e.currentTarget).data('id');

        Swal.fire({
            title: 'Are you sure delete ?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "/Displacement/Delete/" + id,
                    success: function (result) { 
                        window.location.href = '/displacement/';
                        Swal.fire(
                            ` Deleted !`,
                            'Your file has been deleted.',
                            'success'
                        )
                        
                    },
                    error: function (result) {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                        
                    }
                });


            }
        })
    })
    
    checkboxInput.change(function(e){
        let seriesInput = $("#series-length");
        if (seriesInput.prop("disabled")) {          
            $("#series-length").prop("disabled",false)
        } else {
            $("#series-length").prop("disabled", true)          
            $("#series-length").val("");
        }
        
    })

    prodBrandInputEdit.change(function (e) {
        let id = $(e.currentTarget).val();
        let modelsSelect = $(".prod-model-edit");
        $.ajax({
            method: "GET",
            url: "/Admin/Product/GetModels/" + id,
            success: function (result) {


                modelsSelect.find('option').remove()

                $.each(result, function (key, value) {

                    $('.prod-model-edit')
                        .append($("<option></option>")
                            .attr("value", value.value)
                            .text(value.text));
                });

            },
            error: function (result) { }
        });

    })

    prodBrandInput.change(function (e) {
        let id = $(e.currentTarget).val();
        let modelsSelect = $(".prod-model");
        $.ajax({
            method: "GET",
            url: "GetModels/" + id,
            success: function (result) {

                modelsSelect.find('option').remove()

                $.each(result, function (key, value) {

                    $('.prod-model')
                        .append($("<option></option>")
                            .attr("value", value.value)
                            .text(value.text));
                });

                let modelId = result[0].value;

                $.ajax({
                    method: "GET",
                    url: "GetUniqueSeries/" + modelId,
                    success: function (result) {

                        DisableSeriesInput(result);
                    },
                    error: function (result) { }
                });

            },
            error: function (result) { }
        });

    })

    deleteUser.on("click", function (e) {
        let id = $(e.currentTarget).data("id");
        let parentElement = $(e.currentTarget).parent().parent();

        Swal.fire({
            title: 'Are you sure delete ?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Account/Delete/" + id,
                    success: function (result) {

                        parentElement.remove()
                        Swal.fire(
                            `${result} Deleted !`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function (result) {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )

                    }
                });


            }
        })

    })

    deleteProduct.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();

        Swal.fire({
            title: 'Are you sure delete ?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Product/Delete/" + id,
                    success: function (result) {
                        console.log(result)
                        parentElement.remove()
                        Swal.fire(
                            `${result} Deleted !`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function (result) {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                        console.log(result)
                    }
                });


            }
        })
    })

    $('body').on('click', '#load_more', function () {

        if (skip == undefined || skip == null) {
            skip = $("#skip").val();
        }
        var id = $("#user_name").attr('data-id');
        var count = $("#message_count").val();

        $.ajax({
            method: "POST",
            data: { Id: id, Skip: skip },
            url: "/Message/ChatHistory/",
            success: function (result) {
                chatHistory.empty();
                chatHistory.append(result);
                console.log(typeof (skip), "skip", skip, typeof (count), "count", count)
                var numSkip = parseInt(skip);
                var numCount = parseInt(count);
                if (numCount < numSkip) {
                    $("#load_more").remove();
                }
                numSkip += 10;
                skip = numSkip;
            }
        });
    });

    userChat.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let name = $(e.currentTarget).data('name');
        let newMessageCount = $(".new_message").html();
        
        userChat.each(function (index, item) {
            $(item).removeClass("active");
        })
        $(this).addClass("active");

        $.ajax({
            method: "POST",
            data: { Id: id },
            url: "/Message/ChatHistory/",
            success: function (result) {
               $("#last-seen-input").val()
                chatHistory.empty();
                chatHistory.append(result);
               
                $("#user_name").html(name);
                $("#user_name").attr('data-id', id);
                $("#user_last_seen").html($("#last-seen-input").val());
                $(".chat-message").css("display", "block");
                $(".chat-header").css("display", "block");
               

                if (newMessageCount != undefined) {
                    var countMessage = $(".countMessage").html();
                    var numCountMessage = parseInt(countMessage);
                    var numNewMessageCount = parseInt(newMessageCount);
                    var totalCountMessage = numCountMessage - numNewMessageCount;
                    if (totalCountMessage >= 0) {
                        $(".countMessage").html(totalCountMessage);
                    }

                    $(".new_message").remove();
                }

                console.log($(".fa-solid fa-envelope"))

               
               
                var test = chatHistory.prop('scrollHeight');
                chatHistory.scrollTop(test);
            }
        });


    })

    deleteBranch.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();

        Swal.fire({
            title: 'Are you sure delete ?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Branch/Delete/" + id,
                    success: function (result) {
                        console.log(result)
                        parentElement.remove()
                        Swal.fire(
                            `${result} Deleted !`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function (result) {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                        console.log(result)
                    }
                });


            }
        })

    })

    deleteResendProd.on("click", ".delete_resend_prod", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent();
        let body = $(".body_list");

        $.ajax({
            method: "DELETE",
            url: "/Displacement/DeleteList/" + id,
            success: function (result) {
                parentElement.remove();
                let table = `<tr>
                            <td>${result.productBrand}</td>
                            <td>${result.model}</td>
                            <td>${result.color}</td>
                            <td>${result.category}</td>
                            <td>${result.series}</td>
                            <td><a style="cursor:pointer;" data-id="${result.id}" class="add_move"><i class="fa-solid fa-arrow-up-from-bracket"></i></a></td>
                        </tr>`
                body.append(table);
            }
        });


    });

    addList.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let list = $(".displace_list");
        let parentElement = $(e.currentTarget).parent().parent();

        $.ajax({
            method: "POST",
            url: "/Displacement/AddToList/" + id,
            success: function (result) {
                let prodHtml = `
                    <li>
                        ${result.productBrand} ${result.model} ${result.color} ${result.series}
                       <a style="cursor:pointer" class="delete_resend_prod" data-id="${result.id}"><i class="fa-solid fa-circle-minus text-danger"></i></a>
                    </li>`
                list.append(prodHtml);
                parentElement.remove()
                console.log($(".delete_resend_prod"))
            }
        });

    })

    deletePunishment.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();

        Swal.fire({
            title: 'Are you sure delete ?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Punishment/Delete/" + id,
                    success: function (result) {
                        console.log(result)
                        parentElement.remove()
                        Swal.fire(
                            `Deleted !`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function (result) {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                        console.log(result)
                    }
                });


            }
        })

    })

    deleteColor.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        Swal.fire({
            title: 'Are you sure delete Color?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Colors/Delete/" + id,
                    success: function (result) {
                        console.log(result)
                        parentElement.remove()
                        Swal.fire(
                            `Deleted ${result.colorName} !`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });


            }
        })
    });

    deleteProdModel.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        Swal.fire({
            title: 'Are you sure delete Model?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "ProductModel/Delete/" + id,
                    success: function (result) {
                        console.log(result)
                        parentElement.remove()
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });


            }
        })
    });

    deleteEmployee.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();

        Swal.fire({
            title: 'Are you sure delete Employee?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Employee/Delete/" + id,
                    success: function (result) {
                        console.log(result)
                        parentElement.remove()
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });


            }
        })
    })

    deletePosition.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        Swal.fire({
            title: 'Are you sure delete Positon?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "EmployeePosition/Delete/" + id,
                    success: function (result) {
                        parentElement.remove()
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });


            }
        })
    })

    deleteBrandBtn.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        Swal.fire({
            title: 'Are you sure delete Brand?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    method: "DELETE",
                    url: "Brand/Delete/" + id,
                    success: function (result) {
                        parentElement.remove()
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            'Not Deleted!',
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });


            }
        })
    })

    deleteProdCategory.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        Swal.fire({
            title: 'Are you sure delete Category?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    method: "DELETE",
                    url: "ProductCategory/Delete/" + id,
                    success: function (result) {
                        parentElement.remove()
                        Swal.fire(
                            ` Deleted!`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            `Not Deleted!`,
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });

            }

        });





    })

    orderDeleteBtn.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = this.parentElement.parentElement;

        Swal.fire({
            title: 'Are you sure delete Category?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({
                    method: "DELETE",
                    url: "Refund/OrderDelete/" + id,
                    success: function (result) {
                        parentElement.remove()
                        Swal.fire(
                            ` Deleted!`,
                            'Your file has been deleted.',
                            'success'
                        )
                    },
                    error: function () {
                        Swal.fire(
                            `Not Deleted!`,
                            'Your file has been not deleted.',
                            'error'
                        )
                    }
                });

            }
        });

    });

    order.on("click", function (e) {
        let id = $(e.currentTarget).data('id');
        let parentElement = $(e.currentTarget).parent().parent();
        let countSpan = $(".count");
        console.log("ok")

        $.ajax({
            method: "POST",
            url: "/Sale/AddProduct/" + id,
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

        Swal.fire({
            title: 'Are you sure delete Item?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {

                $.ajax({

                    method: "DELETE",
                    url: "/sale/delete/" + id,
                    success: function (result) {
                        parentElement.remove();
                        Swal.fire(
                            ` Deleted!`,
                            'Your item has been deleted.',
                            'success'
                        )
                        if (countBasket > 0) {
                            countBasket--;
                            totalPrice = totalPrice - result;
                            itemCount--;
                            itemCountHtml.html(`${itemCount}`);
                            countSpan.html(`${countBasket}`)
                            total.html(`${totalPrice}`);
                        }
                    },
                    error: function () {
                        Swal.fire(
                            `Not Deleted!`,
                            'Your item has been not deleted.',
                            'error'
                        )
                    }
                });

            }
        });
    })
});