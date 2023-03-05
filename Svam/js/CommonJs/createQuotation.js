

(function () {
  
    GetQuotationType();
    GetQuotationStatus();
    GetSourceBranchList();
    GetAllProducttaxGroup();
    GetAllServicetaxGroup();
})();

//Start To get Quotation Type....
function GetQuotationType() {
    jQuery.get('/common/Getquotationtype', null, function (data) { chkBindquotationtype(data) });
}

function chkBindquotationtype(abc) {
    var data = "";
    data += "<option value= >--Select Quotation Type--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + " >" + abc[it].QuotationTypeName + "</option>";
    }
    $('.ddlquotationtype').html(data)
}

//Start To get Quotation Status....
function GetQuotationStatus() {
    jQuery.get('/common/Getquotationstatus', null, function (data) { chkBindquotationStatus(data) });
}

function chkBindquotationStatus(abc) {
    var data = "";
    data += "<option value= >--Select Quotation Type--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + " >" + abc[it].QStatusName + "</option>";
    }
    $('.ddlquotationstatus').html(data)
}


//Start To get the Source Branch.........
function GetSourceBranchList() {
    jQuery.get('/common/Getbranchlist', null, function (data) { chkBindSourceBranchList(data) });
}

function chkBindSourceBranchList(abc) {
    var data = "";
    data += "<option value= >--Select Branch--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + " >" + abc[it].BranchName +" ("+abc[it].CityName+")"+ "</option>";

    }
    $('.ddlbranch').html(data);
}

//Collect the Product Tax......
function GetAllProducttaxGroup() {
    jQuery.get('/common/GetProducttaxGroup', null, function (data) { chkBindProductTaxgroup(data) });
}

function chkBindProductTaxgroup(abc) {
    var data = "";
    data += "<option value= >--Select Product Tax group--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + ">" + abc[it].TaxgroupName + "</option>";
    }
    $('.ddlproductTaxgroup').html(data);
}

//Collect the Product Tax......
function GetAllServicetaxGroup() {
    jQuery.get('/common/GetservicetaxGroup', null, function (data) { chkBindServiceTaxgroup(data) });
}

function chkBindServiceTaxgroup(abc) {
    var data = "";
    data += "<option value= >--Select Service Tax group--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + ">" + abc[it].ServicetaxGroupName + "</option>";
    }
    $('.ddlserviceTaxgroup').html(data);
}



