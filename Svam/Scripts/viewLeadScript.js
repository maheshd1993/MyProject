

////////////////////////////////////fill email template function//////////////////////////
function FillEmailTemplate() {
    var emailTemplateID = jQuery('#EmailTemplateID').val();
    if (emailTemplateID > 0) {
        jQuery.ajax({
            url: '/Home/GetEmailTemplateContent',
            data: { EmailTemplateID: emailTemplateID },
            dataType: 'json',
            type: 'GET',
            success: function (data) {
                //CKEDITOR.instances["MessageBody"].insertHtml(data);
                jQuery('#MessageBody').summernote('code',data);
            }
        });
    }
}

///////////////////attachment file click function////////////////////////
jQuery('#AttachmentFile').on('click', function () {
    jQuery('#FileManager').attr('style', 'display:none;');
    jQuery('input.fileswitch').not(this).prop('checked', false);
});

///////////////////////file manager change function////////////////////
jQuery('#filemgr').change(function () {
    jQuery('#AttachmentFile').val('');
    if (this.checked) {
        jQuery('#FileManager').attr('style', 'display:block;');
    }
    else {
        jQuery('#FileManager').attr('style', 'display:none;');
    }
});

/////////////////fileExample change funtion//////////////////////////////
jQuery('input.fileexample').on('change', function () {
    var id = jQuery(this).val();
    jQuery('#FileID').val(id);
    jQuery('input.fileexample').not(this).prop('checked', false);
});

//////////////////close button click event////////////////////////

jQuery('.close,.btnclose').on('click', function () {
    jQuery('#txtEmailAddress').val('');
    jQuery('#ToEmailAddress').val('');
    jQuery('#ToContacts').val('');
    jQuery('.chk').prop('checked', false);
    jQuery('.chkAll').prop('checked', false);
    jQuery("#btnSendEmail").attr("style", "color: white; display:none;");
    jQuery("#btnSendSMS").attr("style", "color: white; display:none;");
});
////////////////////////check all checkbox function////////////////////////////////
function checkedall(e) {
    debugger;
    var selecteduserid = jQuery('select#UserddlName option:selected').val();
    jQuery('#Selecteduserid').val(selecteduserid);
    var chkAll = jQuery(e).is(':checked');
    var chkRows = jQuery("#dest>tbody>tr>td").find(".chk");
    chkRows.each(function () {
        if (chkAll == true) {
            jQuery(this)[0].checked = true;
        }
        else {
            jQuery(this)[0].checked = false;
        }
    });
    ///////get users emails/////////////
    var valuesArray = jQuery('input:checkbox:checked').map(function () {
        var emailaddress = this.value.split('/');
        return emailaddress[2];
    }).get().join("|");
    jQuery('#ToEmailAddress').val(valuesArray);
    //////////////end/////////////////////

    ///////get users phone numbers/////////////
    var phoneNoValuesArray = jQuery('input:checkbox:checked').map(function () {
        var phoneNo = this.value.split('/');
        return phoneNo[3];
    }).get().join("|");
    jQuery('#ToContacts').val(phoneNoValuesArray);
    /////////////////end///////////////////////

    var leadData = jQuery('input:checkbox:checked').map(function () {
        var leadValues = this.value;
        return leadValues;
    }).get().join("|");



    jQuery('#SelectedLead').val(leadData.trim());
    debugger;
    jQuery('#SelectedLead123').val(leadData.trim());
    if (valuesArray != '') {
        jQuery("#btnSendEmail").attr("style", "color: white; display:block;");
    }
    else {
        jQuery("#btnSendEmail").attr("style", "color: white; display:none;");
    }

    if (phoneNoValuesArray != '') {
        jQuery("#btnSendSMS").attr("style", "color: white; display:block;");
    }
    else {
        jQuery("#btnSendSMS").attr("style", "color: white; display:none;");
    }
}

