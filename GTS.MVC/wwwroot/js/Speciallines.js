// =====================================================
// Special Lines
// =====================================================

window.selectedLines = window.selectedLines || new Set();
var selectedLines = window.selectedLines;

var tableSpecialLines = null;
var specialLinesCurrentPage = 0;

var SPECIAL_LINES_DT_OPTIONS = {
    paging: true,
    searching: true,
    lengthChange: true,
    ordering: true,
    pageLength: 10,
    lengthMenu: [10, 25, 50],
    order: [[0, 'desc'], [1, 'asc']], // Initial load: Selected items on top
    columnDefs: [
        { targets: 0, orderDataType: 'dom-checkbox' },
        { targets: '_all', orderable: true }
    ],
    dom: "<'dt-top d-flex justify-content-between align-items-center mb-2'" +
        "<'dt-search'f>" +
        "<'dt-controls d-flex align-items-center gap-3'" +
        "<'dt-length'l>" +
        "<'dt-pagination'p>" +
        ">" +
        ">" +
        "rt" +
        "<'dt-bottom d-flex justify-content-between align-items-center mt-2'<'dt-info'i><'dt-pagination'p>>",
    language: {
        emptyTable: "No Special Lines found.",
        info: "Showing _START_ to _END_ of _TOTAL_ records",
        lengthMenu: "_MENU_ entries per page",
        paginate: {
            previous: "Previous",
            next: "Next",
            first: "«",
            last: "»"
        },
        search: "Search:",
        searchPlaceholder: ""
    }
};

$.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
        return $('input[type="checkbox"]', td).prop('checked') ? '1' : '0';
    });
};

function initSpecialLinesGrid() {
    var tbl = $('#tblSpecialLines');

    if (tbl.length && $.fn.DataTable) {
        $.fn.dataTable.ext.errMode = 'none';

        if ($.fn.DataTable.isDataTable(tbl)) {
            tbl.DataTable().destroy();
        }

        tableSpecialLines = tbl.DataTable(SPECIAL_LINES_DT_OPTIONS);

        tbl.off('page.dt').on('page.dt', function () {
            specialLinesCurrentPage = tableSpecialLines.page();
        });

        tbl.off('search.dt').on('search.dt', function () {
            var bottomBar = document.querySelector('.dt-bottom');
            if (bottomBar) {
                bottomBar.style.display = tableSpecialLines.rows({ search: 'applied' }).count() > 0 ? '' : 'none';
            }
        });

        // Retain checkbox status when paging or filtering
        tbl.off('draw.dt').on('draw.dt', function () {
            $(".chkSpecialLine").each(function () {
                var line = parseInt($(this).data("line"));
                $(this).prop("checked", selectedLines.has(line));
            });
        });
    }
}

$(document)
    .off("click", "#btnSpecialLines")
    .on("click", "#btnSpecialLines", function () {
        selectedLines.clear();
        loadSpecialLines();
    });

function loadSpecialLines() {
    var customer = typeof getSelectedCustomer === "function" ? getSelectedCustomer() : null;
    if (!customer) {
        alert("Please select a customer first.");
        return;
    }

    $.get("/CustomerProfile/SpecialLines", { custId: customer.CustId }, function (html) {
        var $target = $("#workspace").length ? $("#workspace") : $("#CustomerModuleContainer");
        $target.html(html);

        $(".chkSpecialLine:checked").each(function () {
            selectedLines.add(parseInt($(this).data("line")));
        });

        initSpecialLinesGrid();
        bindSpecialLineEvents();
    }).fail(function () {
        alert("Unable to load Special Lines.");
    });
}

function bindSpecialLineEvents() {
    // Only update the Set and data-order without triggering a table sort/redraw
    $(document)
        .off("change", ".chkSpecialLine")
        .on("change", ".chkSpecialLine", function () {
            var line = parseInt($(this).data("line"));
            var isChecked = $(this).is(":checked");
            var $td = $(this).closest("td");

            if (isChecked) {
                selectedLines.add(line);
                $td.attr("data-order", "1");
            } else {
                selectedLines.delete(line);
                $td.attr("data-order", "0");
            }
        });

    $("#btnSaveSpecialLines")
        .off("click")
        .on("click", function () {
            saveSpecialLines();
        });

    $("#btnSpecialLinesExit")
        .off("click")
        .on("click", function () {
            if (typeof openModule === "function") {
                openModule("Profile");
            }
        });
}

function saveSpecialLines() {
    var customer = typeof getSelectedCustomer === "function" ? getSelectedCustomer() : null;
    if (!customer) return;

    $.ajax({
        url: "/CustomerProfile/SaveSpecialLines",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            custId: customer.CustId,
            lines: Array.from(selectedLines)
        }),
        success: function () {
            alert("Special Lines saved successfully.");

            // Re-order table so all selected items float to the top and reset to Page 1
            if (tableSpecialLines) {
                tableSpecialLines.rows().invalidate('dom');
                tableSpecialLines.order([[0, 'desc'], [1, 'asc']]).page(0).draw(false);
            }
        },
        error: function () {
            alert("Unable to save Special Lines.");
        }
    });
}