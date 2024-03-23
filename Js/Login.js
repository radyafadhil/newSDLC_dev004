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

                Swal.fire(
                    'Error!',
                    response.Message,
                    'error'
                )
            }
        }
    });
}

const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    },
})