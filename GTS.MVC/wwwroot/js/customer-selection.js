/**
 * GTS Garment Tracking System
 * Customer Selection, Modal Search & Module Navigation
 */

// Global tracker for currently selected customer
window.currentSelectedCustId = 0;

$(document).ready(function () {

    // =========================================================================
    // 1. OPEN CUSTOMER SELECTION MODAL
    // =========================================================================
    $(document).on('click', '#btnSelectCustomer, #btnTriggerModal', function (e) {
        e.preventDefault();

        const modalEl = document.getElementById('customerSelectionModal');
        if (modalEl) {
            const modal = bootstrap.Modal.getOrCreateInstance(modalEl);
            modal.show();

            // Auto-load customers on first open if grid is empty
            if ($('#tblCustomerBody tr').length <= 1) {
                executeCustomerSearch(false);
            }
        }
    });

    // =========================================================================
    // 2. SEARCH & FILTER IN MODAL
    // =========================================================================
    // Search Button Click
    $(document).on('click', '#btnFilterCustomers', function (e) {
        e.preventDefault();
        executeCustomerSearch(false);
    });

    // Show All Button Click
    $(document).on('click', '#btnShowAllCustomers', function (e) {
        e.preventDefault();
        executeCustomerSearch(true);
    });

    // Reset / Clear Button Click
    $(document).on('click', '#btnResetFilter', function (e) {
        e.preventDefault();
        $('#custSearchNbr').val('');
        $('#custSearchName').val('');
        $('#custSearchRoute').val('');
        // Keep default MC intact (561) or clear if needed
        executeCustomerSearch(false);
    });

    // Trigger Search on Enter Key in any search input
    $(document).on('keypress', '#custSearchMC, #custSearchNbr, #custSearchName, #custSearchRoute', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            executeCustomerSearch(false);
        }
    });

    // =========================================================================
    // 3. CUSTOMER ROW SELECTION (From _Search.cshtml)
    // =========================================================================
    $(document).on('click', '.select-customer', function (e) {
        e.preventDefault();

        const $btn = $(this);
        const custId = parseInt($btn.data('custid'), 10);
        const custNbr = $btn.data('custno');
        const custName = $btn.data('name');
        const route = $btn.data('route');
        const gid = $btn.data('gid');

        if (!custId || isNaN(custId)) {
            alert('Invalid Customer ID on selected record.');
            return;
        }

        // Store selected CustId globally
        window.currentSelectedCustId = custId;

        // Populate visible header inputs
        $('#custNo').val(custNbr);
        $('#custName').val(custName);
        $('#route').val(route);
        $('#gid').val(gid);
        $('#hdnCustId').val(custId);

        // Hide the Bootstrap selection modal
        const modalEl = document.getElementById('customerSelectionModal');
        if (modalEl) {
            const modal = bootstrap.Modal.getInstance(modalEl);
            if (modal) modal.hide();
        }

        // Identify which top tab is currently highlighted, defaulting to Profile
        let activeAction = 'Profile';
        const $activeTab = $('.gts-tabs a.active');
        if ($activeTab.length > 0) {
            activeAction = getActionFromTabId($activeTab.attr('id'));
        } else {
            $('.gts-tabs a').removeClass('active');
            $('#tabProfile').addClass('active');
        }

        // Load the chosen view into workspace
        loadWorkspaceSubmodule(custId, activeAction);
    });

    // =========================================================================
    // 4. TOP 4 TABS NAVIGATION (Customer | Wearer | Profile | CTS Settings)
    // =========================================================================
    $(document).on('click', '.gts-tabs a', function (e) {
        e.preventDefault();

        const $clickedTab = $(this);
        const tabId = $clickedTab.attr('id');
        const action = getActionFromTabId(tabId);

        // Highlight active tab
        $('.gts-tabs a').removeClass('active');
        $clickedTab.addClass('active');

        // Check if a customer is selected
        const custId = window.currentSelectedCustId || parseInt($('#hdnCustId').val(), 10) || parseInt($('#custNo').val(), 10) || 0;

        if (custId > 0) {
            loadWorkspaceSubmodule(custId, action);
        } else {
            // If no customer is loaded yet, pop the selection modal
            const modalEl = document.getElementById('customerSelectionModal');
            if (modalEl) {
                bootstrap.Modal.getOrCreateInstance(modalEl).show();
                executeCustomerSearch(false);
            }
        }
    });

    // =========================================================================
    // 5. PROFILE MODULE INNER MENU (Billing, Wash, Packout, etc.)
    // =========================================================================
    $(document).on('click', '.legacy-menu-item, #btnSpecialLines', function (e) {
        e.preventDefault();

        $('.legacy-menu-item').removeClass('active');
        $(this).addClass('active');

        const menuId = $(this).attr('id');
        const custId = window.currentSelectedCustId || parseInt($('#hdnCustId').val(), 10) || 0;

        if (!custId) {
            alert('Please select a customer first.');
            return;
        }

        let innerAction = 'Billing';
        if (menuId === 'lnkBilling') innerAction = 'Billing';
        else if (menuId === 'lnkPackout') innerAction = 'Packout';
        else if (menuId === 'lnkWash') innerAction = 'Wash';
        else if (menuId === 'btnSpecialLines') innerAction = 'SpecialLines';
        else {
            // For other submenus without dedicated views yet, show clean placeholder
            $('#CustomerModuleContainer').html(`
                <div class="alert alert-info my-3">
                    Submodule <strong>${$(this).text().trim()}</strong> is currently under integration.
                </div>
            `);
            return;
        }

        loadInnerModuleContainer(custId, innerAction);
    });
});

