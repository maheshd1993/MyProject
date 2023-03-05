// top Notification
$(document).ready(function(){
	$('#notificationtrigger').click(function(){
	$('.customtoggle').slideToggle();
  });

	$('.container,.menu').click(function(){
	   $('.customtoggle').hide();
	});
	
/* Create Quotation Add row*/
$('#addrow').click(function(){ 
$('#addnewrow').append('<tr><td><span class="count">2</span></td><td><select class="form-control"><option>Status</option></select></td><td></td><td></td><td><input type="text" class="form-control" id="price"></td><td><input type="text" class="form-control" id="inputamt"></td><td class="actiontd"><a class="dltrow deleterow" href="#">Delete</a></td></tr>');	
	
	
	
});

$('.deleterow').click(function(e){
	e.preventDefault();
  $(this).parent().parent().hide();
});
	

/* Services 
/* Add row*/
$('#addrowservice').click(function(){ 
$('#addnewrowservices').append('<tr><td><span class="count">2</span></td><td><select class="form-control"><option>Status</option></select></td><td></td><td></td><td></td><td><input type="text" class="form-control" id="inputamt"></td><td class="actiontd"><a class="dltrow deleterow" href="#">Delete</a></td></tr>');	
	
	
	
});


//Purchase Requisition

/* Add row*/
$('#addrowbtn').click(function(){ 
$('#purchaserequisition').append('<tr><td><span class="count">2</span></td><td><select class="form-control tdselect"><option>Status</option></select></td><td></td><td></td><td><input type="text" class="form-control" id="price"></td><td><input type="text" class="form-control datepicker" id="edu"></td><td><input type="text" class="form-control" id="inputamt"></td><td class="actiontd"><a class="dltrow deleterow" href="#">Delete</a></td></tr>');	
	
	
	
});

// Purchase Order
/* Add row*/
$('#addrowodr').click(function(){ 
$('#purchaseodr').append('<tr><td><span class="count">2</span></td><td><select class="form-control tdselect"><option>Status</option></select></td><td></td><td></td><td><input type="text" class="form-control" id="price"></td><td><input type="text" class="form-control datepicker" id="edu"></td><td><input type="text" class="form-control" id="inputamt"></td><td class="actiontd"><a class="dltrow deleterow" href="#">Delete</a></td></tr>');	
	
	
	
});

$('.deleterow').click(function(e){
	e.preventDefault();
  $(this).parent().parent().hide();
});
	


	
});
// end top notification..............
