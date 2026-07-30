// =====================================================
// customerProfile.js
// GTS Customer Profile Module
// =====================================================

// -----------------------------------------------------
// Session Storage Keys
// -----------------------------------------------------
let currentModule = "Profile";
let currentSpecialLinesPage = 1;
let selectedLines = new Set();

const STORAGE_KEYS = {
    CUSTOMER: "selectedCustomer",
    MODULE: "requestedModule"
};

// -----------------------------------------------------
// Storage Helpers
// -----------------------------------------------------

function getSelectedCustomer() {

    const customer =
        sessionStorage.getItem(STORAGE_KEYS.CUSTOMER);

    return customer
        ? JSON.parse(customer)
        : null;
}

function setSelectedCustomer(customer) {
    sessionStorage.setItem(STORAGE_KEYS.CUSTOMER, JSON.stringify(customer));
}

function clearSelectedCustomer() {
    sessionStorage.removeItem(STORAGE_KEYS.CUSTOMER);
}

function getRequestedModule() {
    return sessionStorage.getItem(STORAGE_KEYS.MODULE);
}

function setRequestedModule(module) {
    sessionStorage.setItem(STORAGE_KEYS.MODULE, module);
}

function clearRequestedModule() {
    sessionStorage.removeItem(STORAGE_KEYS.MODULE);
}

function changeCustomer(moduleName) {

    setRequestedModule(moduleName);

    openCustomerSearch();
}
// -----------------------------------------------------
// Customer Header
// -----------------------------------------------------

function populateCustomerHeader() {

    const customer = getSelectedCustomer();

    if (!customer) {
        clearCustomerHeader();
        return;
    }

    $("#custNo").val(customer.CustNo ?? "");
    $("#custName").val(customer.Customer ?? "");
    $("#route").val(customer.Route ?? "");
    $("#gid").val(customer.GID ?? "");
}

function clearCustomerHeader() {

    $("#custNo,#custName,#route,#gid").val("");
}

// -----------------------------------------------------
// Navigation
// -----------------------------------------------------

function ensureCustomerSelected(moduleName) {

    const customer = getSelectedCustomer();

    if (!customer) {

        setRequestedModule(moduleName);

        openCustomerSearch();

        return;

    }

    openModule(moduleName);

}

function openRequestedModule() {

    const module = getRequestedModule();

    clearRequestedModule();

    switch (module) {

        case "Customer":
            openCustomer();
            break;

        case "Wearer":
            openWearer();
            break;

        case "Profile":
            openProfile();
            break;

        case "CTSSettings":
            openModule("CTSSettings");
            break;

        case "MaximumWash":
            openModule("MaximumWash");
            break;

case "CustomerLineComments":
    openModule("CustomerLineComments");
    break;

case "SpecialLines":
    openModule("SpecialLines");
    break;
    }
}
function openModule(moduleName) {

    currentModule = moduleName;

    const customer = getSelectedCustomer();

    let url = `/CustomerProfile/${moduleName}`;

    if (customer) {
        url += `?custId=${customer.CustId}`;
    }

    $.get(url, function (html) {

        $("#workspace").html(html);

        afterModuleLoaded(moduleName);

    });
}

// =====================================================
// Customer Search
// =====================================================

function openCustomerSearch() {

    showAllCustomers();

}
function wireCustomerSearch() {

    $("#btnSearch").off("click").on("click", searchCustomer);

    $("#btnShowAll").off("click").on("click", showAllCustomers);

    $("#btnClear").off("click").on("click", clearCustomerSearch);

    wireSelectCustomer();
}

function searchCustomer() {

    clearSearchMessage();

    const data = {
        searchId: $("#searchId").val().trim(),
        searchName: $("#searchName").val().trim(),
        searchCity: $("#searchCity").val().trim()
    };

    if (!data.searchId && !data.searchName && !data.searchCity) {
        showSearchError("Please enter Customer ID, Customer Name or City.");
        return;
    }

    $.get("/Customer/Search", data)
        .done(function (html) {
            $("#workspace").html(html);
            wireCustomerSearch();
        })
        .fail(function () {
            showSearchError("Search failed.");
        });
}

function showAllCustomers() {

    $.get("/Customer/Search", { showAll: true })
        .done(function (html) {
            $("#workspace").html(html);
            wireCustomerSearch();
        })
        .fail(function () {
            showSearchError("Unable to load customers.");
        });
}

function clearCustomerSearch() {

    $("#searchId,#searchName,#searchCity").val("");

    $("#customerGrid").empty();

    clearSearchMessage();

    $("#searchId").focus();
}

function clearSearchMessage() {

    $("#searchMessage")
        .removeClass("text-danger text-success fw-bold")
        .empty();
}

function showSearchError(message) {

    $("#searchMessage")
        .removeClass("text-success")
        .addClass("text-danger fw-bold")
        .text(message);
}

