// =====================================================
// customerProfile.js
// GTS Customer Profile Module
// =====================================================

// -----------------------------------------------------
// Session Storage Keys & State
// -----------------------------------------------------
let currentModule = "Profile";
let model = window.currentCustomerProfile;
let billingController = null;

const STORAGE_KEYS = {
    CUSTOMER: "selectedCustomer",
    MODULE: "requestedModule"
};

// -----------------------------------------------------
// Storage Helpers
// -----------------------------------------------------

function getSelectedCustomer() {
    const customer = sessionStorage.getItem(STORAGE_KEYS.CUSTOMER);
    return customer ? JSON.parse(customer) : null;
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

    if (!customer) return;

    $.get(`/api/Customer/${customer.CustId}`, function (profile) {
        window.currentCustomerProfile = profile;
        let url = `/CustomerProfile/${moduleName}?custId=${customer.CustId}`;

        $.get(url, function (html) {
            $("#workspace").html(html);
            afterModuleLoaded(moduleName);
        });
    });
}

function openCustomerProfileModule(moduleName, params = {}) {
    const customer = getSelectedCustomer();

    if (!customer) {
        alert("Please select a customer first.");
        return;
    }

    let url = `/CustomerProfile/${moduleName}`;
    const query = [];
    query.push(`custId=${encodeURIComponent(customer.CustId)}`);

    Object.keys(params).forEach(function (key) {
        const value = params[key];
        if (value !== null && value !== undefined && value !== "") {
            query.push(`${key}=${encodeURIComponent(value)}`);
        }
    });

    url += "?" + query.join("&");

    $.get(url)
        .done(function (html) {
            $("#CustomerModuleContainer").html(html);

            if (moduleName === "Billing") {
                initializeBilling();
            } else if (moduleName === "Packout") {
                initializePackout();
            } else if (moduleName === "Wash") {
                initializeWash();
            }
        })
        .fail(function (xhr) {
            console.error("Module load failed:", xhr.status, xhr.responseText);
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
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(model)
    });

    if (!response.ok) {
        alert("Unable to save CTS Settings.");
        return;
    }

    alert("CTS Settings saved successfully.");
    openModule("CTSSettings");
}

$(document)
    .off("click", "#btnSaveCTS")
    .on("click", "#btnSaveCTS", saveCTSSettings);

function afterModuleLoaded(moduleName) {
    switch (moduleName) {
        case "Customer":
        case "Wearer":
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
        if (!response.ok) throw new Error();
        $("#CustomerModuleContainer").html(await response.text());
    } catch {
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

    if (!model.ItemCode) return alert("Item Code is required.");
    if (model.MaxWash <= 0) return alert("Maximum Wash must be greater than zero.");
    if (model.MaxWeeks <= 0) return alert("Maximum Weeks must be greater than zero.");
    if (model.MaxCycles <= 0) return alert("Maximum Cycles must be greater than zero.");

    try {
        const response = await fetch("/CustomerProfile/AddMaximumWash", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(model)
        });

        if (!response.ok) throw new Error();
        refreshMaximumWash();
    } catch {
        alert("Unable to add Maximum Wash.");
    }
}

async function updateMaximumWash(button) {
    const customer = getSelectedCustomer();
    const itemCode = button.dataset.itemcode;

    const model = {
        CustId: customer.CustId,
        OldItemCode: itemCode,
        NewItemCode: $("#itemCode_" + itemCode).val().trim(),
        MaxWash: Number($("#maxWash_" + itemCode).val()),
        MaxWeeks: Number($("#maxWeeks_" + itemCode).val()),
        MaxCycles: Number($("#maxCycles_" + itemCode).val())
    };

    const response = await fetch("/CustomerProfile/UpdateMaximumWash", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(model)
    });

    if (response.ok) {
        refreshMaximumWash();
    } else {
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
    $(button).next(".cancelEditMaxWash").show();
}

async function deleteMaximumWash(custId, itemCode) {
    if (!confirm("Delete this record?")) return;

    try {
        const response = await fetch(
            `/CustomerProfile/DeleteMaximumWash?custId=${custId}&itemCode=${encodeURIComponent(itemCode)}`,
            { method: "DELETE" }
        );

        if (!response.ok) throw new Error();
        refreshMaximumWash();
    } catch {
        alert("Delete failed.");
    }
}

