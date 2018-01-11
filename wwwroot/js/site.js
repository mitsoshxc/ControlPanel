// Write your JavaScript code.
// Customer forms validation
//
// Add
//
function AddCustValidate() {
    var _res = true;

    document.getElementById("customer_name").style.display = "none";

    if (document.forms["customer"]["_name"].value == "") {
        document.getElementById("customer_name").innerHTML = "Customer's name can not be empty!";
        document.getElementById("customer_name").style.display = "block";
        _res = false;
    }

    return _res;
}