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
                return `<div class="action-buttons">
                            <a onclick="handleEdit('${data}')" style="text-decoration: none">
                                <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                            </a>

                            <a onclick="handleDelete('${data}')" style="text-decoration: none">
                                <button class="btn btn-danger action-button"><i class="fa-solid fa-trash text-light"></i></button>
                            </a>
                        </div>`
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

        "pagingType": "numbers",
        //"language": {
        //    "paginate": {
        //        "first": "<<",
        //        "last": ">>",
        //        "previous": "<",
        //        "next": ">"
        //    }
        //}
    })


    $(".dataTables_filter").appendTo("#customSearchContainer");

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
                //console.log(val.PLATFORM_ID, val.NAME);
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
                //console.log(val.SERVER_ID, val.NAME);
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
                //console.log(val.STATUS_ID, val.NAME);
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

    var appname = $("#input_appname").val();
    var owner = $("#input_owner").val();
    var platform = $("#input_platform").val();
    var server = $("#input_server").val();
    var status = $("#input_Status").val();
    var version = $("#input_Version").val();

    console.log(appname, owner, platform, server, status, version)

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

                Swal.fire(
                    'Success!',
                    response.Message,
                    'success'
                )
                window.location.reload();

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

function handleDelete(id) {
    Swal.fire({
        title: 'Apakah kamu yakin?',
        text: 'Data akan dihapus ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Ya, Hapus!',
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: `/Home/DeleteAppList?id=${id}`,
                dataType: 'json',
                contentType: 'application/json',
                success: function (response) {

                    if (response.Status === true) {
                        tableLocation.ajax.reload()
                        Swal.fire(
                            'Deleted!',
                            'Data berhasil dihapus.',
                            'success'
                        )
                    } else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                },
            })
        }
    })
}

function handleEdit(id) {
    window.location.href = "/Home/AppDetail?id=" + id
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