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
            "url": "/Admin/Company/GetAll"
        },

        "columns": [
            { "data": "name", "width": "20%" },
            { "data": "streetAddress", "width": "15%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "4%", "className": "text-center" },
            { "data": "phoneNumber", "width": "16%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked />`
                    }
                    else {
                        return `<input type="checkbox" disabled/>`
                    }
                },
                "className": "text-center", "width": "8%"
            },
            {
                "data": "id",
                "render": function (data) { //data will be the id
                    return `
                        <div class="text-center">
                            <a title="Editare" href="/Admin/Company/Upsert/${data}" class="btn-sm btn-success text-white" style="cursor:pointer; margin-right:16px;"><i class="fas fa-edit"></i></a>

                            <a title="Stergere" onclick=Delete("/Admin/Company/Delete/${data}") class="btn-sm btn-danger text-white" style="cursor:pointer">
                                <i class="fas fa-trash-alt"></i>
                            </a>
                        </div>
                    `;
                },
                "orderable": false,
                "searchable": false,
                "width": "40%"
            }
        ],

        "language": DTLangOpt,
        "lengthMenu": [[2, 4, 8, 16, 32, -1], [2, 4, 8, 16, 32, "TOT"]],
        "pageLength": 4,
        "pagingType": "full_numbers"
    });
}

function Delete(url) {
    swal(swalDelOpt).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.succesuri) {
                        toastr.success(data.mesaj);
                        dataTable.ajax.reload();
                    }
                    else
                        toastr.error(data.mesaj);
                }
            });
        }
    });
}