// =====================================================
// Customer Selection
// =====================================================

function wireSelectCustomer() {

    $(".selectCustomer")
        .off("click")
        .on("click", function (e) {

            e.preventDefault();

            const customer = {
                MC: $(this).data("mc"),
                CustId: Number($(this).data("custid")),
                CustNo: $(this).data("custno"),
                Customer: $(this).data("name"),
                Route: $(this).data("route"),
                GID: $(this).data("gid")
            };

            setSelectedCustomer(customer);

            populateCustomerHeader();

            openRequestedModule();
        });
}
// =====================================================
// CTS Settings
// =====================================================

function initializeCTSSettings() {

    populateCustomerHeader();

}

async function saveCTSSettings() {

    const customer = getSelectedCustomer();

    if (!customer) {
        alert("Please select a customer.");
        return;
    }

    const model = {
        MC: customer.MC,
        CustNo: Number(customer.CustNo),
        PrintIssueStatusFlag: $("#PrintIssueStatusFlag").is(":checked"),
        PrintBornonDateFlag: $("#PrintBornonDateFlag").is(":checked"),
        NOGFlag: $("#NOGFlag").is(":checked"),
        LabelHeader: $("#LabelHeader").val()
    };

    const response = await fetch("/CTSSettings/Create", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(model)
    });

    if (!response.ok) {
        alert("Unable to save CTS Settings.");
        return;
    }

    alert("CTS Settings saved successfully.");

// Reload the CTS partial so it reflects the latest data
    openModule("CTSSettings");
}

$(document)
    .off("click", "#btnSaveCTS")
    .on("click", "#btnSaveCTS", saveCTSSettings);
function afterModuleLoaded(moduleName) {

    switch (moduleName) {

        case "Customer":
            populateCustomerHeader();
            break;

        case "Wearer":
            populateCustomerHeader();
            break;

        case "Profile":
            populateCustomerHeader();
            break;

        case "CTSSettings":
            initializeCTSSettings();
            break;

        case "MaximumWash":
            initializeMaximumWash();
            break;

        case "CustomerLineComments":
            initializeCustomerLineComments();
            break;

        case "SpecialLines":
            initializeSpecialLines();
            break;
    }
}
// =====================================================
// Maximum Wash
// =====================================================

function initializeMaximumWash() {

    populateCustomerHeader();

    loadMaximumWash();

}

async function loadMaximumWash() {

    const customer = getSelectedCustomer();

    if (!customer) return;

    try {

        const response = await fetch(`/CustomerProfile/MaximumWash?custId=${customer.CustId}`);

        if (!response.ok)
            throw new Error();

        $("#CustomerModuleContainer").html(await response.text());

    }
    catch {

        alert("Unable to load Maximum Wash.");

    }
}
function refreshMaximumWash() {
    openModule("MaximumWash");
}
function enableNewMaximumWash() {

    $("#newItemCode").prop("disabled", false);
    $("#newMaxWash").prop("disabled", false);
    $("#newMaxWeeks").prop("disabled", false);
    $("#newMaxCycles").prop("disabled", false);

    $("#btnAddMaxWash").hide();
    $("#btnSaveMaxWash").show();
    $("#btnCancelMaxWash").show();

    $("#newItemCode").focus();

}

function cancelNewMaximumWash() {

    $("#newItemCode").val("").prop("disabled", true);
    $("#newMaxWash").val("").prop("disabled", true);
    $("#newMaxWeeks").val("").prop("disabled", true);
    $("#newMaxCycles").val(1).prop("disabled", true);

    $("#btnAddMaxWash").show();
    $("#btnSaveMaxWash,#btnCancelMaxWash").hide();
}

async function saveNewMaximumWash() {

    const customer = getSelectedCustomer();

    if (!customer) return;

    const model = {
        CustId: customer.CustId,
        ItemCode: $("#newItemCode").val().trim(),
        MaxWash: Number($("#newMaxWash").val()),
        MaxWeeks: Number($("#newMaxWeeks").val()),
        MaxCycles: Number($("#newMaxCycles").val())
    };

    if (!model.ItemCode)
        return alert("Item Code is required.");

    if (model.MaxWash <= 0)
        return alert("Maximum Wash must be greater than zero.");

    if (model.MaxWeeks <= 0)
        return alert("Maximum Weeks must be greater than zero.");

    if (model.MaxCycles <= 0)
        return alert("Maximum Cycles must be greater than zero.");

    try {

        const response = await fetch("/CustomerProfile/AddMaximumWash", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(model)
        });

        if (!response.ok)
            throw new Error();

        refreshMaximumWash();

    }
    catch {

        alert("Unable to add Maximum Wash.");

    }
}