$(document)
    .on("click", "#btnAddMaxWash", function () { enableNewMaximumWash(); })
    .on("click", "#btnCancelMaxWash", function () { cancelNewMaximumWash(); })
    .on("click", "#btnSaveMaxWash", function () { saveNewMaximumWash(); })
    .on("click", ".editMaxWash", function () {
        if ($(this).text().trim() === "Edit") {
            enableMaximumWashEdit(this);
        } else {
            updateMaximumWash(this);
        }
    })
    .on("click", ".cancelEditMaxWash", function () { refreshMaximumWash(); })
    .on("click", ".deleteMaxWash", function () {
        deleteMaximumWash($(this).data("custid"), $(this).data("itemcode"));
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
        const response = await fetch(`/CustomerProfile/CustomerLineComments?custId=${customer.CustId}`);
        if (!response.ok) throw new Error();
        $("#CustomerModuleContainer").html(await response.text());
    } catch {
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
    $(button).next(".cancelCustLineComment").show();
    $("#btnExitCustLineComments").prop("disabled", true);
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
        const response = await fetch("/CustomerProfile/SaveCustomerLineComments", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(model)
        });

        if (!response.ok) throw new Error();
        refreshCustomerLineComments();
    } catch {
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
        if ($(this).text().trim() === "Edit") enableCustomerLineCommentEdit(this);
        else saveCustomerLineComment(this);
    })
    .off("click", ".cancelCustLineComment")
    .on("click", ".cancelCustLineComment", cancelCustomerLineCommentEdit)
    .off("click", "#btnExitCustLineComments")
    .on("click", "#btnExitCustLineComments", exitCustomerLineComments);

// =====================================================
// Main Navigation
// =====================================================

function openCustomer() { ensureCustomerSelected("Customer"); }
function openWearer() { ensureCustomerSelected("Wearer"); }
function openProfile() { ensureCustomerSelected("Profile"); }

function registerNavigation() {
    $("#tabCustomer").off("click").on("click", function () { openCustomer(); });
    $("#tabWearer").off("click").on("click", function () { openWearer(); });
    $("#tabProfile").off("click").on("click", function () { openProfile(); });
    $("#tabCTS").off("click").on("click", function () { ensureCustomerSelected("CTSSettings"); });
    $("#tabCustLineComments").off("click").on("click", function () { ensureCustomerSelected("CustomerLineComments"); });

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
    .on("click", "#btnExitMaxWash", function () { openProfile(); })
    .on("click", "#btnExitCTS", function () { openProfile(); })
    .on("click", "#btnExitCustomer", function () { openCustomer(); })
    .on("click", "#btnExitWearer", function () { openCustomer(); })
    .off("click", "#btnMaxWash")
    .on("click", "#btnMaxWash", function () { openModule("MaximumWash"); })
    .off("click", "#btnLineComments")
    .on("click", "#btnLineComments", function () { openModule("CustomerLineComments"); });

function showSuccess(message) { alert(message); }
function showError(message) { alert(message); }
function isCustomerSelected() { return getSelectedCustomer() !== null; }
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

function resetModule() { clearRequestedModule(); }
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


// =====================================================
// Billing Module
// =====================================================

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
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(model)
    });

    const result = await response.text();

    if (result === "1") {
        openCustomerProfileModule("Billing");
    } else if (result === "2") {
        alert("This Charge Type already exists for this customer.");
    } else {
        alert("Unable to save Billing.");
    }
}

async function deleteBilling(id) {
    if (!id) {
        alert("Invalid Billing ID.");
        return;
    }

    if (!confirm("Delete record?")) return;

    try {
        const response = await fetch(
            `/CustomerProfile/DeleteBilling?billingChargesId=${encodeURIComponent(id)}`,
            { method: "DELETE" }
        );

        const result = await response.text();
        console.log("Delete Billing:", response.status, result);

        if (!response.ok) {
            alert(result || "Delete failed.");
            return;
        }

        if (result.trim() === "1") {
            await openCustomerProfileModule("Billing");
        } else {
            alert("Unable to delete Billing.");
        }
    } catch (error) {
        console.error("Delete Billing error:", error);
        alert("Delete failed.");
    }
}

