$("#IsDesignation").click(function () {
    if ($(this).is(":checked")) {
        $("#divDesig").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divDesig").hide();
        $("#DesigNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////dob click function///////////////////////
$("#IsDateofBirth").click(function () {
    if ($(this).is(":checked")) {
        $("#divDOB").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divDOB").hide();
        $("#DobNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////marriage click function///////////////////////
$("#IsMarriageAnniversary").click(function () {
    if ($(this).is(":checked")) {
        $("#divMrgAni").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divMrgAni").hide();
        $("#MrgNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////email click function///////////////////////
$("#IsEmailId").click(function () {
    if ($(this).is(":checked")) {
        $("#divEmail").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divEmail").hide();
        $("#EmailNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////country click function///////////////////////
$("#IsCountry").click(function () {
    if ($(this).is(":checked")) {
        $("#divCountry").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divCountry").hide();
        $("#CountryNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////state click function///////////////////////
$("#IsState").click(function () {
    if ($(this).is(":checked")) {
        $("#divState").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divState").hide();
        $("#StateNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////city click function///////////////////////
$("#IsCity").click(function () {
    if ($(this).is(":checked")) {
        $("#divCity").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divCity").hide();
        $("#CityNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////OrganizationName click function///////////////////////
$("#IsOrganizationName").click(function () {
    if ($(this).is(":checked")) {
        $("#divOrg").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divOrg").hide();
        $("#OrgNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////Address click function///////////////////////
$("#IsAddress").click(function () {
    if ($(this).is(":checked")) {
        $("#divAddr").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divAddr").hide();
        $("#AddrNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsFollowUpTime click function///////////////////////
$("#IsFollowUpTime").click(function () {
    if ($(this).is(":checked")) {
        $("#divFUPTime").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divFUPTime").hide();
        $("#FUPTimeNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsFollowupTimeinIST click function///////////////////////
$("#IsFollowupTimeinIST").click(function () {
    if ($(this).is(":checked")) {
        $("#divISTtime").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divISTtime").hide();
        $("#ISTNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsTimeZoneName click function///////////////////////
$("#IsTimeZoneName").click(function () {
    if ($(this).is(":checked")) {
        $("#divTZ").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divTZ").hide();
        $("#TZNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsUrl click function///////////////////////
$("#IsUrl").click(function () {
    if ($(this).is(":checked")) {
        $("#divURL").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divURL").hide();
        $("#URLNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsSkypeId click function///////////////////////
$("#IsSkypeId").click(function () {
    if ($(this).is(":checked")) {
        $("#divSkyId").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divSkyId").hide();
        $("#SkyIdNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsLeadResource click function///////////////////////
$("#IsLeadResource").click(function () {
    if ($(this).is(":checked")) {
        $("#divLdRes").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divLdRes").hide();
        $("#LSNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsProductType click function///////////////////////
$("#IsProductType").click(function () {
    if ($(this).is(":checked")) {
        $("#divProdTyp").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divProdTyp").hide();
        $("#ProdNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

/////////////IsExpectedClosingDate click function///////////////////////
$("#IsExpectedClosingDate").click(function () {
    if ($(this).is(":checked")) {
        $("#divExpClDate").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExpClDate").hide();
        $("#ExpDateNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});


///////////////////////IsExpectedDealAmount click function///////////////////////////////
$("#IsExpectedDealAmount").click(function () {
    if ($(this).is(":checked")) {
        $("#divExpAmt").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExpAmt").hide();
        $("#EpAmtNo").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});


///////////////////////IsExtraCol1 click function///////////////////////////////
$("#IsExtraCol1").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol1").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol1").hide();
        $("#ExtraCol1No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol2 click function///////////////////////////////
$("#IsExtraCol2").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol2").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol2").hide();
        $("#ExtraCol2No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol3 click function///////////////////////////////
$("#IsExtraCol3").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol3").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol3").hide();
        $("#ExtraCol3No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol4 click function///////////////////////////////
$("#IsExtraCol4").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol4").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol4").hide();
        $("#ExtraCol4No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol5 click function///////////////////////////////
$("#IsExtraCol5").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol5").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol5").hide();
        $("#ExtraCol5No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol6 click function///////////////////////////////
$("#IsExtraCol6").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol6").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol6").hide();
        $("#ExtraCol6No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol7 click function///////////////////////////////
$("#IsExtraCol7").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol7").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol7").hide();
        $("#ExtraCol7No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol8 click function///////////////////////////////
$("#IsExtraCol8").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol8").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol8").hide();
        $("#ExtraCol8No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol9 click function///////////////////////////////
$("#IsExtraCol9").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol9").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol9").hide();
        $("#ExtraCol9No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});

///////////////////////IsExtraCol10 click function///////////////////////////////
$("#IsExtraCol10").click(function () {
    if ($(this).is(":checked")) {
        $("#divExtraCol10").show();
        //console.log("Checkbox is checked.");
    }
    else if ($(this).is(":not(:checked)")) {
        $("#divExtraCol10").hide();
        $("#ExtraCol10No").prop("checked", true);
        //console.log("Checkbox is unchecked.");
    }
});