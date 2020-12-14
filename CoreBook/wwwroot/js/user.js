var dataTable;

var DTLangOpt = {
    "emptyTable": "Nicio inregistrare, Please click on <b>Create New</b> Button",
    "loadingRecords": "Incarcare...",
    "processing": "Procesare...",
    "search": "Cautare:",
    "zeroRecords": "Nu a fost gasita nicio inregistrare",
    "lengthMenu": "Vizualizare _MENU_ inregistrari pe pagina",
    "info": "Vizualizare pagina _PAGE_ din _PAGES_",
    "infoEmpty": "Nicio inregistrare disponibila",
    "infoFiltered": "(filtrate din _MAX_ inregistrari)",
    "paginate": {
        "first": "Prim",
        "previous": "Anterior",
        "next": "Urmator",
        "last": "Ultim"
    }
};

var swalDelOpt = { title: "Are you sure you want to Delete?", text: "You will not be able to restore this data!", icon: "warning", buttons: true, dangerMode: true };


$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    $.fn.dataTable.ext.classes.sPageButton = 'button primary_button btn-sm'; // butoane paginare

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },

        "columns": [
            { "data": "name", "width": "16%" },
            { "data": "email", "width": "16%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "20%" },
            { "data": "role", "width": "15%" },

            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        // user is currently locked
                        return `
                            <div class="text-center">
                                <a title="Unlock" onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:80%;">
                                    <i class="fas fa-lock-open"></i> Unlock
                                </a>
                            </div>
                        `
                    }
                    else {
                        return `
                            <div class="text-center">
                                <a title="Unlock" onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:80%;">
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                            </div>
                        `
                    }
                },
                "orderable": false,
                "searchable": false,
                "width": "24%"
            }
        ],

        "language": DTLangOpt,
        "lengthMenu": [[2, 4, 8, 16, 32, -1], [2, 4, 8, 16, 32, "TOT"]],
        "pageLength": 4,
        "pagingType": "full_numbers"
    });
}

function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id), // data passed
        contentType: "application/json",
        success: function (data) {
            if (data.scc) {
                toastr.success(data.msj);
                dataTable.ajax.reload();
            }
            else
                toastr.error(data.msj);
        }
    });
}