$(document)
    .off("click", "#btnSaveBilling")
    .on("click", "#btnSaveBilling", function () { saveBilling(); })
    .off("click", ".deleteBilling")
    .on("click", ".deleteBilling", function () { deleteBilling($(this).data("id")); })
    .off("click", "#btnExitBilling")
    .on("click", "#btnExitBilling", function () { openModule("Profile"); })
    .off("click", "#lnkBilling")
    .on("click", "#lnkBilling", function () {
        $(".legacy-menu-item").removeClass("active");
        $(this).addClass("active");
        openCustomerProfileModule("Billing");
    });

function initializeBilling() {
    let rawComment = $("#hdnBillingComment").val();
    if (!rawComment && $("#txtBillingNotes").length) {
        rawComment = $("#txtBillingNotes").val() || "";
    }
    rawComment = (rawComment || "").trim();

    let parts = rawComment.split(",");
    let textParts = [];

    for (let i = 0; i < parts.length; i++) {
        let chunk = parts[i].trim();
        if (/^.+:\s*\d+(\.\d+)?$/.test(chunk)) {
            break;
        }
        if (chunk) {
            textParts.push(chunk);
        }
    }

    let customerNote = textParts.join(", ").trim();
    $("#txtBillingNotes").data("comment", customerNote);

    refreshBillingNotes();

    $("#newBillingRow").hide();
    $("#btnCancelBilling").hide();
}

function refreshBillingNotes() {
    let customerComment = $("#txtBillingNotes").data("comment");

    if (customerComment === undefined || customerComment === null) {
        customerComment = "";
    }

    let entries = [];

    $("#tblBilling tbody tr").not("#newBillingRow").each(function () {
        const charge = $(this).find("td:eq(0)").text().trim();
        const amount = $(this).find("td:eq(1)").text().trim();

        if (charge && amount) {
            entries.push(`${charge}:${amount}`);
        }
    });

    let displayOutput = customerComment.trim();

    if (entries.length > 0) {
        if (displayOutput !== "") {
            displayOutput += "\n\n";
        }
        displayOutput += entries.join("\n");
    }

    $("#txtBillingNotes").val(displayOutput);
}

function getBillingPayloadComment() {
    let currentText = $("#txtBillingNotes").val() || "";
    let lines = currentText.split(/\r?\n/);
    let noteLines = [];

    for (let line of lines) {
        let trimmed = line.trim();
        if (!trimmed) continue;

        if (/^.+:\s*\d+(\.\d+)?$/.test(trimmed)) {
            continue;
        }
        noteLines.push(trimmed);
    }

    let customerNote = noteLines.join(" ").trim();
    let tableEntries = [];

    $("#tblBilling tbody tr").not("#newBillingRow").each(function () {
        const charge = $(this).find("td:eq(0)").text().trim();
        const amount = $(this).find("td:eq(1)").text().trim();

        if (charge && amount) {
            tableEntries.push(`${charge}:${amount}`);
        }
    });

    let finalBillingCom = customerNote;
    if (tableEntries.length > 0) {
        if (finalBillingCom !== "") {
            finalBillingCom += ",";
        }
        finalBillingCom += tableEntries.join(",");
    }

    return finalBillingCom;
}

$(document)
    .off("click.billingAdd", "#btnAddBilling")
    .on("click.billingAdd", "#btnAddBilling", function (e) {
        e.preventDefault();
        e.stopPropagation();
        $("#newBillingRow").show();
        $("#btnCancelBilling").show();
    })
    .off("click.billingCancel", "#btnCancelBilling")
    .on("click.billingCancel", "#btnCancelBilling", function (e) {
        e.preventDefault();
        $("#newBillingRow").hide();
        $("#btnCancelBilling").hide();
        $("#txtChargeAmount").val("");
    })
    .off("click", ".legacy-menu-item")
    .on("click", ".legacy-menu-item", function () {
        $(".legacy-menu-item").removeClass("active");
        $(this).addClass("active");
    });

// =====================================================
// Packout Module
// =====================================================

$(document)
    .off("click", "#lnkPackout")
    .on("click", "#lnkPackout", function () {
        $(".legacy-menu-item").removeClass("active");
        $(this).addClass("active");
        openCustomerProfileModule("Packout");
    });

