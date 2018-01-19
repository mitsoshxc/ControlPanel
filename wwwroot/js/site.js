// Write your JavaScript code.
//
// Customer forms validation
//
function CustomerValidate() {
    var _res = true;

    document.getElementById("customer_name").style.display = "none";

    if (document.forms["customer"]["_name"].value == "") {
        document.getElementById("customer_name").innerHTML = "Customer's name can not be empty!";
        document.getElementById("customer_name").style.display = "block";
        _res = false;
    }

    return _res;
}
//
// Customer Details forms validation
//
function CustDetailsValidate() {
    var _res = true;

    document.getElementById("customerdetails_type").style.display = "none";
    document.getElementById("customerdetails_username").style.display = "none";
    document.getElementById("customerdetails_password").style.display = "none";

    if (document.forms["customerdetails"]["_type"].value == "") {
        document.getElementById("customerdetails_type").innerHTML = "Entry Type can not be empty!";
        document.getElementById("customerdetails_type").style.display = "block";
        _res = false;
    }

    if (document.forms["customerdetails"]["_username"].value == "") {
        document.getElementById("customerdetails_username").innerHTML = "Username can not be empty!";
        document.getElementById("customerdetails_username").style.display = "block";
        _res = false;
    }

    if (document.forms["customerdetails"]["_password"].value == "") {
        document.getElementById("customerdetails_password").innerHTML = "Password can not be empty!";
        document.getElementById("customerdetails_password").style.display = "block";
        _res = false;
    }

    return _res;
}
//
// User forms validation
//
function UserValidation() {
    var _res = true;

    document.getElementById("user_name").style.display = "none";
    document.getElementById("user_rank").style.display = "none";
    document.getElementById("user_pass").style.display = "none";

    if (document.forms["user"]["_name"].value == "") {
        document.getElementById("user_name").innerHTML = "User's name can not be empty!";
        document.getElementById("user_name").style.display = "block";
        _res = false;
    }

    if (document.forms["user"]["_rank"].value == "") {
        document.getElementById("user_rank").innerHTML = "User's rank can not be empty!";
        document.getElementById("user_rank").style.display = "block";
        _res = false;
    }

    if (document.forms["user"]["_pass"].value == "") {
        document.getElementById("user_pass").innerHTML = "User's password can not be empty!";
        document.getElementById("user_pass").style.display = "block";
        _res = false;
    }

    return _res;
}
//
// Customer's Payments Details
//
function PaymentsValidation() {
    var _res = true;

    document.getElementById("customerpayment_type").style.display = "none";
    document.getElementById("customerpayment_date").style.display = "none";
    document.getElementById("customerpayment_amount").style.display = "none";

    if (document.forms["customerpayments"]["_type"].value == "") {
        document.getElementById("customerpayment_type").innerHTML = "Payment's type can not be empty!";
        document.getElementById("customerpayment_type").style.display = "block";
        _res = false;
    }

    if (document.forms["customerpayments"]["_date"].value == "") {
        document.getElementById("customerpayment_date").innerHTML = "Payment's date can not be empty!";
        document.getElementById("customerpayment_date").style.display = "block";
        _res = false;
    }
    else if (/^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/.test(document.forms["customerpayments"]["_date"].value) == false) {
        document.getElementById("customerpayment_date").innerHTML =
            "Payment's date does not seem correnct! 25/10/1990 is a correct format";
        document.getElementById("customerpayment_date").style.display = "block";
        _res = false;
    }

    if (document.forms["customerpayments"]["_amount"].value == "") {
        document.getElementById("customerpayment_amount").innerHTML = "Payment's amount can not be empty!";
        document.getElementById("customerpayment_amount").style.display = "block";
        _res = false;
    }

    return _res;
}