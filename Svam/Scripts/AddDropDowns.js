/// <reference path="../js/jquery-1.11.3.min.js" />

var prdDDlName = $("#prdddlname").val();
var prdname;
//prodct Type Name textbox keyup function
$("#txtboxProdTypeName").keyup(function () {
    prdname = $(this).val();
})

//btn  add product type name plus button click function
$("#btnPrdTpNmPlus").click(function () {
    $("#divProductTypeName").show();
    $("#btnPrdTpNmPlus").hide();
});

//btn cancel add product type name
$("#btnPrdTpNmCancel").click(function () {
    $("#txtboxProdTypeName").val('');
    $("#divProductTypeName").hide();
    $("#btnPrdTpNmPlus").show();
});

//btn product type name submit function
$("#btnAddProdTypeName").click(function () {
    if (prdname == undefined || prdname == '') {
        alert("Please enter " + prdDDlName);
    }
    else {
        $.get('/common/AddProductType', { prdTypeName: prdname }, function (data) {
            if (data == "empty") {
                alert("Please enter " + prdDDlName);
            }
            else if (data == "err") {
                alert("Something went wrong, please try again.");
            }
            else if (data == "exist") {
                alert(prdDDlName + " already exist");
            }
            else if (data == "expire") {
                alert("Your session has been expired, please login again.");
            }
            else if (data.Id > 0) {
                $('#ProductTypeID').append(`<option value="${data.Id}" selected='true'>${data.ProductTypeName}</option>`);
                $("#txtboxProdTypeName").val('');
                $("#divProductTypeName").hide();
                $("#btnPrdTpNmPlus").show();
            }
            else {
                alert("Something went wrong, please try again.");
            }
        });
    }
});

/////////////////////////////////////////////add lead status name script/////////////////////////////////////////////////
/// <reference path="../js/jquery-1.11.3.min.js" />

var prdDDlName = $("#prdddlname").val();
var prdname;
//prodct Type Name textbox keyup function
$("#txtboxProdTypeName").keyup(function () {
    prdname = $(this).val();
})

//btn  add product type name plus button click function
$("#btnPrdTpNmPlus").click(function () {
    $("#divProductTypeName").show();
    $("#btnPrdTpNmPlus").hide();
});

//btn cancel add product type name
$("#btnPrdTpNmCancel").click(function () {
    $("#txtboxProdTypeName").val('');
    $("#divProductTypeName").hide();
    $("#btnPrdTpNmPlus").show();
});

//btn product type name submit function
$("#btnAddProdTypeName").click(function () {
    if (prdname == undefined || prdname == '') {
        alert("Please enter " + prdDDlName);
    }
    else {
        $.get('/common/AddLeadStatus', { leadStatusName: ldsname }, function (data) {
            if (data == "empty") {
                alert("Please enter " + prdDDlName);
            }
            else if (data == "err") {
                alert("Something went wrong, please try again.");
            }
            else if (data == "exist") {
                alert(prdDDlName + " already exist");
            }
            else if (data == "expire") {
                alert("Your session has been expired, please login again.");
            }
            else if (data.Id > 0) {
                $('#ProductTypeID').append(`<option value="${data.Id}" selected='true'>${data.ProductTypeName}</option>`);
                $("#txtboxProdTypeName").val('');
                $("#divProductTypeName").hide();
                $("#btnPrdTpNmPlus").show();
            }
            else {
                alert("Something went wrong, please try again.");
            }
        });
    }
});

/////////////////////////////////////////////////add lead source name script//////////////////////////////////////////////////////
/// <reference path="../js/jquery-1.11.3.min.js" />

var prdDDlName = $("#prdddlname").val();
var prdname;
//prodct Type Name textbox keyup function
$("#txtboxProdTypeName").keyup(function () {
    prdname = $(this).val();
})

//btn  add product type name plus button click function
$("#btnPrdTpNmPlus").click(function () {
    $("#divProductTypeName").show();
    $("#btnPrdTpNmPlus").hide();
});

//btn cancel add product type name
$("#btnPrdTpNmCancel").click(function () {
    $("#txtboxProdTypeName").val('');
    $("#divProductTypeName").hide();
    $("#btnPrdTpNmPlus").show();
});

//btn product type name submit function
$("#btnAddProdTypeName").click(function () {
    if (prdname == undefined || prdname == '') {
        alert("Please enter " + prdDDlName);
    }
    else {
        $.get('/common/AddLeadStatus', { prdTypeName: prdname }, function (data) {
            if (data == "empty") {
                alert("Please enter " + prdDDlName);
            }
            else if (data == "err") {
                alert("Something went wrong, please try again.");
            }
            else if (data == "exist") {
                alert(prdDDlName + " already exist");
            }
            else if (data == "expire") {
                alert("Your session has been expired, please login again.");
            }
            else if (data.Id > 0) {
                $('#ProductTypeID').append(`<option value="${data.Id}" selected='true'>${data.ProductTypeName}</option>`);
                $("#txtboxProdTypeName").val('');
                $("#divProductTypeName").hide();
                $("#btnPrdTpNmPlus").show();
            }
            else {
                alert("Something went wrong, please try again.");
            }
        });
    }
});