function initializePackout() {
    const existingText = $("#txtPackoutNotes").val() || "";

    if ($("#hdnPackoutComment").length) {
        $("#txtPackoutNotes").data("comment", $("#hdnPackoutComment").val() || "");
    } else if ($("#txtPackoutNotes").data("comment") === undefined) {
        $("#txtPackoutNotes").data("comment", existingText);
    }

    $("#newPackoutRow").hide();
    refreshPackoutNotes();
}

$(document)
    .off("click.packout", "#btnAddPackout")
    .on("click.packout", "#btnAddPackout", function () {
        $("#newPackoutRow").show();
        $("#txtPackoutColor").val("");
        $("#txtPackoutSize").val("");
    });

async function savePackout() {
    const customer = getSelectedCustomer();
    if (!customer) {
        alert("Please select a customer.");
        return;
    }

    const item = $("#ddlPackoutItem").val();
    const restriction = $("#ddlPackoutRestriction").val();

    if (!item) return alert("Please select an Item.");
    if (!restriction) return alert("Please select a Packout Restriction.");

    const model = {
        CustId: customer.CustId,
        PkoutRestrict: restriction,
        Item: item,
        Color: $("#txtPackoutColor").val().trim(),
        Size: $("#txtPackoutSize").val().trim(),
        UpdtUser: 1
    };

    try {
        const response = await fetch("/CustomerProfile/SavePackout", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(model)
        });

        const result = await response.text();

        if (!response.ok) {
            alert(result || "Unable to save Packout.");
            return;
        }

        $("#newPackoutRow").hide();
        $("#txtPackoutColor").val("");
        $("#txtPackoutSize").val("");

        await openCustomerProfileModule("Packout");
    } catch (error) {
        console.error("Packout save error:", error);
        alert("Unable to save Packout.");
    }
}

$(document)
    .off("click.packout", "#btnSavePackout")
    .on("click.packout", "#btnSavePackout", function () { savePackout(); });

async function deletePackout(id) {
    if (!id) return alert("Invalid Packout ID.");
    if (!confirm("Delete record?")) return;

    try {
        const response = await fetch(
            `/CustomerProfile/DeletePackout?pkoutRestrictId=${encodeURIComponent(id)}`,
            { method: "DELETE" }
        );

        const result = await response.text();

        if (!response.ok) {
            alert(result || "Delete failed.");
            return;
        }

        await openCustomerProfileModule("Packout");
    } catch (error) {
        console.error("Delete Packout error:", error);
        alert("Delete failed.");
    }
}

$(document)
    .off("click.packout", ".deletePackout")
    .on("click.packout", ".deletePackout", function () {
        deletePackout($(this).data("id"));
    });

function refreshPackoutNotes() {
    const comment = $("#hdnPackoutComment").val() || "";
    let rows = [];

    $("#tblPackout tbody tr")
        .not("#newPackoutRow")
        .each(function () {
            const restriction = $(this).find("td:eq(0)").text().trim();
            const item = $(this).find("td:eq(1)").text().trim();
            const color = $(this).find("td:eq(2)").text().trim();
            const size = $(this).find("td:eq(3)").text().trim();

            if (restriction && item) {
                let line = restriction + " : " + item;
                if (color) line += " - " + color;
                if (size) line += " (" + size + ")";
                rows.push(line);
            }
        });

    let finalText = comment;
    if (rows.length > 0) {
        if (finalText !== "") finalText += "\n\n";
        finalText += rows.join("\n");
    }

    $("#txtPackoutNotes").val(finalText);
}

