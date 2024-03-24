let tableLocation
let tableVersion
let tableDatabase

$(document).ready(function () {
    //runFunctions();
    //getPlatform();
    //getServer();
    //getStatus();
    //getDataById();

    getPlatform()
        .then(() => getServer())
        .then(() => getStatus())
        .then(() => getDataById())
        .catch((error) => {
            console.error("Error:", error);
        });

    getListDocType();

    tableLocation = $('#table-layer-modal_doc').DataTable({
        ajax: '/Home/getListDocument',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'DOC_ID',
                //mRender: function (data, type, row) {
                //    return `<div class="action-buttons">
                //            <a onclick="handleEditDocList('${data}')" style="text-decoration: none">
                //                <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                //            </a>
                //            <a onclick="handleDeleteDocList('${data}')" style="text-decoration: none">
                //                <button class="btn btn-danger action-button"><i class="fa-solid fa-trash text-light"></i></button>
                //            </a>
                //        </div>`
                //},

                mRender: function (data, type, row) {
                    return `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-sm btn-warning" data-bs-toggle="tooltip" title="Edit" onclick="handleEditDocList('${data}')">
                                    <i class="fa fa-pencil-alt"></i>
                                </button>
                                <button type="button" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Delete" onclick="handleDeleteDocList('${data}')">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>`;
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
        "searching": false
    })

    tableVersion = $('#table-layer-modal_version').DataTable({
        ajax: '/Home/getListVersion',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'VERSION_ID',
                //mRender: function (data, type, row) {
                //    return `<div class="action-buttons">
                //            <a onclick="handleEditVersionList('${data}')" style="text-decoration: none">
                //                <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                //            </a>
                //            <a onclick="handleDeleteVersionList('${data}')" style="text-decoration: none">
                //                <button class="btn btn-danger action-button"><i class="fa-solid fa-trash text-light"></i></button>
                //            </a>
                //        </div>`
                //},

                mRender: function (data, type, row) {
                    return `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-sm btn-warning" data-bs-toggle="tooltip" title="Edit" onclick="handleEditVersionList('${data}')">
                                    <i class="fa fa-pencil-alt"></i>
                                </button>
                                <button type="button" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Delete" onclick="handleDeleteVersionList('${data}')">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>`;
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
        "searching": false
    })

    tableDatabase = $('#table-layer-modal_database').DataTable({
        ajax: '/Home/getListDatabase',
        dataSrc: 'data',
        scrollY: '48vh',
        columns: [
            {
                mData: 'DB_ID',
                //mRender: function (data, type, row) {
                //    return `<div class="action-buttons">
                //            <a onclick="handleEditDBList('${data}')" style="text-decoration: none">
                //                <button class="btn btn-warning"><i class="fa-solid fa-pen text-light"></i></button>
                //            </a>
                //            <a onclick="handleDeleteDBList('${data}')" style="text-decoration: none">
                //                <button class="btn btn-danger action-button"><i class="fa-solid fa-trash text-light"></i></button>
                //            </a>
                //        </div>`
                //},

                mRender: function (data, type, row) {
                    return `<div class="btn-group" role="group" aria-label="Basic example">
                                <button type="button" class="btn btn-sm btn-warning" data-bs-toggle="tooltip" title="Edit" onclick="handleEditDBList('${data}')">
                                    <i class="fa fa-pencil-alt"></i>
                                </button>
                                <button type="button" class="btn btn-sm btn-danger" data-bs-toggle="tooltip" title="Delete" onclick="handleDeleteDBList('${data}')">
                                    <i class="fa fa-times"></i>
                                </button>
                            </div>`;
                },

                width: '200px',
            },
            {
                mData: 'NAME',
            }
        ],
        "searching": false
    })
        
});

async function runFunctions() {
    try {
        await getPlatform();
        await getServer();
        await getStatus();
        getDataById();
    } catch (error) {
        console.error("Error:", error);
    }
}

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
                        if (response.Message == "logout") {
                            window.location.href = "/Home/Login"
                        }

                        else {
                            Swal.fire(
                                'Error!',
                                response.Message,
                                'error'
                            )
                        }
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
                if (response.Message == "logout") {
                    window.location.href = "/Home/Login"
                }

                else {
                    Swal.fire(
                        'Error!',
                        response.Message,
                        'error'
                    )
                }
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
                        if (response.Message == "logout") {
                            window.location.href = "/Home/Login"
                        }

                        else {
                            Swal.fire(
                                'Error!',
                                response.Message,
                                'error'
                            )
                        }
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
                if (response.Message == "logout") {
                    window.location.href = "/Home/Login"
                }

                else {
                    Swal.fire(
                        'Error!',
                        response.Message,
                        'error'
                    )
                }
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
                        if (response.Message == "logout") {
                            window.location.href = "/Home/Login"
                        }

                        else {
                            Swal.fire(
                                'Error!',
                                response.Message,
                                'error'
                            )
                        }
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
                if (response.Message == "logout") {
                    window.location.href = "/Home/Login"
                }

                else {
                    Swal.fire(
                        'Error!',
                        response.Message,
                        'error'
                    )
                }
            }
        },
    })

}

