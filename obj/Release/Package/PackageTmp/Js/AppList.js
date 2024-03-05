let tableLocation

$(document).ready(function () {
    tableLocation = $('#table-layer-group').DataTable({
        ajax: '/Home/GetAppList',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'APP_ID',
                mRender: function (data, type, row) {
                    return ` <a onclick="handleEdit('${data}')" style="text-decoration: none">
                       <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                       </a>
                       <button class="btn btn-danger" onclick="handleDelete('${data}')">
                       <i class="fa-solid fa-trash text-light"></i></button>
                       `
                },
                width: '200px',
            },
            {
                mData: 'APP_NAME',
            },
            {
                mData: 'OWNER',
            },
            {
                mData: 'PLATFORM_NAME',
            },
            {
                mData: 'SERVER_NAME',
            },
            {
                mData: 'STATUS',
            },
        ],
    })

    getPlatform();
    getServer();
    getStatus();

});

function getPlatform() {
    $.ajax({
        url: "/Home/getPlatform",
        type: "GET",
        cache: false,
        success: function (result) {
            $('#input_platform').empty();
            var text = '<option></option>'; // Pastikan Anda mendeklarasikan variabel 'text' dengan 'var' atau 'let'
            $.each(result.data, function (key, val) {
                console.log(val.PLATFORM_ID, val.NAME);
                text += '<option value="' + val.PLATFORM_ID + '">' + val.NAME + '</option>';
            });
            $('#input_platform').html(text); // Menambahkan opsi ke dalam <select>

            // Jika Anda menggunakan Bootstrap Select atau library serupa
            // $('#input_platform').selectpicker('refresh');
        },
        error: function (xhr, status, error) {
            // Tambahkan penanganan error di sini jika ajax call gagal
            console.error("Terjadi kesalahan saat memuat platform: ", status, error);
        }
    });
}

function getServer() {
    $.ajax({
        url: "/Home/getServer",
        type: "GET",
        cache: false,
        success: function (result) {
            var text = '<option></option>'; // Pastikan deklarasi 'var' atau 'let' untuk 'text'
            $.each(result.data, function (key, val) {
                console.log(val.SERVER_ID, val.NAME);
                text += '<option value="' + val.SERVER_ID + '">' + val.NAME + '</option>';
            });
            $('#input_server').html(text); // Tambahkan opsi-opsi ke dalam <select>
            // Jika menggunakan Bootstrap Select atau library serupa:
            // $('#input_server').selectpicker('refresh');
        },
        error: function (xhr, status, error) {
            // Tambahkan penanganan error
            console.error("Terjadi kesalahan saat memuat server: ", status, error);
        }
    });
}

function getStatus() {
    $.ajax({
        url: "/Home/getStatus",
        type: "GET",
        cache: false,
        success: function (result) {
            var text = '<option></option>'; // Pastikan deklarasi 'var' atau 'let' untuk 'text'
            $.each(result.data, function (key, val) {
                console.log(val.STATUS_ID, val.NAME);
                text += '<option value="' + val.STATUS_ID + '">' + val.NAME + '</option>';
            });
            $('#input_Status').html(text); // Tambahkan opsi-opsi ke dalam <select>
            // Jika menggunakan Bootstrap Select atau library serupa:
            // $('#input_Status').selectpicker('refresh');
        },
        error: function (xhr, status, error) {
            // Tambahkan penanganan error
            console.error("Terjadi kesalahan saat memuat status: ", status, error);
        }
    });
}

function SaveNewApp() {
    var appname = $("#input_appname");
    var owner = $("#input_owner");
    var platform = $("#input_platform");
    var server = $("#input_server");
    var status = $("#input_Status");
    var version = $("#input_Version");

    var obj = {
        APP_NAME: appname,
        OWNER: owner,
        PLATFORM: platform,
        SERVER: server,
        STATUS: status,
        VERSION: version
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Home/InsertNewApp",
        data: JSON.stringify(obj),
        success: function (response) {
            if (response.Status == true) {

                alert(response.Message);
                window.location.reload();

            } else {

                alert(response.Message);
            }
        }
    });

}