// =====================================================
// Wash Module
// =====================================================

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

    if ($("#newWashRow").is(":visible")) {
        const garmentType = $("#ddlWashGarmentType").val();
        const formula = $("#txtWashFormula").val().trim();

        if (!garmentType) return alert("Please select Garment Type.");
        if (!formula) return alert("Formula cannot be empty.");
    }

    const formulaParts = [];

    $("#tblWash tbody tr")
        .not("#newWashRow")
        .each(function () {
            const garmentType = $(this).find("td:eq(0)").text().trim();
            const formula = $(this).find("td:eq(1)").text().trim();

            if (!garmentType) return;
            formulaParts.push(garmentType, formula);
        });

    if ($("#newWashRow").is(":visible")) {
        const garmentType = $("#ddlWashGarmentType").val();
        const formula = $("#txtWashFormula").val().trim();

        const exists = $("#tblWash tbody tr")
            .not("#newWashRow")
            .filter(function () {
                return $(this).find("td:eq(0)").text().trim().toLowerCase() === garmentType.toLowerCase();
            }).length > 0;

        if (exists) {
            alert(garmentType + " already exists.");
            return;
        }

        formulaParts.push(garmentType, formula);
    }

    const formula = formulaParts.join(",");
    const washCom = $("#txtWashNotes").val() || "";

    const payload = {
        CustId: customer.CustId,
        WashCom: washCom,
        Formula: formula
    };

    const response = await fetch("/CustomerProfile/SaveWash", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    if (response.ok) {
        $("#newWashRow").hide();
        $("#txtWashFormula").val("");
        openCustomerProfileModule("Wash");
    } else {
        alert("Unable to save Wash.");
    }
}

$(document)
    .off("click", "#btnSaveWash")
    .on("click", "#btnSaveWash", function () { saveWash(); })
    .off("click", ".deleteWash")
    .on("click", ".deleteWash", async function () {
        if (!confirm("Delete this Wash formula?")) return;
        $(this).closest("tr").remove();
        await saveWash();
    })
    .off("click", "#lnkWash")
    .on("click", "#lnkWash", function () {
        $(".legacy-menu-item").removeClass("active");
        $(this).addClass("active");
        openCustomerProfileModule("Wash");
    });

function getCurrentWashFormula(savedCustomer) {
    if (!$("#tblWash").length) {
        return savedCustomer.formula || "";
    }

    const parts = [];
    $("#tblWash tbody tr")
        .not("#newWashRow")
        .each(function () {
            const garmentType = $(this).find("td:eq(0)").text().trim();
            const formula = $(this).find("td:eq(1)").text().trim();

            if (garmentType && formula) {
                parts.push(garmentType, formula);
            }
        });

    return parts.join(",");
}

// =====================================================
// Customer Screen Save
// =====================================================

