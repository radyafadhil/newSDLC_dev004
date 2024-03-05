$(document).ready(function () {
    
});

function GoToLogin() {

    var rememberMe = $('#login-remember-me').is(':checked');
    var username = $("#login-username").val();
    var password = $("#login-password").val();

    console.log(rememberMe)

    var obje = {
        Username: username,
        Password: password,
        rememberMe: rememberMe
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Home/LoginLDAP",
        data: JSON.stringify(obje),
        success: function (response) {
            if (response.Status == true) {
                window.location.href = "/Activity/Index";
            } else {

                alert(response.Message);
            }
        }
    });
}