////////////////////This script use for mobile app
function checkallbutton(e) {
    debugger;
   
    var chkAll = jQuery(e).is(':checked');

    var chkRows = jQuery("table>tbody>tr>td").find(".chk");
    chkRows.each(function () {
        if (chkAll == true) {
            jQuery(this)[0].checked = true;
        }
        else {
            jQuery(this)[0].checked = false;
        }
    });
    ///////get users emails/////////////
    var valuesArray = jQuery('input:checkbox:checked').map(function () {
        var emailaddress = this.value.split('/');
        return emailaddress[2];
    }).get().join("|");
    jQuery('#ToEmailAddress').val(valuesArray);
    //////////////end/////////////////////

    ///////get users phone numbers/////////////
    var phoneNoValuesArray = jQuery('input:checkbox:checked').map(function () {
        var phoneNo = this.value.split('/');
        return phoneNo[3];
    }).get().join("|");
    jQuery('#ToContacts').val(phoneNoValuesArray);
    /////////////////end///////////////////////

    var leadData = jQuery('input:checkbox:checked').map(function () {
        var leadValues = this.value;
        return leadValues;
    }).get().join("|");

    jQuery('#SelectedLead').val(leadData.trim());
    debugger;
    jQuery('#SelectedLead123').val(leadData.trim());
    if (valuesArray != '') {
        jQuery("#btnSendEmail").attr("style", "color: white; display:block;");
    }
    else {
        jQuery("#btnSendEmail").attr("style", "color: white; display:none;");
    }

    if (phoneNoValuesArray != '') {
        jQuery("#btnSendSMS").attr("style", "color: white; display:block;");
    }
    else {
        jQuery("#btnSendSMS").attr("style", "color: white; display:none;");
    }
}

/////////////////////// END /////////////////////////////////


//perticular check box click function
jQuery('.chk').on('click', function () {
    var valuesArray = jQuery('input:checkbox:checked').map(function () {
        var emailaddress = this.value.split('/');
        return emailaddress[2];
    }).get().join("|");
   
    jQuery('#ToEmailAddress').val(valuesArray.trim());

    ///////get users phone numbers/////////////
    var phoneNoValuesArray = jQuery('input:checkbox:checked').map(function () {
        var phoneNo = this.value.split('/');
        return phoneNo[3];
    }).get().join("|");
    jQuery('#ToContacts').val(phoneNoValuesArray);
    /////////////////end///////////////////////

    var leadData = jQuery('input:checkbox:checked').map(function () {
        var leadValues = this.value;
        return leadValues;
    }).get().join("|");

    jQuery('#SelectedLead').val(leadData);
    debugger;
    jQuery('#SelectedLead123').val(leadData);
 
    if (valuesArray != '') {
        jQuery("#btnSendEmail").attr("style", "color: white; display:block;");
    }
    else {
        jQuery("#btnSendEmail").attr("style", "color: white; display:none;");
    }

    if (phoneNoValuesArray != '') {
        jQuery("#btnSendSMS").attr("style", "color: white; display:block;");
    }
    else {
        jQuery("#btnSendSMS").attr("style", "color: white; display:none;");
    }
});

function changeLeadStatus(obj) {
    jQuery.noConflict();
    var currVal = jQuery(obj).val();
    var currText = jQuery(obj).find('option:selected').text();
    jQuery("#ldStatus").val(currVal);
    jQuery("#hdnLeadStatusName").val(currText);
}

/////////////////////facebook upload function//////////////////////////////
function CRMFBLeadUpload() {
    debugger;
    jQuery.noConflict();
    var fileData = new FormData();
    var fileUpload = jQuery("#fbleadfile").get(0);
    var files = fileUpload.files;
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    if (files.length > 0) {
        showLoader();
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Home/CMRFBUploadLead');
        xhr.send(fileData);
        xhr.onreadystatechange = function () {
            if (xhr.responseText.replace('"', '').replace('"', '') == 'Facebook uploaded successfully') {
                jQuery("#divProcessing").hide();
                //swal("Success !", xhr.responseText.replace('"', '').replace('"', ''), "success");
                swal({
                    title: "Success !",
                    text: xhr.responseText.replace('"', '').replace('"', ''),
                    type: "success"
                },
                    function () {
                        location.reload(true);
                    });
            }
            else {
                //swal("Alert !", xhr.responseText.replace('"', '').replace('"', ''), "warning");
                swal({
                    title: "Alert !",
                    text: xhr.responseText.replace('"', '').replace('"', ''),
                    type: "warning"
                },
                    function () {
                        location.reload(true);
                    });
                jQuery("#divProcessing").hide();
            }
            //setTimeout("location.reload(true);", 3000);
        }
    }
    else {
        swal("Alert !", "** Please select file", "warning");
        jQuery("#divProcessing").hide();
    }
}

