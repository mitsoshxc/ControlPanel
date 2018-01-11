// Write your JavaScript code.
// Add customer for validation
function AddCustValidate()
{
    var _res = true;

    document.getElementById("customer_name").style.display = "none";
    
    if (document.forms["add_customer"]["_name"].value == "") {
        document.getElementById("customer_name").innerHTML = "Name can not be empty!";
        document.getElementById("customer_name").style.display = "block";
        _res = false;
    }
    
    return _res;
}