async function updateMaximumWash(button) {

    const customer = getSelectedCustomer();

    const itemCode = button.dataset.itemcode;

    const model = {

        CustId: customer.CustId,

        OldItemCode: itemCode,

        NewItemCode:
            $("#itemCode_" + itemCode).val().trim(),

        MaxWash:
            Number($("#maxWash_" + itemCode).val()),

        MaxWeeks:
            Number($("#maxWeeks_" + itemCode).val()),

        MaxCycles:
            Number($("#maxCycles_" + itemCode).val())

    };

    const response = await fetch(

        "/CustomerProfile/UpdateMaximumWash",

        {

            method: "PUT",

            headers: {

                "Content-Type": "application/json"

            },

            body: JSON.stringify(model)

        });

    if (response.ok) {

        refreshMaximumWash();

    }
    else {

        alert("Update failed.");

    }

}

function enableMaximumWashEdit(button) {

    const itemCode = button.dataset.itemcode;

    $(`#itemCode_${itemCode},
       #maxWash_${itemCode},
       #maxWeeks_${itemCode},
       #maxCycles_${itemCode}`)
        .prop("disabled", false);

    button.innerText = "Save";

    $(button)
        .next(".cancelEditMaxWash")
        .show();
}

async function deleteMaximumWash(custId, itemCode) {

    if (!confirm("Delete this record?"))
        return;

    try {

        const response = await fetch(
            `/CustomerProfile/DeleteMaximumWash?custId=${custId}&itemCode=${encodeURIComponent(itemCode)}`,
            {
                method: "DELETE"
            });

        if (!response.ok)
            throw new Error();

        refreshMaximumWash();

    }
    catch {

        alert("Delete failed.");

    }
}
$(document)

    .on("click", "#btnAddMaxWash", function () {

        enableNewMaximumWash();

    })

    .on("click", "#btnCancelMaxWash", function () {

        cancelNewMaximumWash();

    })

    .on("click", "#btnSaveMaxWash", function () {

        saveNewMaximumWash();

    })

    .on("click", ".editMaxWash", function () {

        if ($(this).text().trim() === "Edit") {

            enableMaximumWashEdit(this);

        }
        else {

            updateMaximumWash(this);

        }

    })

    .on("click", ".cancelEditMaxWash", function () {

        refreshMaximumWash();

    })

    .on("click", ".deleteMaxWash", function () {

        deleteMaximumWash(

            $(this).data("custid"),

            $(this).data("itemcode")

        );

    });
// =====================================================
// Customer Line Comments
// =====================================================

function initializeCustomerLineComments() {

    populateCustomerHeader();

    loadCustomerLineComments();

}

async function loadCustomerLineComments() {

    const customer = getSelectedCustomer();

    if (!customer) return;

    try {

        const response = await fetch(
            `/CustomerProfile/CustomerLineComments?custId=${customer.CustId}`
        );

        if (!response.ok)
            throw new Error();

        $("#CustomerModuleContainer")
            .html(await response.text());

    }
    catch {

        alert("Unable to load Customer Line Comments.");

    }
}

function refreshCustomerLineComments() {
    openModule("CustomerLineComments");
}
function enableCustomerLineCommentEdit(button) {

    const wearItemId = button.dataset.wearitemid;

    const $descr = $(`#descr_${wearItemId}`);

    $descr.prop("disabled", false).focus();

    button.innerText = "Save";

    $(button)
        .next(".cancelCustLineComment")
        .show();

    $("#btnExitCustLineComments")
        .prop("disabled", true);
}

async function saveCustomerLineComment(button) {

    const wearItemId = Number(button.dataset.wearitemid);
    const custId = Number(button.dataset.custid);

    const descr = $(`#descr_${wearItemId}`);

    if (!descr.val().trim()) {

        alert("Please enter comments.");

        descr.focus();

        return;
    }

    const model = {
        CustId: custId,
        WearItemId: wearItemId,
        Descr: descr.val().trim()
    };

    try {

        const response = await fetch(
            "/CustomerProfile/SaveCustomerLineComments",
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(model)
            });

        if (!response.ok)
            throw new Error();

        refreshCustomerLineComments();

    }
    catch {

        alert("Unable to save comments.");

    }
}
function cancelCustomerLineCommentEdit() {
    refreshCustomerLineComments();
}   
function exitCustomerLineComments() {
    openModule("Profile");
}

$(document)

    .off("click", ".editCustLineComment")
    .on("click", ".editCustLineComment", function () {

        if ($(this).text().trim() === "Edit")
            enableCustomerLineCommentEdit(this);
        else
            saveCustomerLineComment(this);

    })

    .off("click", ".cancelCustLineComment")
    .on("click", ".cancelCustLineComment", cancelCustomerLineCommentEdit)

    .off("click", "#btnExitCustLineComments")
    .on("click", "#btnExitCustLineComments", exitCustomerLineComments);// Main Navigation
// =====================================================