//////////////////////normal upload funtion//////////////////////////////////
function CRMNormalLeadUpload() {
    jQuery.noConflict();
    var fileData = new FormData();
    var fileUpload = jQuery("#normalleadfile").get(0);
    var files = fileUpload.files;
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    if (files.length > 0) {
        showLoader();
        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Home/CRMNormalUploadLead');
        xhr.send(fileData);
        xhr.onreadystatechange = function () {
            if (xhr.responseText.replace('"', '').replace('"', '') == 'Uploaded Successfully') {
                //swal("Success !", xhr.responseText.replace('"', '').replace('"', ''), "success");
                swal({
                    title: "Success !",
                    text: xhr.responseText.replace('"', '').replace('"', ''),
                    type: "success"
                },
                    function () {
                        location.reload(true);
                    });
                jQuery("#divProcessing").hide();
            }
            else {
                //swal("Alert !", xhr.responseText.replace('"', '').replace('"', ''), "warning");

                swal({
                    title: "Alert !",
                    text: xhr.responseText.replace('"', '').replace('"', ''),
                    type: "warning"
                },
                    function () {
                        location.reload(true);
                    });
                jQuery("#divProcessing").hide();
            }
            //setTimeout("location.reload(true);", 3000);
        }
    }
    else {
        swal("Alert !", "** Please select file", "warning");
        jQuery("#divProcessing").hide();
    }
}

////////////////////////send email funtion////////////////////////////////////
function SendEmail() {
    jQuery.noConflict();
    var fileData = new FormData();
    var fileUpload = jQuery("#AttachmentFile").get(0);
    var files = fileUpload.files;
    for (var i = 0; i < files.length; i++) {
        fileData.append(files[i].name, files[i]);
    }
    //var editorval = CKEDITOR.instances['MessageBody'].getData();
    var editorval = jQuery('#MessageBody').summernote('code');
    var toEmailAddress = jQuery('#ToEmailAddress').val();
    var subject = jQuery('#Subject').val();
    var FileID = jQuery('#FileID').val();
    fileData.append('EmailAddress', toEmailAddress);
    fileData.append('Subject', subject);
    fileData.append('MessageBody', editorval);
    fileData.append('FileID', FileID);
    if (toEmailAddress != '') {
        if (subject != '') {
            if (editorval != '') {
                showLoader();
                jQuery('#btnSendNow').html('Sending..');
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Home/CRMSendNow');
                xhr.send(fileData);
                xhr.onreadystatechange = function () {
                    //if (xhr.readyState == 4 && xhr.status == 200) {
                    if (xhr.responseText.replace('"', '').replace('"', '') == 'Email send successfully.') {
                        swal("Success !", xhr.responseText.replace('"', '').replace('"', ''), "success");
                        jQuery('#modalSendEmail').modal('hide');
                        jQuery("#divProcessing").hide();
                        jQuery('#txtEmailAddress').val('');
                        jQuery('#ToEmailAddress').val('');
                        jQuery('#Subject').val('');
                        //CKEDITOR.instances['MessageBody'].getData('')
                        jQuery('#MessageBody').summernote('code', '');
                        jQuery('.chk').prop('checked', false);
                        jQuery('.chkAll').prop('checked', false);
                        jQuery("#btnSendEmail").attr("style", "color: white; display:none;")
                    }
                    else {
                        jQuery('#btnSendNow').html('Send Now');
                        jQuery("#divProcessing").hide();
                        swal("Alert !", xhr.responseText.replace('"', '').replace('"', ''), "warning");
                    }
                }
                //return false;
            }
            else {
                jQuery('#btnSendNow').html('Send Now');
                jQuery("#divProcessing").hide();
                swal("Alert !", '** Please enter message', "warning");
            }
        }
        else {
            jQuery('#btnSendNow').html('Send Now');
            swal("Alert !", '** Please enter subject', "warning");
        }
    }
    else {
        jQuery('#btnSendNow').html('Send Now');
        swal("Alert !", '** Please enter email address.', "warning");
    }
}

////////////////////////send sms funtion////////////////////////////////////
function SendSMS() {
    jQuery.noConflict();
    var smsData = new FormData();
   
    var toPhoneNumbers = jQuery('#ToContacts').val();
    var message = jQuery('#SMSMsg').val();
    
    smsData.append('PhoneNumbers', toPhoneNumbers);
    smsData.append('TextMessage', message);
    
    if (toPhoneNumbers != '') {
        if (message != '') {
            showLoader();
            jQuery('#btnSendSmsNow').html('Sending..');
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/Home/SendSMS');
            xhr.send(smsData);
            xhr.onreadystatechange = function () {
                //if (xhr.readyState == 4 && xhr.status == 200) {
                if (xhr.responseText.replace('"', '').replace('"', '') == 'SMS send successfully.') {
                    swal("Success !", xhr.responseText.replace('"', '').replace('"', ''), "success");
                    jQuery('#modalSendSMS').modal('hide');
                    jQuery("#divProcessing").hide();                   
                    jQuery('#ToContacts').val('');
                    jQuery('#SMSMsg').val('');
                    jQuery('.chk').prop('checked', false);
                    jQuery('.chkAll').prop('checked', false);
                    jQuery("#btnSendSMS").attr("style", "color: white; display:none;")
                }
                else {
                    jQuery('#btnSendSmsNow').html('Send Now');
                    jQuery("#divProcessing").hide();
                    swal("Alert !", xhr.responseText.replace('"', '').replace('"', ''), "warning");
                }
            }
        }
        else {
            jQuery('#btnSendSmsNow').html('Send Now');
            swal("Alert !", '** Please enter message', "warning");
        }
    }
    else {
        jQuery('#btnSendSmsNow').html('Send Now');
        swal("Alert !", '** Please enter phone number.', "warning");
    }
}