function getPlatform() {
    return new Promise((resolve, reject) => {

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

        setTimeout(() => {
            console.log("getPlatform done");
            resolve();
        }, 1000); // Contoh timeout 1000 ms (1 detik)
    });
    console.log("platform");
    
}

function getServer() {
    console.log("server");
    return new Promise((resolve, reject) => {

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

        setTimeout(() => {
            console.log("getServer done");
            resolve();
        }, 1000); // Contoh timeout 1000 ms (1 detik)
    });
    
}

function getStatus() {
    console.log("status");
    return new Promise((resolve, reject) => {

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

        setTimeout(() => {
            console.log("getStatus done");
            resolve();
        }, 1000); // Contoh timeout 1000 ms (1 detik)
    });
    
}

function getDataById() {
    $.ajax({
        url: "/Home/getDetailApp",
        type: "GET",
        cache: false,
        success: function (result) {
            console.log(result.data);
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

    var docid = $("#txtDOCID").val();
    var version = $("#txtVersion").val();
    var releasetype = $("#txtReleaseType").val();
    var ddldoctype = $("#ddl_docType").val();

    if (docid != "") {
        var obj = {
            DOC_ID: docid,
            VERSION_DOCLIST: version,
            RELEASEDATE_DOCLIST: releasetype,
            DOCTYPE_DOCLIST: ddldoctype
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "/Home/UpdateDocList",
            data: JSON.stringify(obj),
            success: function (response) {
                if (response.Status == true) {

                    tableLocation.ajax.reload()
                    Swal.fire(
                        'Success!',
                        response.Message,
                        'success'
                    )

                    $("#txtDOCID").val("");
                    $("#txtVersion").val("")
                    $("#txtReleaseType").val("");
                    $("#ddl_docType").val("")
                    $("#btnDynamicDOC").text("Simpan")

                } else {

                    if (response.Message == "logout") {
                        window.location.href = "/Home/Login"
                    }

                    else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                }
            }
        });
    }

    else {
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

                    tableLocation.ajax.reload()
                    Swal.fire(
                        'Success!',
                        response.Message,
                        'success'
                    )

                    $("#txtDOCID").val("");
                    $("#txtVersion").val("")
                    $("#txtReleaseType").val("");
                    $("#ddl_docType").val("")
                    $("#btnDynamicDOC").text("Simpan")

                } else {

                    if (response.Message == "logout") {
                        window.location.href = "/Home/Login"
                    }

                    else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                }
            }
        });
    }
}