$(document)
    .off("click", "#btnSaveCustomer")
    .on("click", "#btnSaveCustomer", async function () {
        const customer = getSelectedCustomer();
        if (!customer) {
            alert("Please select a customer.");
            return;
        }

        const payload = {
            CustId: customer.CustId,
            OSSFlag: $("#ossFlag").is(":checked"),
            STFlag: $("#stFlag").is(":checked")
        };

        const response = await fetch("/CustomerProfile/SaveCustomerFlags", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (response.ok) {
            alert("Customer details saved successfully.");
        } else {
            alert("Unable to save Customer details.");
        }
    });


// =====================================================
// WEARER MODULE
// =====================================================

function getWearerTargetContainer() {
    return $("#CustomerModuleContainer").length ? $("#CustomerModuleContainer") : $("#workspace");
}

// 1. OPEN WEARER MODAL (Injects _WearerSelection.cshtml)
$(document)
    .off("click.wearer", "#btnOpenWearerModal")
    .on("click.wearer", "#btnOpenWearerModal", function () {
        const customer = getSelectedCustomer();
        if (!customer) {
            alert("Please select a customer first.");
            return;
        }

        // Call the SearchModal action on WearerController
        $.get(`/Wearer/SearchModal?custId=${customer.CustId}`)
            .done(function (html) {
                // Clear any lingering instance
                const existingEl = document.getElementById("wearerSelectionModal");
                if (existingEl) {
                    const inst = bootstrap.Modal.getInstance(existingEl);
                    if (inst) inst.dispose();
                }

                // Render _WearerSelection.cshtml into the placeholder
                $("#wearerModalContainer").html(html);

                // Show modal
                const modalElement = document.getElementById("wearerSelectionModal");
                if (modalElement) {
                    const modal = new bootstrap.Modal(modalElement);
                    modal.show();
                }
            })
            .fail(function () {
                alert("Unable to load wearer list.");
            });
    });

// 2. SELECT WEARER FROM _WearerSelection TABLE
$(document)
    .off("click.wearer", ".btnSelectWearer")
    .on("click.wearer", ".btnSelectWearer", function () {
        const customer = getSelectedCustomer();
        if (!customer) return alert("Please select a customer.");

        const wearNbr = $(this).attr("data-wear-nbr");
        if (!wearNbr) return console.error("Wearer number missing.");

        // Hide and dispose modal
        const modalElement = document.getElementById("wearerSelectionModal");
        if (modalElement) {
            const modal = bootstrap.Modal.getInstance(modalElement);
            if (modal) modal.hide();
        }

        const url = `/Wearer/Index?custId=${customer.CustId}&wearNbr=${encodeURIComponent(wearNbr)}`;

        $.get(url, function (html) {
            const $container = $("#CustomerModuleContainer").length ? $("#CustomerModuleContainer") : $("#workspace");
            $container.html(html);
            afterModuleLoaded("Wearer");
        }).fail(function (xhr) {
            console.error("Loading selected wearer failed:", xhr.status, xhr.responseText);
            alert("Unable to load selected wearer.");
        });
    });
// 3. NEXT WEARER
$(document)
    .off("click.wearer", "#btnNext")
    .on("click.wearer", "#btnNext", function () {
        const customer = getSelectedCustomer();
        let custId = parseInt($("#hdnCustId").val() || $("input[name='CustId']").val() || (customer ? customer.CustId : 0));
        let wearerId = parseInt($("#hdnWearerId").val() || $("input[name='WearerId']").val() || 0);

        if (!custId) return alert("Please select a customer first.");

        $.get("/CustomerProfile/WearerNext", { custId: custId, wearerId: wearerId })
            .done(function (html) {
                if (html) {
                    getWearerTargetContainer().html(html);
                    afterModuleLoaded("Wearer");
                }
            })
            .fail(function (xhr) {
                console.error("Wearer Next failed:", xhr.status, xhr.responseText);
            });
    });

// 4. PREVIOUS WEARER
$(document)
    .off("click.wearer", "#btnPrevious")
    .on("click.wearer", "#btnPrevious", function () {
        const customer = getSelectedCustomer();
        let custId = parseInt($("#hdnCustId").val() || $("input[name='CustId']").val() || (customer ? customer.CustId : 0));
        let wearerId = parseInt($("#hdnWearerId").val() || $("input[name='WearerId']").val() || 0);

        if (!custId) return alert("Please select a customer first.");

        $.get("/CustomerProfile/WearerPrevious", { custId: custId, wearerId: wearerId })
            .done(function (html) {
                if (html) {
                    getWearerTargetContainer().html(html);
                    afterModuleLoaded("Wearer");
                }
            })
            .fail(function (xhr) {
                console.error("Wearer Previous failed:", xhr.status, xhr.responseText);
            });
    });

// 5. SAVE WEARER
$(document)
    .off("click.wearer", "#btnSaveWearer")
    .on("click.wearer", "#btnSaveWearer", function () {
        const wearerId = parseInt($("#hdnWearerId").val() || $("input[name='WearerId']").val());

        if (!wearerId || isNaN(wearerId)) {
            alert("No wearer selected.");
            return;
        }

        const dto = {
            WearerId: wearerId,
            Locker: ($("input[name='Locker']").val() || $("#Locker").val() || "").trim(),
            LockRm: ($("input[name='LockRm']").val() || $("#LockRm").val() || "").trim(),
            Sex: ($("select[name='Sex']").val() || $("#Sex").val()) === "true"
        };

        $.ajax({
            url: "/CustomerProfile/SaveWearer",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dto),
            success: function () {
                alert("Wearer saved successfully.");
            },
            error: function (xhr) {
                console.error("Save Wearer failed:", xhr.status, xhr.responseText);
                alert("Save Wearer failed.");
            }
        });
    });

// 4. SAVE WEARER
$(document)
    .off("click.wearer", "#btnSaveWearer")
    .on("click.wearer", "#btnSaveWearer", function () {
        const wearerId = parseInt($("#hdnWearerId").val() || $("input[name='WearerId']").val() || $("#WearerId").val());

        if (!wearerId || isNaN(wearerId)) {
            alert("No wearer selected.");
            return;
        }

        const dto = {
            WearerId: wearerId,
            Locker: $("input[name='Locker']").val() ? $("input[name='Locker']").val().trim() : ($("#Locker").val() || "").trim(),
            LockRm: $("input[name='LockRm']").val() ? $("input[name='LockRm']").val().trim() : ($("#LockRm").val() || "").trim(),
            Sex: ($("select[name='Sex']").val() || $("#Sex").val()) === "true"
        };

        $.ajax({
            url: "/CustomerProfile/SaveWearer",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dto),
            success: function () {
                alert("Wearer saved successfully.");
            },
            error: function (xhr) {
                console.error("Save Wearer failed:", xhr.status, xhr.responseText);
                alert("Save Wearer failed.");
            }
        });
    });

