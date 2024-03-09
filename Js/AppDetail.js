let tableLocation
let tableVersion
let tableDatabase

$(document).ready(function () {
    tableLocation = $('#table-layer-modal_doc').DataTable({
        ajax: '/Home/getListDocument',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'DOC_ID',
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
                mData: 'VERSION',
            },
            {
                mData: 'RELEASE_DATE',
                mRender: function (data, type, row) {
                    if (type === 'display' && data) {
                        var date = new Date(parseInt(data.match(/\d+/)[0])); // Konversi string /Date(12345678)/ menjadi Date
                        var formattedDate = date.getFullYear() + '-' +
                            ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
                            ('0' + date.getDate()).slice(-2); // Format menjadi yyyy-MM-dd
                        return formattedDate;
                    }
                    return data; // Untuk tipe lain seperti sorting, dll, kembalikan data asli
                }
            },
            {
                mData: 'DOC_TYPE',
            },
        ],
    })

    tableVersion = $('#table-layer-modal_version').DataTable({
        ajax: '/Home/getListVersion',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'VERSION_ID',
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
                mData: 'VERSION',
            },
            {
                mData: 'DATE',
                mRender: function (data, type, row) {
                    if (type === 'display' && data) {
                        var date = new Date(parseInt(data.match(/\d+/)[0])); // Konversi string /Date(12345678)/ menjadi Date
                        var formattedDate = date.getFullYear() + '-' +
                            ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
                            ('0' + date.getDate()).slice(-2); // Format menjadi yyyy-MM-dd
                        return formattedDate;
                    }
                    return data; // Untuk tipe lain seperti sorting, dll, kembalikan data asli
                }
            },
        ],
    })

    tableDatabase = $('#table-layer-modal_database').DataTable({
        ajax: '/Home/getListDatabase',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'DB_ID',
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
                mData: 'NAME',
            }
        ],
    })

    getPlatform()        
    getServer();
    getStatus();    
    getDataById()
    getListDocType()

});

function getPlatform() {
    $.ajax({
        url: "/Home/getPlatform",
        type: "GET",
        cache: false,
        success: function (result) {
            $('#modify_platform').empty();
            var text = '<option></option>'; // Pastikan Anda mendeklarasikan variabel 'text' dengan 'var' atau 'let'
            $.each(result.data, function (key, val) {

                text += '<option value="' + val.PLATFORM_ID + '">' + val.NAME + '</option>';
            });
            $('#modify_platform').html(text);
        },
        error: function (xhr, status, error) {
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
            $('#modify_server').html(text); // Tambahkan opsi-opsi ke dalam <select>
            // Jika menggunakan Bootstrap Select atau library serupa:
            // $('#modify_server').selectpicker('refresh');
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

                text += '<option value="' + val.STATUS_ID + '">' + val.NAME + '</option>';
            });
            $('#modify_Status').html(text); // Tambahkan opsi-opsi ke dalam <select>
            // Jika menggunakan Bootstrap Select atau library serupa:
            // $('#modify_Status').selectpicker('refresh');
        },
        error: function (xhr, status, error) {
            // Tambahkan penanganan error
            console.error("Terjadi kesalahan saat memuat status: ", status, error);
        }
    });
}

function getDataById() {
    $.ajax({
        url: "/Home/getDetailApp",
        type: "GET",
        cache: false,
        success: function (result) {

            if (result.Status == true) {

               // console.log(result.data.SERVER_ID, result.data.STATUS_ID)

                $("#modify_appname").val(result.data.APP_NAME);
                $("#modify_owner").val(result.data.OWNER);
                $("#modify_platform").val(result.data.PLATFORM_ID.toLowerCase());
                $("#modify_server").val(result.data.SERVER_ID.toLowerCase());
                $("#modify_Status").val(result.data.STATUS_ID.toLowerCase());
            }
        },
        error: function (xhr, status, error) {
            // Tambahkan penanganan error
            console.error("Terjadi kesalahan saat memuat status: ", status, error);
        }
    });
}

function getListDocType() {
    $.ajax({
        url: "/Home/getListDocType",
        type: "GET",
        cache: false,
        success: function (result) {
            var text = '<option></option>'; // Pastikan deklarasi 'var' atau 'let' untuk 'text'
            $.each(result.data, function (key, val) {

                text += '<option value="' + val.DOC_TYPE_ID + '">' + val.TYPE + '</option>';
            });
            $('#ddl_docType').html(text); // Tambahkan opsi-opsi ke dalam <select>
            // Jika menggunakan Bootstrap Select atau library serupa:
            // $('#modify_Status').selectpicker('refresh');
        },
        error: function (xhr, status, error) {
            // Tambahkan penanganan error
            console.error("Terjadi kesalahan saat memuat status: ", status, error);
        }
    });
}

function saveDocList() {
    var server = $("#txtServer").val();
    var remark = $("#txtRemark").val();
}

function saveVersionNew() {
    var appname = $("#input_appname").val();
    var owner = $("#input_owner").val();
}

function saveDatabaseList() {
    var appname = $("#input_appname").val();
    var owner = $("#input_owner").val();
}


//function SaveNewApp() {

//    var appname = $("#input_appname").val();
//    var owner = $("#input_owner").val();
//    var platform = $("#input_platform").val();
//    var server = $("#input_server").val();
//    var status = $("#input_Status").val();
//    var version = $("#input_Version").val();

//    console.log(appname, owner, platform, server, status, version)

//    var obj = {
//        APP_NAME: appname,
//        OWNER: owner,
//        PLATFORM: platform,
//        SERVER: server,
//        STATUS: status,
//        VERSION: version
//    }

//    $.ajax({
//        type: "POST",
//        dataType: "json",
//        contentType: "application/json",
//        url: "/Home/InsertNewApp",
//        data: JSON.stringify(obj),
//        success: function (response) {
//            if (response.Status == true) {

//                alert(response.Message);
//                window.location.reload();

//            } else {

//                alert(response.Message);
//            }
//        }
//    });

//}

//function handleDelete(id) {
//    Swal.fire({
//        title: 'Apakah kamu yakin?',
//        text: 'Data akan dihapus ?',
//        icon: 'warning',
//        showCancelButton: true,
//        confirmButtonColor: '#3085d6',
//        cancelButtonColor: '#d33',
//        confirmButtonText: 'Ya, Hapus!',
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                type: 'POST',
//                url: `/Home/DeleteAppList?id=${id}`,
//                dataType: 'json',
//                contentType: 'application/json',
//                success: function (response) {

//                    if (response.Status === true) {
//                        tableLocation.ajax.reload()
//                        Swal.fire(
//                            'Deleted!',
//                            'Data berhasil dihapus.',
//                            'success'
//                        )
//                    } else {
//                        Toast.fire({
//                            icon: 'error',
//                            title: response.Message,
//                        })
//                    }
//                },
//            })
//        }
//    })
//}

//function handleEdit(id) {
//    window.location.href = "/Home/AppDetail?id=" + id
//}