function saveVersionNew() {

    var versionid = $("#txtVERSIONID").val();
    var version = $("#txtVersion_verslist").val();
    var releasedate = $("#txtReleaseDate").val();

    if (versionid != "") {
        var obj = {
            VERSION_ID: versionid,
            VERSION_VERSIONLIST: version,
            DATE_VERSIONLIST: releasedate
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "/Home/UpdateVersionList",
            data: JSON.stringify(obj),
            success: function (response) {
                if (response.Status == true) {

                    tableVersion.ajax.reload()
                    Swal.fire(
                        'Success!',
                        response.Message,
                        'success'
                    )

                    $("#txtVERSIONID").val("")
                    $("#txtVersion_verslist").val("")
                    $("#txtReleaseDate").val("");
                    $("#btnDynamicVERSION").text("Simpan")

                } else {

                    if (response.Message == "logout") {
                        window.location.href = "/Home/Login"
                    }

                    else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                }
            }
        });
    }

    else {
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

                    tableVersion.ajax.reload()
                    Swal.fire(
                        'Success!',
                        response.Message,
                        'success'
                    )

                    $("#txtVERSIONID").val("")
                    $("#txtVersion_verslist").val("")
                    $("#txtReleaseDate").val("");
                    $("#btnDynamicVERSION").text("Simpan")

                } else {

                    if (response.Message == "logout") {
                        window.location.href = "/Home/Login"
                    }

                    else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                }
            }
        });
    }

}

function saveDatabaseList() {

    var dbid = $("#txtDBID").val();
    var namedb = $("#txtName").val();

    if (dbid != "") {
        var obj = {
            DB_ID: dbid,
            NAME_DBLIST: namedb
        }

        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: "/Home/UpdateDatabaseList",
            data: JSON.stringify(obj),
            success: function (response) {
                if (response.Status == true) {

                    tableDatabase.ajax.reload()
                    Swal.fire(
                        'Success!',
                        response.Message,
                        'success'
                    )

                    $("#txtDBID").val("")
                    $("#txtName").val("")
                    $("#btnDynamicDB").text("Simpan")

                } else {

                    if (response.Message == "logout") {
                        window.location.href = "/Home/Login"
                    }

                    else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                }
            }
        });
    }

    else {
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

                    tableDatabase.ajax.reload()
                    Swal.fire(
                        'Success!',
                        response.Message,
                        'success'
                    )

                    $("#txtDBID").val("")
                    $("#txtName").val("")
                    $("#btnDynamicDB").text("Simpan")

                } else {

                    if (response.Message == "logout") {
                        window.location.href = "/Home/Login"
                    }

                    else {
                        Swal.fire(
                            'Error!',
                            response.Message,
                            'error'
                        )
                    }
                }
            }
        });
    }
}

function resetDocList() {
    $("#txtDOCID").val("");
    $("#txtVersion").val("")
    $("#txtReleaseType").val("");
    $("#ddl_docType").val("")
    $("#btnDynamicDOC").text("Simpan")
}

function resetVersionList() {
    $("#txtVERSIONID").val("")
    $("#txtVersion_verslist").val("")
    $("#txtReleaseDate").val("");
    $("#btnDynamicVERSION").text("Simpan")
}

function resetDatabaseList() {
    $("#txtDBID").val("")
    $("#txtName").val("")
    $("#btnDynamicDB").text("Simpan")
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

function UpdateApp() {
    var appname = $("#modify_appname").val();
    var owner = $("#modify_owner").val();
    var platform = $("#modify_platform").val();
    var server = $("#modify_server").val();
    var status = $("#modify_Status").val();

    var obj = {
        APP_NAME: appname,
        OWNER: owner,
        PLATFORM: platform,
        SERVER: server,
        STATUS: status
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/json",
        url: "/Home/UpdateAppDetail",
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

                if (response.Message == "logout") {
                    window.location.href = "/Home/Login"
                }

                else {
                    Swal.fire(
                        'Error!',
                        response.Message,
                        'error'
                    )
                }
            }
        }
    });

}