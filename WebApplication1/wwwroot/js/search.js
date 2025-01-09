document.addEventListener('load', function () {
	var item = localStorage.getItem('productTypeId');
	var select = document.getElementById("productTypeId");
	select.value = item;
});

function submitForm() {
	var select = document.getElementById("productTypeId");
	var value = select.options[select.selectedIndex].value;
	localStorage.setItem('productTypeId', value);
	document.forms["myform"].submit()
}