//////////////pop up click funtion////////////////////////////////
jQuery('.Pop').on('click', function () {
    var frmList = jQuery(this).data('name');
    jQuery.post('/common/ProductNameListDisplay', { firmList: frmList }, function (data) {
        jQuery('#modalDoses').html(data);
        jQuery(jQuery(this).data("#modalDoses")).show();
    });
});

/////////////////description view funtion///////////////////////////
jQuery('.PopViewDesc').on('click', function () {
    var self = jQuery(this);
    var leadid = self.data("leadid");
    if (leadid != '') {
        jQuery.post('/home/ViewLeadDecsription', { Lid: leadid }, function (data) {
            jQuery('#ViewDescript').html(data);
            jQuery(jQuery(this).data("#ViewDescript")).show();
        });
    }
});

    ////////////////////////add description click funtion/////////////////////////////

jQuery('.AddDescriptcls').on('click', function () {
        var self = jQuery(this);
        //var txtfollowdt = self.parents().find("#followDateVal").text();
        //var txtleadstName = self.parents().find("#leadStatusVal").text();

          var LeadId = self.attr("data-leadid");
          var leadname = self.attr("data-leadname");
          var leadstatus = self.attr("data-leadstatus");
          var ledStatusId = self.attr("data-leadstatusid");
          var followupdate = self.attr("data-folloupdate");
          //var followupTime = self.attr("data-followuptime");

       
        jQuery('#hdnLeadId').val(LeadId); 
        jQuery("#LeadStatusID").val(ledStatusId).attr("selected", true);
        jQuery("#ldStatus").val(ledStatusId);
        jQuery('#LeadStatusName').html(leadstatus);
        jQuery('#hdnLeadStatusName').val(leadstatus);
        jQuery('#leadname').html(leadname);
        jQuery('#hdnFollowUpDate').val(followupdate);
        //jQuery('#hdnFollowUpTime').val(followupTime);

        jQuery('#FollowUpDate').val(followupdate);
        //jQuery('#FollowupTime').val(followupTime);
        //CKEDITOR.instances['txtDescription'].setData('');
        jQuery('#txtDescription').summernote('code', '');
    });


    ////////////////////////save description funtion/////////////////////////////