function openCustomer() {
    ensureCustomerSelected("Customer");
}

function openWearer() {
    ensureCustomerSelected("Wearer");
}

function openProfile() {
    ensureCustomerSelected("Profile");
}
function registerNavigation() {

    $("#tabCustomer")
        .off("click")
        .on("click", function () {
            openCustomer();
        });

    $("#tabWearer")
        .off("click")
        .on("click", function () {
            openWearer();
        });

    $("#tabProfile")
        .off("click")
        .on("click", function () {
            openProfile();
        });

    $("#tabCTS")
        .off("click")
        .on("click", function () {
            ensureCustomerSelected("CTSSettings");
        });

    $("#tabCustLineComments")
        .off("click")
        .on("click", function () {
            ensureCustomerSelected("CustomerLineComments");
        });

    // Change Customer buttons
    $(document)
        .off("click", "#btnSelectCustomer")
        .on("click", "#btnSelectCustomer", function () {

            setRequestedModule(currentModule);

            openCustomerSearch();
        });
}

// =====================================================
// Common Exit Buttons
// =====================================================

$(document)

    .on("click", "#btnExitMaxWash", function () {

        openProfile();

    })

    .on("click", "#btnExitCTS", function () {

        openProfile();

    })

    .on("click", "#btnExitCustomer", function () {

        openCustomer();

    })

    .on("click", "#btnExitWearer", function () {

        openCustomer();

    });

$(document)

    .off("click", "#btnMaxWash")

    .on("click", "#btnMaxWash", function () {

        openModule("MaximumWash");

    });

// =====================================================
// Helpers
// =====================================================
$(document)

    .off("click", "#btnLineComments")

    .on("click", "#btnLineComments", function () {

        openModule("CustomerLineComments");

    });
function showSuccess(message) {

    alert(message);

}

function showError(message) {

    alert(message);

}

function isCustomerSelected() {

    return getSelectedCustomer() !== null;

}

function requireCustomer(moduleName) {

    if (!isCustomerSelected()) {

        ensureCustomerSelected(moduleName);

        return false;

    }

    return true;

}

// =====================================================
// Application Initialization
// =====================================================

$(function () {

    registerNavigation();

    const customer = getSelectedCustomer();

    if (customer) {

        populateCustomerHeader();

    }

});
// =====================================================
// Utilities
// =====================================================

function resetModule() {

    clearRequestedModule();

}

function logoutCustomer() {

    clearSelectedCustomer();

    clearCustomerHeader();

    resetModule();

}

window.CustomerProfile = {

    openCustomer,

    openWearer,

    openProfile,


    openModule,

    openCustomerSearch,

    refreshMaximumWash,

    refreshCustomerLineComments,

    logoutCustomer

};
// ================================
// Special Lines
// ================================

$(document)
    .off("click", "#btnSpecialLines")
    .on("click", "#btnSpecialLines", function () {

        selectedLines.clear();

        loadSpecialLines(1);

    });

// =========================
// Special Lines
// =========================

function initializeSpecialLines() {

    populateCustomerHeader();

    bindSpecialLineEvents();

}

function loadSpecialLines(page = 1) {

    currentSpecialLinesPage = page;

    const customer = getSelectedCustomer();

    if (!customer)
        return;

    $.get(
        "/CustomerProfile/SpecialLines",
        {
            custId: customer.CustId,
            page: page
        },
        function (html) {

            $("#workspace").html(html);

            // Restore selected checkboxes
            $(".chkSpecialLine").each(function () {

                const line = parseInt($(this).data("line"));

                if (selectedLines.has(line)) {
                    $(this).prop("checked", true);
                }
                else if ($(this).is(":checked")) {
                    // Add already saved selections to the Set
                    selectedLines.add(line);
                }

            });

            bindSpecialLineEvents();

        }
    );
}

function bindSpecialLineEvents() {

    // Pagination
    $(".specialLinesPage")
        .off("click")
        .on("click", function (e) {

            e.preventDefault();

            const page = parseInt($(this).data("page"));

            if (!isNaN(page)) {
                loadSpecialLines(page);
            }

        });

    // Checkbox Change
    $(".chkSpecialLine")
        .off("change")
        .on("change", function () {

            const line = parseInt($(this).data("line"));

            if ($(this).is(":checked")) {
                selectedLines.add(line);
            }
            else {
                selectedLines.delete(line);
            }

        });

    // Save
    $("#btnSaveSpecialLines")
        .off("click")
        .on("click", function () {

            saveSpecialLines();

        });

    // Exit
    $("#btnSpecialLinesExit")
        .off("click")
        .on("click", function () {

            openModule("Profile");

        });
}

function saveSpecialLines() {

    const customer = getSelectedCustomer();

    if (!customer)
        return;

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

        },

        error: function () {

            alert("Unable to save Special Lines.");

        }

    });
}