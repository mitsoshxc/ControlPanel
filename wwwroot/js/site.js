﻿// Write your JavaScript code.
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