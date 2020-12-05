var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    $.fn.dataTable.ext.classes.sPageButton = 'button primary_button btn-sm'; // butoane paginare

    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },

        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) { //data will be the id
                    return `
                        <div class="text-center">
                            <a title="Editare" href="/Admin/Category/Upsert/${data}" class="btn-sm btn-success text-white" style="cursor:pointer; margin-right:16px;"><i class="fas fa-edit"></i></a>

                            <a title="Stergere" onclick=Delete("/Admin/Category/Delete/${data}") class="btn-sm btn-danger text-white" style="cursor:pointer">
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

        "language": {
            "emptyTable": "Nicio inregistrare, Please click on <b>Add New</b> Button",
            "loadingRecords": "Incarcare...",
            "processing": "Procesare...",
            "search": "Cautare:",
            "zeroRecords": "Nu a fost gasita nicio inregistrare",
            "lengthMenu": "Vizualizare _MENU_ inregistrari pe pagina",
            "info": "Vizualizare pagina _PAGE_ din _PAGES_",
            "infoEmpty": "Nicio inregistrare disponibila",
            "infoFiltered": "(filtrate din _MAX_ inregistrari)",
            "paginate": {
                "first": "Primul",
                "last": "Ultimul",
                "next": "Urmator",
                "previous": "Anterior"
            }
        },
        "lengthMenu": [[2, 4, 8, 16, 32, -1], [2, 4, 8, 16, 32, "TOT"]],
        "pageLength": 4
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
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