// =============================================================================
// HELPER FUNCTIONS
// =============================================================================

/**
 * Maps Tab Element IDs to Controller Actions
 */
function getActionFromTabId(tabId) {
    switch (tabId) {
        case 'tabCustomer': return 'Customer';
        case 'tabWearer': return 'Wearer';
        case 'tabCTS': return 'CTSSettings';
        case 'tabProfile':
        default:
            return 'Profile';
    }
}

/**
 * Executes Ajax call for Modal Customer Search
 */
function executeCustomerSearch(showAll) {
    const $tbody = $('#tblCustomerBody');
    $tbody.html(`
        <tr>
            <td colspan="10" class="text-center py-3">
                <div class="spinner-border spinner-border-sm text-primary" role="status"></div>
                <span class="ms-2 text-muted">Searching database records...</span>
            </td>
        </tr>
    `);

    const mcInput = parseInt($('#custSearchMC').val(), 10);
    const mc = (!isNaN(mcInput) && mcInput > 0) ? mcInput : 561;

    const queryParams = $.param({
        marketCenter: mc,
        searchId: parseInt($('#custSearchNbr').val(), 10) || 0,
        searchName: $('#custSearchName').val()?.trim() || '',
        route: parseInt($('#custSearchRoute').val(), 10) || 0,
        showAll: showAll
    });

    $.ajax({
        url: `/CustomerProfile/Search?${queryParams}`,
        type: 'GET',
        cache: false,
        success: function (html) {
            // Check if HTML is a wrapped table or plain rows
            if (html.indexOf('<table') > -1) {
                const rows = $(html).find('tbody').html() || html;
                $tbody.html(rows);
            } else {
                $tbody.html(html);
            }
        },
        error: function (xhr) {
            $tbody.html(`
                <tr>
                    <td colspan="10" class="text-danger text-center py-2">
                        Failed to fetch customers (${xhr.status}: ${xhr.statusText}).
                    </td>
                </tr>
            `);
        }
    });
}

/**
 * Injects Selected Top Module Partial into #workspace
 */
function loadWorkspaceSubmodule(custId, action) {
    const $workspace = $('#workspace');

    $workspace.html(`
        <div class="text-center py-5">
            <div class="spinner-border text-primary" role="status"></div>
            <div class="text-muted mt-2 small">Loading ${action}...</div>
        </div>
    `);

    $.ajax({
        url: `/CustomerProfile/${action}?custId=${custId}`,
        type: 'GET',
        cache: false,
        success: function (response) {
            $workspace.html(response);
        },
        error: function (xhr) {
            console.error(`Failed to load ${action}:`, xhr);
            $workspace.html(`
                <div class="alert alert-danger my-3">
                    <strong>Error loading ${action}:</strong> Server returned status ${xhr.status} (${xhr.statusText}).
                </div>
            `);
        }
    });
}

/**
 * Injects Sub-modules (Billing, Wash, Packout) into Profile's #CustomerModuleContainer
 */
function loadInnerModuleContainer(custId, action) {
    const $innerContainer = $('#CustomerModuleContainer');
    if ($innerContainer.length === 0) return;

    $innerContainer.html(`
        <div class="text-center py-4">
            <div class="spinner-border spinner-border-sm text-primary" role="status"></div>
            <div class="text-muted mt-1 small">Loading ${action}...</div>
        </div>
    `);

    $.ajax({
        url: `/CustomerProfile/${action}?custId=${custId}`,
        type: 'GET',
        cache: false,
        success: function (response) {
            $innerContainer.html(response);
        },
        error: function (xhr) {
            console.error(`Failed to load ${action}:`, xhr);
            $innerContainer.html(`
                <div class="alert alert-danger my-2 small">
                    Failed to load ${action} (${xhr.status}).
                </div>
            `);
        }
    });
}