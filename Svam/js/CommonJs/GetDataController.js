


$(document).ready(function () {
    $.noConflict();
    GetDepartment();
    getUser();
   
});

//Start To Get All User in Dropdownlist......
function getUser() {
    jQuery.get('/common/GetAllusers', null, function (data) { chkBindgetUser(data) });
}

function chkBindgetUser(abc) {
    var data = "";
    data += "<option value= >--Select User--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + " >" + abc[it].Fname + " " + abc[it].Lname + "</option>";
    }
    $('.getuser').html(data)
}

// Start to Get The All Department
function GetDepartment() {
    jQuery.get('/common/GetAllDepartment', null, function (data) { chkBindDepartment(data) });
}

function chkBindDepartment(abc) {
    var data = "";
    data += "<option value= >--Select Department--</option>";
    for (var it in abc) {
        data += "<option value=" + abc[it].Id + " >" + abc[it].DepartmentName +"</option>";
    }
    $('.getdepartment').html(data)
}