function SaveCurrentDescription() {
   
    ///////check description value is empty or not//////////////
    //var value = CKEDITOR.instances["txtDescription"].getData();
    var value = jQuery('#txtDescription').summernote('code');
    if (value == "") {
        swal("Alert !", "Please input a description", "warning");
        return false;
    }

    //get modified date
    //var d = new Date(jQuery.now());
    //var ampm = (d.getHours() >= 12) ? "PM" : "AM";
    //var hours = (d.getHours() >= 12) ? d.getHours() - 12 : d.getHours();
    //var modifiedDate = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear() + " " + hours + ":" + d.getMinutes() + " " + ampm;

    var LeadID = jQuery('#hdnLeadId').val();
    var followUpDate = jQuery('#FollowUpDate').val();
    //var followUpTime = jQuery("#hdnFollowUpTime").val();
    var followUpTime = jQuery("#FollowupTime").val();
    var fileData = new FormData();
    var fileUpload = jQuery("#Postfile").get(0);
    var files = fileUpload.files;

    for (var i = 0; i < files.length; i++)
    {
            fileData.append(files[i].name, files[i]);
    }

    var editorval = value;
    var LeadStatus = jQuery('#hdnLeadStatusName').val();//this is changed  
    var LeadStatusId = jQuery('#ldStatus').val();
        fileData.append('LID', LeadID);
        fileData.append('txtDescription', editorval);
        fileData.append('LeadStatusName', LeadStatus);//this is changed
        fileData.append('LeadStatusId', LeadStatusId);//this is changed
        fileData.append('FollowUpDate', followUpDate);
        fileData.append('FollowUpTime', followUpTime);

        if (LeadID != '') {
            if (editorval != "") {
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Home/ViewLeadAddDescription');
                xhr.send(fileData);
                xhr.onreadystatechange = function () {

                    if (xhr.readyState == 4 && xhr.status == 200)
                    {
                        var data = JSON.parse(xhr.responseText);
                        
                        if (data.Message == "BackFollowupDate")
                        {
                            swal("Alert !", "Please select followup date greater after the day", "warning");
                        }
                        else if (data.Message == "error")
                        {
                            swal("Alert !", '** Somthing went wrong.', "warning");
                        }
                        else if (data.Message == "ok")
                        {
                            swal("Success !", 'Description added succesfully.', "success");

                            //var str = xhr.responseText;
                            //var modifiedDate = str.replace(/^"|"$/g, '');//set modified date and remove double quotes
                            var modifiedDate = data.ModifiedDate;
                            //jQuery('#AddDescript').modal('hide');
                            jQuery('#AddDescript').attr('style', 'display:none');
                            jQuery('#AddDescript').attr('class', 'modal fade');
                            //jQuery('.modal-backdrop').removeClass('modal-backdrop fade in');
                            jQuery(".modal-backdrop").remove();
                            jQuery('body').attr('style', 'padding-right: 0px !important;');
                            jQuery('body').removeClass('modal-open');

                            //find current row in td span to update followupDateTime, fupTime and fupDate
                            var spanElement = jQuery('#' + LeadID).find("td[id='golivedate']").find('span.golivedate');

                            spanElement.attr('data-followupDateTime', data.FollowupDateTime);
                            spanElement.attr('data-fupTime', data.FollowupTime);
                            spanElement.attr('data-isReminder', data.LeadReminder);
                            //spanElement.attr('data-fupDate', data.FollowupDate);

                            //jQuery('#' + LeadID).find("td").eq(5).html(followUpDate + ' ' + followUpTime);//update followup date 
                            //jQuery('#' + LeadID).find("td").eq(11).html(modifiedDate);//update modified date
                            //jQuery('#' + LeadID).find("td").eq(12).html(LeadStatus);//update lead status name

                            jQuery('#' + LeadID).find("td[id='followDateVal']").html(followUpDate);//update followup date;
                            jQuery('#' + LeadID).find("td[id='fupTime']").html(data.FollowupTime);//update followUpTime
                            jQuery('#' + LeadID).find("td[id='modifiedDateVal']").html(modifiedDate);//update modified date
                            jQuery('#' + LeadID).find("td[id='leadStatusVal']").html(LeadStatus);//update lead status name

                            //find current row in td a to update folloupdate, leadstatus, leadstatusId and followuptime
                            var anchorElement = jQuery('#' + LeadID).find("td[id='adesc']").find('a.adesc');
                            anchorElement.attr('data-folloupdate', followUpDate);
                            anchorElement.attr('data-leadstatus', LeadStatus);
                            anchorElement.attr('data-leadstatusId', LeadStatusId);
                            anchorElement.attr('data-followuptime', data.FollowupTime);
                        }
                        else {
                            swal("Alert !", data.Message, "warning");
                        }
                    }
                    else {
                        swal("Alert !", '** Somthing went wrong.', "warning");
                    }
                }
                return false;
            }
            else {
                swal("Alert !", "Please enter description", "warning");
            }
        }
        else {
            swal("Alert !", '** Somthing went wrong.', "warning");
        }
    }



//jQuery('#btnAddDescription').click(function () {
//    var Desc = jQuery('#txtDescription').val();
//    var LeadId = jQuery('#hdnLeadId').val();
//    var value = CKEDITOR.instances["txtDescription"].getData();
//    if (value == "") {
//        swal("Alert !", "Please input a description", "warning");
//        return false;
//    }
//});



jQuery("#dest tbody>tr").click(function () {
    jQuery(this).addClass('selected').siblings().removeClass('selected');
    var value = jQuery(this).find('td:first').html();
});

///Loader on button click open
function showLoader() {
    jQuery("<div id='divProcessing' />").css({
        'position': 'fixed',
        'left': 0,
        'right': 0,
        'bottom': 0,
        'top': 0,
        'background': '#0020ff36',
        'z-index': '99',
        'text-align': 'center'
    }).appendTo(jQuery("body"))
        .append(
            jQuery("<img />").attr("src", "/img/pageloader.gif"),
            jQuery("<p style='margin-top:50px; font-weight:bold;'/>").text('Processing, please wait . . .')
        );
}

