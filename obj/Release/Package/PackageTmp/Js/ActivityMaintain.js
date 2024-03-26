$(document).ready(function () {
    getApp();
    getStep();
    //getValDefault();

    $('#block-form-app').on('change', function () {
        loadVersion();
    });
});

function getApp() {
    $.ajax({
        url: "/Activity/GetAppName",
        type: "GET",
        cache: false,
        success: function (result) {
            $('#block-form-app').empty();
            text = '<option></option>';
            $.each(result.data, function (key, val) {
                text += '<option value="' + val.APP_ID + '" data-role="' + val.APP_NAME + '">' + val.APP_NAME + '</option>';
            });

            $("#block-form-app").append(text).select2({
                placeholder: "",
                allowClear: true,
                width: '100%' // Menyesuaikan lebar dengan container jika diperlukan
            });
        }
    });
}

function getStep() {
    $.ajax({
        url: "/Activity/GetStep",
        type: "GET",
        cache: false,
        success: function (result) {
            $('#block-form-step').empty();
            text = '<option></option>';
            $.each(result.data, function (key, val) {
                text += '<option value="' + val.STEP_ID + '" data-role="' + val.NAME + '">' + val.NAME + '</option>';
            });

            $("#block-form-step").append(text);
        }
    });
}

function getValDefault() {
    var appid = $("#block-form-appidGet").val()
    console.log("masuk appid : " + appid)

    if (appid != "") {
        var actdetail = $("#block-form-actdetailGet").val()
        var remarks = $("#block-form-remarkGet").val()
        var versionid = $("#block-form-versionidGet").val()
        var stepid = $("#block-form-stepidGet").val()

        console.log(actdetail, remarks, versionid, stepid, appid)

        $("#block-form-actdetail").val(actdetail)
        $("#block-form-remarks").val(remarks)
        $("#block-form-app").val("LANGSIR APP")
        $("#block-form-version").val(versionid)
        $("#block-form-step").val(stepid)
    }
}

function loadVersion() {

    $.ajax({
        url: "/Activity/GetVersion?appId=" + $('#block-form-app').val(),
        type: "GET",
        cache: false,
        success: function (result) {
            $('#block-form-version').empty();
            text = '<option></option>';
            $.each(result.data, function (key, val) {
                text += '<option value="' + val.VERSION_ID + '" data-role="' + val.VERSION + '">' + val.VERSION + '</option>';
            });

            console.log(text);
            $("#block-form-version").append(text);
        }
    });
}

function CRUD_Activity() {

    var txtBtn = $("#block-btn-state").text()
    var actId = $("#block-form-actid").val()

    var app = "";
    var version = "";
    var step = "";

    //debugger
    var actDetail = "";
    var getActVal = $("#block-form-actdetail").val()
    if (getActVal === undefined) {
        actDetail = $("#block-form-actdetailTxt").val();
    }

    else {
        actDetail = $("#block-form-actdetail").val();
    }

    var remark = ""
    var getRemarkVal = $("#block-form-remarks").val()
    if (getRemarkVal === undefined) {
        remark = $("#block-form-remarksTxt").val();
    }

    else {
        remark = $("#block-form-remarks").val();
    }

    var appname = "";
    var getAppname = $("#block-form-app").val();
    if (getAppname === undefined) {
        appname = $("#block-form-appTxt").val();
    }

    else {
        appname = $("#block-form-app").val();
    }

    var version = "";
    var getVersion = $("#block-form-version").val();
    if (getVersion === undefined) {
        version = $("#block-form-versionTxt").val();
    }

    else {
        version = $("#block-form-version").val();
    }

    var step = "";
    var getStep = $("#block-form-step").val();
    if (getStep === undefined) {
        step = $("#block-form-stepTxt").val();
    }

    else {
        step = $("#block-form-step").val();
    }

    var pic = $("#block-form-picid").val();

    var obj = {
        statusBtn: txtBtn,
        ACTIVITY_ID: actId,
        ACTIVITY_DETAIL: actDetail,
        REMARKS: remark,
        APP_ID: appname,
        VERSION_ID: version,
        STEP_ID: step,
        PIC_ID: pic
    }

    //console.log(obj)

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Activity/CRUDActivity",
        data: JSON.stringify(obj),
        success: function (response) {
            if (response.Status == true) {

                Swal.fire(
                    'Success!',
                    response.Message,
                    'success'
                ).then((result) => {
                    if (result.isConfirmed) {
                        window.location.reload();
                    }
                })


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