let tableLocation
let tableVersion
let tableDatabase

$(document).ready(function () {
    getPlatform()
    getServer();
    getStatus();
    getDataById()
    getListDocType()

    tableLocation = $('#table-layer-modal_doc').DataTable({
        ajax: '/Home/getListDocument',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'DOC_ID',
                mRender: function (data, type, row) {
                    return ` <a onclick="handleEditDocList('${data}')" style="text-decoration: none">
                       <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                       </a>
                       <button class="btn btn-danger" onclick="handleDeleteDocList('${data}')">
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
                    return ` <a onclick="handleEditVersionList('${data}')" style="text-decoration: none">
                       <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                       </a>
                       <button class="btn btn-danger" onclick="handleDeleteVersionList('${data}')">
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
                    return ` <a onclick="handleEditDBList('${data}')" style="text-decoration: none">
                       <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                       </a>
                       <button class="btn btn-danger" onclick="handleDeleteDBList('${data}')">
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
        
});

function handleDeleteDocList(id) {
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
                url: `/Home/DeleteDocList?id=${id}`,
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
                        Toast.fire({
                            icon: 'error',
                            title: response.Message,
                        })
                    }
                },
            })
        }
    })
}

function handleEditDocList(id) {
    $("#txtDOCID").val(id)

    $.ajax({
        type: 'GET',
        url: `/Home/GetDataDOCLIST?id=` + id,
        dataType: 'json',
        success: function (response) {
            if (response.Status) {
                $("#txtVersion").val(response.data.VERSION)
                $("#txtReleaseType").val(returnDate(response.data.RELEASE_DATE));
                $("#ddl_docType").val(response.data.DOC_TYPE_ID)
                $("#btnDynamicDOC").text("Update")
            } else {
                window.location.reload();
            }
        },
    })

}

function handleDeleteVersionList(id) {
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
                url: `/Home/DeleteVersionList?id=${id}`,
                dataType: 'json',
                contentType: 'application/json',
                success: function (response) {

                    if (response.Status === true) {
                        tableVersion.ajax.reload()
                        Swal.fire(
                            'Deleted!',
                            'Data berhasil dihapus.',
                            'success'
                        )
                    } else {
                        Toast.fire({
                            icon: 'error',
                            title: response.Message,
                        })
                    }
                },
            })
        }
    })
}

function handleEditVersionList(id) {
    $("#txtVERSIONID").val(id)

    $.ajax({
        type: 'GET',
        url: `/Home/GetDataVERSIONLIST?id=` + id,
        dataType: 'json',
        success: function (response) {
            if (response.Status) {
                $("#txtVersion_verslist").val(response.data.VERSION)
                $("#txtReleaseDate").val(returnDate(response.data.DATE));
                $("#btnDynamicVERSION").text("Update")
            } else {
                window.location.reload();
            }
        },
    })

}

function handleDeleteDBList(id) {
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
                url: `/Home/DeleteDatabaseList?id=${id}`,
                dataType: 'json',
                contentType: 'application/json',
                success: function (response) {

                    if (response.Status === true) {
                        tableDatabase.ajax.reload()
                        Swal.fire(
                            'Deleted!',
                            'Data berhasil dihapus.',
                            'success'
                        )
                    } else {
                        Toast.fire({
                            icon: 'error',
                            title: response.Message,
                        })
                    }
                },
            })
        }
    })
}

function handleEditDBList(id) {
    $("#txtDBID").val(id)

    $.ajax({
        type: 'GET',
        url: `/Home/GetDataDBLIST?id=` + id,
        dataType: 'json',
        success: function (response) {
            if (response.Status) {
                $("#txtName").val(response.data.NAME)
                $("#btnDynamicDB").text("Update")
            } else {
                window.location.reload();
            }
        },
    })

}

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
    var version = $("#txtVersion").val();
    var releasetype = $("#txtReleaseType").val();
    var ddldoctype = $("#ddl_docType").val();

    var obj = {
        VERSION_DOCLIST: version,
        RELEASEDATE_DOCLIST: releasetype,
        DOCTYPE_DOCLIST: ddldoctype
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Home/InsertNewDocList",
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

function saveVersionNew() {
    var version = $("#txtVersion_verslist").val();
    var releasedate = $("#txtReleaseDate").val();

    var obj = {
        VERSION_VERSIONLIST: version,
        DATE_VERSIONLIST: releasedate
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Home/InsertNewVersionList",
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

function saveDatabaseList() {
    var namedb = $("#txtName").val();

    var obj = {
        NAME_DBLIST: namedb
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Home/InsertNewDatabaseList",
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

function returnDate(dateString) {
    // Misal response.data.DATE = "/Date(1709830800000)/";
    //var dateString = response.data.DATE;

    // Ekstrak timestamp dari string dan konversi ke integer
    var timestamp = parseInt(dateString.match(/\d+/)[0]);

    // Buat objek Date baru dengan timestamp tersebut
    var date = new Date(timestamp);

    // Format tanggal ke yyyy-MM-dd tanpa konversi ke UTC
    var year = date.getFullYear();
    // getMonth() mengembalikan bulan dari 0-11, tambahkan 1 untuk mendapatkan format bulan yang benar
    var month = ('0' + (date.getMonth() + 1)).slice(-2);
    var day = ('0' + date.getDate()).slice(-2);

    var formattedDate = year + '-' + month + '-' + day;

    return formattedDate
}