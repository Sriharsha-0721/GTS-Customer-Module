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
let model = window.currentCustomerProfile;
// Controller used to cancel an in-flight billing save request
let billingController = null;

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
        case "Billing":
            openCustomerProfileModule("Billing");
            break;
    }

}
function openModule(moduleName) {

    currentModule = moduleName;

    const customer = getSelectedCustomer();

    if (!customer)
        return;

    // Load customer profile once
    $.get(`/api/Customer/${customer.CustId}`, function (profile) {

        window.currentCustomerProfile = profile;

        let url = `/CustomerProfile/${moduleName}?custId=${customer.CustId}`;

        $.get(url, function (html) {

            $("#workspace").html(html);

            afterModuleLoaded(moduleName);

        });

    });

}
function openCustomerProfileModule(moduleName) {

    const customer = getSelectedCustomer();

    let url = `/CustomerProfile/${moduleName}`;

    if (customer) {
        url += `?custId=${customer.CustId}`;
    }

    $.get(url, function (html) {

        $("#CustomerModuleContainer").html(html);

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
        case "Billing":
            initializeBilling();
            break;
        case "Packout":
            initializePackout();
            break;
        case "Wash":
            initializeWash();
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

$(document)
    .off("click", "#btnBilling")
    .on("click", "#btnBilling", function () {

        openCustomerProfileModule("Billing");

    });

async function saveBilling() {

    const customer = getSelectedCustomer();

    if (!customer) {
        alert("Please select a customer.");
        return;
    }

    const amount = $("#txtChargeAmount").val().trim();

    if (amount === "") {
        alert("Enter Charge Amount.");
        return;
    }

    const model = {

        CustId: customer.CustId,

        ChargeTypeId: parseInt($("#ddlChargeType").val()),

        Charges: parseFloat(amount),

        UpdtUser: 1
    };

    console.log("Saving Billing:", model);

    const response = await fetch("/CustomerProfile/SaveBilling", {

        method: "POST",

        headers: {
            "Content-Type": "application/json"
        },

        body: JSON.stringify(model)

    });

    const result = await response.text();

    if (result === "1") {

        openCustomerProfileModule("Billing");

    }
    else if (result === "2") {

        alert("This Charge Type already exists for this customer.");

    }
    else {

        alert("Unable to save Billing.");

    }
}

async function deleteBilling(id) {

    if (!id) {
        alert("Invalid Billing ID.");
        return;
    }

    if (!confirm("Delete record?"))
        return;

    try {

        const response = await fetch(
            `/CustomerProfile/DeleteBilling?billingChargesId=${encodeURIComponent(id)}`,
            {
                method: "DELETE"
            }
        );

        const result = await response.text();

        console.log("Delete Billing:", response.status, result);

        if (!response.ok) {
            alert(result || "Delete failed.");
            return;
        }

        if (result.trim() === "1") {

            await openCustomerProfileModule("Billing");

        }
        else {

            alert("Unable to delete Billing.");

        }

    }
    catch (error) {

        console.error("Delete Billing error:", error);

        alert("Delete failed.");

    }
}

$(document)

    .off("click", "#btnSaveBilling")
    .on("click", "#btnSaveBilling", function () {

        saveBilling();

    });

$(document)

    .off("click", ".deleteBilling")
    .on("click", ".deleteBilling", function () {

        deleteBilling($(this).data("id"));

    });

$(document)

    .off("click", "#btnExitBilling")
    .on("click", "#btnExitBilling", function () {

        openModule("Profile");

    });

$(document)

    .off("click", "#lnkBilling")
    .on("click", "#lnkBilling", function () {

        $(".legacy-menu-item").removeClass("active");

        $(this).addClass("active");

        openCustomerProfileModule("Billing");

    });

function refreshBillingNotes() {

    let comment = $("#txtBillingNotes").data("comment") || "";

    let entries = [];

    $("#tblBilling tbody tr").not("#newBillingRow").each(function () {

        const charge = $(this).find("td:eq(0)").text().trim();
        const amount = $(this).find("td:eq(1)").text().trim();

        if (charge && amount) {
            entries.push(charge + " : " + amount);
        }
    });

    let text = comment;

    if (entries.length > 0) {

        if (text !== "")
            text += "\n\n";  

        text += entries.join("\n");  
    }

    $("#txtBillingNotes").val(text);
}
function initializeBilling() {

    let fullComment = $("#hdnBillingComment").val() || "";

    let customerComment = fullComment;

    // Split on commas
    let parts = fullComment.split(",");

    // Keep everything until the first billing item (something containing :)
    let commentParts = [];

    for (let i = 0; i < parts.length; i++) {

        if (parts[i].includes(":"))
            break;

        commentParts.push(parts[i]);
    }

    customerComment = commentParts.join(",").trim();

    $("#txtBillingNotes").data("comment", customerComment);

    refreshBillingNotes();

    $("#newBillingRow").hide();

    $("#btnCancelBilling").hide();

    $(document).off("click.billing");

    $(document).on("click.billing", "#btnAddBilling", function () {

        $("#newBillingRow").show();

        $("#btnCancelBilling").show();

    });

    $(document).on("click.billing", "#btnCancelBilling", function () {

        $("#newBillingRow").hide();

        $("#btnCancelBilling").hide();

        $("#txtChargeAmount").val("");

    });

}


$(document)

    .off("click", ".legacy-menu-item")
    .on("click", ".legacy-menu-item", function () {

        $(".legacy-menu-item").removeClass("active");

        $(this).addClass("active");

    });


//---------------------------------------------------------
// Packout menu
//---------------------------------------------------------

$(document)
    .off("click", "#lnkPackout")
    .on("click", "#lnkPackout", function () {

        $(".legacy-menu-item").removeClass("active");

        $(this).addClass("active");

        openCustomerProfileModule("Packout");

    });


//---------------------------------------------------------
// Initialize Packout
//---------------------------------------------------------

function initializePackout() {

    const existingText = $("#txtPackoutNotes").val() || "";

    // Store only the customer comment.
    // If the hidden value exists, use it.
    if ($("#hdnPackoutComment").length) {

        $("#txtPackoutNotes").data(
            "comment",
            $("#hdnPackoutComment").val() || ""
        );

    }
    else if ($("#txtPackoutNotes").data("comment") === undefined) {

        $("#txtPackoutNotes").data(
            "comment",
            existingText
        );

    }

    $("#newPackoutRow").hide();

    refreshPackoutNotes();

}


//---------------------------------------------------------
// Add Packout
//---------------------------------------------------------

$(document)
    .off("click.packout", "#btnAddPackout")
    .on("click.packout", "#btnAddPackout", function () {

        $("#newPackoutRow").show();

        // Clear editable fields for a new row
        $("#txtPackoutColor").val("");
        $("#txtPackoutSize").val("");

    });


//---------------------------------------------------------
// Save Packout
//---------------------------------------------------------

async function savePackout() {

    const customer = getSelectedCustomer();

    if (!customer) {

        alert("Please select a customer.");

        return;

    }

    const item = $("#ddlPackoutItem").val();

    const restriction = $("#ddlPackoutRestriction").val();

    if (!item) {

        alert("Please select an Item.");

        return;

    }

    if (!restriction) {

        alert("Please select a Packout Restriction.");

        return;

    }

    const model = {

        CustId: customer.CustId,

        PkoutRestrict: restriction,

        Item: item,

        Color: $("#txtPackoutColor").val().trim(),

        Size: $("#txtPackoutSize").val().trim(),

        UpdtUser: 1

    };

    console.log("Saving Packout:", model);

    try {

        const response = await fetch(
            "/CustomerProfile/SavePackout",
            {
                method: "POST",

                headers: {
                    "Content-Type": "application/json"
                },

                body: JSON.stringify(model)
            }
        );

        const result = await response.text();

        console.log("Packout save response:", result);

        if (!response.ok) {

            alert(
                result ||
                "Unable to save Packout."
            );

            return;

        }

        // Hide new row
        $("#newPackoutRow").hide();

        // Clear fields
        $("#txtPackoutColor").val("");
        $("#txtPackoutSize").val("");

        // Reload Packout grid
        await openCustomerProfileModule("Packout");

    }
    catch (error) {

        console.error("Packout save error:", error);

        alert("Unable to save Packout.");

    }

}


//---------------------------------------------------------
// Save button
//---------------------------------------------------------

$(document)
    .off("click.packout", "#btnSavePackout")
    .on("click.packout", "#btnSavePackout", function () {

        savePackout();

    });


//---------------------------------------------------------
// Delete Packout
//---------------------------------------------------------

async function deletePackout(id) {

    console.log("Deleting Packout ID:", id);

    if (!id) {
        alert("Invalid Packout ID.");
        return;
    }

    if (!confirm("Delete record?"))
        return;

    try {

        const response = await fetch(
            `/CustomerProfile/DeletePackout?pkoutRestrictId=${encodeURIComponent(id)}`,
            {
                method: "DELETE"
            }
        );

        const result = await response.text();

        console.log("Delete response:", response.status, result);

        if (!response.ok) {

            alert(result || "Delete failed.");
            return;

        }

        await openCustomerProfileModule("Packout");

    }
    catch (error) {

        console.error("Delete Packout error:", error);

        alert("Delete failed.");

    }
}


//---------------------------------------------------------
// Delete button
//---------------------------------------------------------

$(document)
    .off("click.packout", ".deletePackout")
    .on("click.packout", ".deletePackout", function () {

        const id = $(this).data("id");

        console.log("Clicked delete ID:", id);

        deletePackout(id);

    });

//---------------------------------------------------------
// Refresh Packout Notes
//---------------------------------------------------------

function refreshPackoutNotes() {

    // Original customer comment ONLY
    const comment =
        $("#hdnPackoutComment").val() || "";

    let rows = [];

    $("#tblPackout tbody tr")
        .not("#newPackoutRow")
        .each(function () {

            const restriction =
                $(this).find("td:eq(0)").text().trim();

            const item =
                $(this).find("td:eq(1)").text().trim();

            const color =
                $(this).find("td:eq(2)").text().trim();

            const size =
                $(this).find("td:eq(3)").text().trim();

            if (restriction && item) {

                let line =
                    restriction + " : " + item;

                if (color)
                    line += " - " + color;

                if (size)
                    line += " (" + size + ")";

                rows.push(line);
            }
        });

    let finalText = comment;

    if (rows.length > 0) {

        if (finalText !== "")
            finalText += "\n\n";

        finalText += rows.join("\n");
    }

    $("#txtPackoutNotes").val(finalText);
}

function initializeWash() {

    $("#newWashRow").hide();

}

$(document)
    .off("click", "#btnAddWash")
    .on("click", "#btnAddWash", function () {

        $("#newWashRow").show();

        $("#ddlWashGarmentType").focus();

    });
async function saveWash() {

    const customer = getSelectedCustomer();

    if (!customer) {
        alert("Please select a customer.");
        return;
    }

    //-------------------------------------------------
    // Check new row
    //-------------------------------------------------

    if ($("#newWashRow").is(":visible")) {

        const garmentType =
            $("#ddlWashGarmentType").val();

        const formula =
            $("#txtWashFormula").val().trim();

        if (!garmentType) {
            alert("Please select Garment Type.");
            return;
        }

        if (!formula) {
            alert("Formula cannot be empty.");
            return;
        }
    }


    //-------------------------------------------------
    // Build Formula string
    //
    // Example:
    // All,6,SHIRT,8,PANT,5
    //-------------------------------------------------

    const formulaParts = [];

    let duplicate = false;

    $("#tblWash tbody tr")
        .not("#newWashRow")
        .each(function () {

            const garmentType =
                $(this)
                    .find("td:eq(0)")
                    .text()
                    .trim();

            const formula =
                $(this)
                    .find("td:eq(1)")
                    .text()
                    .trim();

            if (!garmentType)
                return;

            formulaParts.push(
                garmentType,
                formula
            );
        });


    //-------------------------------------------------
    // Add new row
    //-------------------------------------------------

    if ($("#newWashRow").is(":visible")) {

        const garmentType =
            $("#ddlWashGarmentType").val();

        const formula =
            $("#txtWashFormula").val().trim();


        //-------------------------------------------------
        // Check duplicate Garment Type
        //-------------------------------------------------

        const exists =
            $("#tblWash tbody tr")
                .not("#newWashRow")
                .filter(function () {

                    return $(this)
                        .find("td:eq(0)")
                        .text()
                        .trim()
                        .toLowerCase()
                        === garmentType.toLowerCase();

                })
                .length > 0;


        if (exists) {

            alert(
                garmentType +
                " already exists."
            );

            return;
        }


        formulaParts.push(
            garmentType,
            formula
        );
    }


    //-------------------------------------------------
    // Create Formula string
    //-------------------------------------------------

    const formula =
        formulaParts.join(",");


    console.log(
        "WASH FORMULA:",
        formula
    );


    //-------------------------------------------------
    // WashCom
    //-------------------------------------------------

    const washCom =
        $("#txtWashNotes").val() || "";


    //-------------------------------------------------
    // Payload
    //-------------------------------------------------

    const payload = {

        CustId: customer.CustId,

        WashCom: washCom,

        Formula: formula

    };


    console.log(
        "WASH SAVE PAYLOAD:",
        payload
    );


    //-------------------------------------------------
    // Save
    //-------------------------------------------------

    const response =
        await fetch(
            "/CustomerProfile/SaveWash",
            {
                method: "POST",

                headers: {
                    "Content-Type":
                        "application/json"
                },

                body:
                    JSON.stringify(payload)
            }
        );


    if (response.ok) {

        $("#newWashRow").hide();

        $("#txtWashFormula").val("");

        openCustomerProfileModule("Wash");

    }
    else {

        const error =
            await response.text();

        console.error(
            "Wash save failed:",
            error
        );

        alert(
            "Unable to save Wash."
        );
    }
}
$(document)
    .off("click", "#btnSaveWash")
    .on("click", "#btnSaveWash", function () {

        saveWash();

    });
$(document)
    .off("click", ".deleteWash")
    .on("click", ".deleteWash", async function () {

        if (!confirm("Delete this Wash formula?"))
            return;

        $(this)
            .closest("tr")
            .remove();

        await saveWash();
    });
$(document)
    .off("click", "#lnkWash")
    .on("click", "#lnkWash", function () {

        $(".legacy-menu-item").removeClass("active");

        $(this).addClass("active");

        openCustomerProfileModule("Wash");

    });
function getCurrentWashFormula(savedCustomer) {

    // Wash module is not currently loaded.
    // Preserve the value already in DB.
    if (!$("#tblWash").length) {
        return savedCustomer.formula || "";
    }

    const parts = [];

    $("#tblWash tbody tr")
        .not("#newWashRow")
        .each(function () {

            const garmentType =
                $(this)
                    .find("td:eq(0)")
                    .text()
                    .trim();

            const formula =
                $(this)
                    .find("td:eq(1)")
                    .text()
                    .trim();

            if (garmentType && formula) {

                parts.push(garmentType);
                parts.push(formula);
            }
        });

    return parts.join(",");
}

$(document)
    .off("click.customerProfile", "#btnSaveCustomerProfile")
    .on("click.customerProfile", "#btnSaveCustomerProfile", async function (e) {

        e.preventDefault();
        e.stopPropagation();

        const customer = getSelectedCustomer();

        if (!customer) {
            alert("Please select a customer.");
            return;
        }

        //-------------------------------------------------
        // GET LATEST CUSTOMER DATA FROM DATABASE
        //-------------------------------------------------

        let savedCustomer;

        try {

            const response = await fetch(
                `/api/Customer/${customer.CustId}`
            );

            if (!response.ok) {
                throw new Error("Unable to load customer data.");
            }

            savedCustomer = await response.json();

            console.log(
                "LATEST CUSTOMER FROM API:",
                savedCustomer
            );

        }
        catch (error) {

            console.error(
                "Unable to load latest customer:",
                error
            );

            alert("Unable to load current customer data.");
            return;
        }


        //-------------------------------------------------
        // BILLING
        //-------------------------------------------------

        let billingCom =
            savedCustomer.billingCom || "";

        // Billing is currently loaded
        if (
            $("#txtBillingNotes").length &&
            $("#tblBilling").length
        ) {

            let billingComment =
                $("#txtBillingNotes").val() || "";

            let billingEntries = [];

            $("#tblBilling tbody tr")
                .not("#newBillingRow")
                .each(function () {

                    const charge =
                        $(this)
                            .find("td:eq(0)")
                            .text()
                            .trim();

                    const amount =
                        $(this)
                            .find("td:eq(1)")
                            .text()
                            .trim();

                    if (charge && amount) {

                        billingEntries.push(
                            `${charge}:${amount}`
                        );
                    }
                });


            //-------------------------------------------------
            // Extract only the customer-written comment
            //-------------------------------------------------

            let lines =
                billingComment.split(/\r?\n/);

            let commentLines = [];

            for (const line of lines) {

                const value = line.trim();

                if (!value)
                    continue;

                // Skip generated Billing rows
                // Example:
                // Zipper : 7.00
                if (
                    /^[^:]+:\s*\d/.test(
                        value.replace(" : ", ":")
                    )
                ) {
                    continue;
                }

                commentLines.push(value);
            }

            billingCom =
                commentLines.join("\n").trim();


            //-------------------------------------------------
            // Add Billing entries
            //-------------------------------------------------

            if (billingEntries.length > 0) {

                if (billingCom !== "")
                    billingCom += ",";

                billingCom +=
                    billingEntries.join(",");
            }
        }


        //-------------------------------------------------
        // PACKOUT
        //-------------------------------------------------

        // IMPORTANT:
        // PackoutCom stores ONLY the customer note.
        // Packout rows are stored in PackoutRestriction.

        let packoutCom =
            savedCustomer.packoutCom || "";


        // Packout is currently loaded
        if ($("#txtPackoutNotes").length) {

            let packoutText =
                $("#txtPackoutNotes").val() || "";

            let lines =
                packoutText.split(/\r?\n/);

            let commentLines = [];

            for (const line of lines) {

                const value = line.trim();

                if (!value)
                    continue;

                // Remove generated Packout rows.
                //
                // YES : ITEM - COLOR (SIZE)
                // NO : ITEM - COLOR (SIZE)
                //
                if (
                    /^(YES|NO)\s*:/i.test(value)
                ) {
                    continue;
                }

                commentLines.push(value);
            }

            packoutCom =
                commentLines.join("\n").trim();
        }


        //-------------------------------------------------
        // OTHER CUSTOMER COMMENTS
        //-------------------------------------------------

        const washCom =
            $("#txtWashNotes").length
                ? $("#txtWashNotes").val()
                : (savedCustomer.washCom || "");


        const soilCom =
            $("#txtSoilNotes").length
                ? $("#txtSoilNotes").val()
                : (savedCustomer.soilCom || "");


        const dryerCom =
            $("#txtDryerNotes").length
                ? $("#txtDryerNotes").val()
                : (savedCustomer.dryerCom || "");


        const receivingCom =
            $("#txtReceivingNotes").length
                ? $("#txtReceivingNotes").val()
                : (savedCustomer.receivingCom || "");


        const shippingCom =
            $("#txtShippingNotes").length
                ? $("#txtShippingNotes").val()
                : (savedCustomer.shippingCom || "");


        const driverCom =
            $("#txtDriverNotes").length
                ? $("#txtDriverNotes").val()
                : (savedCustomer.driverCom || "");


        const mendCom =
            $("#txtMendNotes").length
                ? $("#txtMendNotes").val()
                : (savedCustomer.mendCom || "");


        const qaCom =
            $("#txtQANotes").length
                ? $("#txtQANotes").val()
                : (savedCustomer.qaCom || "");


        const custSrvCom =
            $("#txtCustomerServiceNotes").length
                ? $("#txtCustomerServiceNotes").val()
                : (savedCustomer.custSrvCom || "");


        const officeCom =
            $("#txtOfficeNotes").length
                ? $("#txtOfficeNotes").val()
                : (savedCustomer.officeCom || "");


        const genOfficeCom =
            $("#txtGeneralOfficeNotes").length
                ? $("#txtGeneralOfficeNotes").val()
                : (savedCustomer.genOfficeCom || "");


        const merControlCom =
            $("#txtMerchControlNotes").length
                ? $("#txtMerchControlNotes").val()
                : (savedCustomer.merControlCom || "");


        const mainCleanRoomCom =
            $("#txtMainCleanRoomNotes").length
                ? $("#txtMainCleanRoomNotes").val()
                : (savedCustomer.mainCleanRoomCom || "");


        const qaInspCom =
            $("#txtQAInspectorNotes").length
                ? $("#txtQAInspectorNotes").val()
                : (savedCustomer.qaInspCom || "");


        const prodCom =
            $("#txtProductionNotes").length
                ? $("#txtProductionNotes").val()
                : (savedCustomer.prodCom || "");

        const formula =
            getCurrentWashFormula(savedCustomer);


        //-------------------------------------------------
        // FINAL PAYLOAD
        //-------------------------------------------------

        const payload = {

            custId: customer.CustId,

            billingCom: billingCom,

            packoutCom: packoutCom,

            washCom: washCom,

            formula: formula,

            soilCom: soilCom,

            dryerCom: dryerCom,

            receivingCom: receivingCom,

            shippingCom: shippingCom,

            driverCom: driverCom,

            mendCom: mendCom,

            qaCom: qaCom,

            custSrvCom: custSrvCom,

            officeCom: officeCom,

            genOfficeCom: genOfficeCom,

            merControlCom: merControlCom,

            mainCleanRoomCom: mainCleanRoomCom,

            qaInspCom: qaInspCom,

            prodCom: prodCom
        };


        //-------------------------------------------------
        // DEBUG
        //-------------------------------------------------

        console.log(
            "OVERALL SAVE PAYLOAD:",
            payload
        );


        //-------------------------------------------------
        // SAVE CUSTOMER PROFILE
        //-------------------------------------------------

        $.ajax({

            url: "/CustomerProfile/SaveCustomerProfile",

            type: "POST",

            contentType: "application/json",

            data: JSON.stringify(payload),

            success: function () {

                alert(
                    "Customer Profile Saved Successfully."
                );

            },

            error: function (xhr) {

                console.error(
                    "Customer Profile Save Error:",
                    xhr.responseText
                );

                alert(
                    xhr.responseText ||
                    "Unable to save Customer Profile."
                );
            }
        });

    });