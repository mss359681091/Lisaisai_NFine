// 
$(function(){
 
	$("#log").click(function(){
		$("#sunBj").toggle();
		$("#logBj").toggle();
		$("#regBj").hide();
	});
	$("#reg").click(function(){
		$("#sunBj").toggle();
		$("#regBj").toggle();
		$("#logBj").hide();
	});
	$("#existingAccount").click(function () {
	    $("#pc").toggle();
	    $("#code").toggle();
	    $("#logBj").show();
	    $("#regBj").hide();
	});

	$("#sunBj").click(function(){
		$("#sunBj").hide();
		$("#logBj").hide();
		$("#regBj").hide();
	});

	$("#logChange").click(function(){
			$("#pc").toggle();
			$("#code").toggle();
			$("#logPage").toggle();
			$("#codePage").toggle();

		});
	$("#freeRegistration").click(function () {
	    $("#pc").toggle();
	    $("#code").toggle();
		$("#logBj").hide();
		$("#regBj").show();

	});
})