// =====================================================
// Overall Customer Profile Save
// =====================================================

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

        let savedCustomer;
        try {
            const response = await fetch(`/api/Customer/${customer.CustId}`);
            if (!response.ok) throw new Error("Unable to load customer data.");
            savedCustomer = await response.json();
        } catch (error) {
            console.error("Unable to load latest customer:", error);
            alert("Unable to load current customer data.");
            return;
        }

        // 1. BILLING
        let billingCom = savedCustomer.billingCom || "";
        if ($("#txtBillingNotes").length && $("#tblBilling").length) {
            billingCom = getBillingPayloadComment();
        }

        // 2. PACKOUT
        let packoutCom = savedCustomer.packoutCom || "";
        if ($("#txtPackoutNotes").length) {
            let packoutText = $("#txtPackoutNotes").val() || "";
            let lines = packoutText.split(/\r?\n/);
            let commentLines = [];

            for (const line of lines) {
                const value = line.trim();
                if (!value) continue;
                if (/^(YES|NO)\s*:/i.test(value)) continue;
                commentLines.push(value);
            }
            packoutCom = commentLines.join("\n").trim();
        }

        // 3. OTHER COMMENTS
        const washCom = $("#txtWashNotes").length ? $("#txtWashNotes").val() : (savedCustomer.washCom || "");
        const soilCom = $("#txtSoilNotes").length ? $("#txtSoilNotes").val() : (savedCustomer.soilCom || "");
        const dryerCom = $("#txtDryerNotes").length ? $("#txtDryerNotes").val() : (savedCustomer.dryerCom || "");
        const receivingCom = $("#txtReceivingNotes").length ? $("#txtReceivingNotes").val() : (savedCustomer.receivingCom || "");
        const shippingCom = $("#txtShippingNotes").length ? $("#txtShippingNotes").val() : (savedCustomer.shippingCom || "");
        const driverCom = $("#txtDriverNotes").length ? $("#txtDriverNotes").val() : (savedCustomer.driverCom || "");
        const mendCom = $("#txtMendNotes").length ? $("#txtMendNotes").val() : (savedCustomer.mendCom || "");
        const qaCom = $("#txtQANotes").length ? $("#txtQANotes").val() : (savedCustomer.qaCom || "");
        const custSrvCom = $("#txtCustomerServiceNotes").length ? $("#txtCustomerServiceNotes").val() : (savedCustomer.custSrvCom || "");
        const officeCom = $("#txtOfficeNotes").length ? $("#txtOfficeNotes").val() : (savedCustomer.officeCom || "");
        const genOfficeCom = $("#txtGeneralOfficeNotes").length ? $("#txtGeneralOfficeNotes").val() : (savedCustomer.genOfficeCom || "");
        const merControlCom = $("#txtMerchControlNotes").length ? $("#txtMerchControlNotes").val() : (savedCustomer.merControlCom || "");
        const mainCleanRoomCom = $("#txtMainCleanRoomNotes").length ? $("#txtMainCleanRoomNotes").val() : (savedCustomer.mainCleanRoomCom || "");
        const qaInspCom = $("#txtQAInspectorNotes").length ? $("#txtQAInspectorNotes").val() : (savedCustomer.qaInspCom || "");
        const prodCom = $("#txtProductionNotes").length ? $("#txtProductionNotes").val() : (savedCustomer.prodCom || "");
        const formula = getCurrentWashFormula(savedCustomer);

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

        $.ajax({
            url: "/CustomerProfile/SaveCustomerProfile",
            type: "POST",
            contentType: "application/json",
            data: JSON.stringify(payload),
            success: function () {
                alert("Customer Profile Saved Successfully.");
            },
            error: function (xhr) {
                alert(xhr.responseText || "Unable to save Customer Profile.");
            }
        });
    });