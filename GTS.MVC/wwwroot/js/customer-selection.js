let customerModalInstance = null;
let onCustomerSelectCallback = null;

function openCustomerSelection(marketCenter = 569, onSelectCallback = null) {
    onCustomerSelectCallback = onSelectCallback;

    if (!customerModalInstance) {
        customerModalInstance = new bootstrap.Modal(document.getElementById('customerSelectionModal'));
    }

    // Default initial fetch: all active customers for this Market Center
    fetchFilteredCustomers({
        numRecsToFetch: 50,
        marketCenter: marketCenter,
        route: 0,
        wDay: 0,
        gid: "",
        custNbr: 0,
        custName: "",
        wearerNbr: 0
    });

    customerModalInstance.show();
}

async function fetchFilteredCustomers(payload) {
    const tbody = document.getElementById("tblCustomerBody");
    tbody.innerHTML = `<tr><td colspan="9" class="text-center py-3">Loading records...</td></tr>`;

    try {
        const response = await fetch('/api/customer/customers', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (!response.ok) throw new Error('Failed to retrieve customer list.');

        const customers = await response.json();
        renderCustomerTable(customers);
    } catch (err) {
        tbody.innerHTML = `<tr><td colspan="9" class="text-danger text-center py-3">${err.message}</td></tr>`;
    }
}

function renderCustomerTable(customers) {
    const tbody = document.getElementById("tblCustomerBody");
    tbody.innerHTML = "";

    if (!customers || customers.length === 0) {
        tbody.innerHTML = `<tr><td colspan="9" class="text-center text-muted py-3">No customers found.</td></tr>`;
        return;
    }

    customers.forEach(cust => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>
                <button type="button" class="btn btn-outline-primary btn-sm py-0 px-2 btn-select-cust" 
                        data-custid="${cust.custId}" 
                        data-custnbr="${cust.custNbr}" 
                        data-name="${encodeURIComponent(cust.name || '')}">
                    Select
                </button>
            </td>
            <td class="fw-bold">${cust.custNbr}</td>
            <td>${cust.name || ''}</td>
            <td>${cust.route}</td>
            <td><code>${cust.gid || ''}</code></td>
            <td><span class="badge bg-secondary">${cust.wDay || '-'}</span></td>
            <td>${cust.city || ''}</td>
            <td>${cust.phone || ''}</td>
            <td>${cust.stopDt ? '<span class="badge bg-danger">Stopped</span>' : '<span class="badge bg-success">Active</span>'}</td>
        `;
        tbody.appendChild(row);
    });

    document.querySelectorAll(".btn-select-cust").forEach(btn => {
        btn.addEventListener("click", function () {
            const custId = this.dataset.custid;
            const custNbr = this.dataset.custnbr;
            const custName = decodeURIComponent(this.dataset.name);

            if (customerModalInstance) customerModalInstance.hide();

            if (typeof onCustomerSelectCallback === "function") {
                onCustomerSelectCallback({ custId, custNbr, custName });
            }
        });
    });
}

// Hook Search & Clear Buttons
document.getElementById("btnFilterCustomers")?.addEventListener("click", function () {
    const payload = {
        numRecsToFetch: 50,
        marketCenter: 569, // Or read dynamically from an MC selector on screen
        custNbr: parseInt(document.getElementById("custSearchNbr").value) || 0,
        custName: document.getElementById("custSearchName").value.trim(),
        route: parseInt(document.getElementById("custSearchRoute").value) || 0,
        wDay: 0,
        gid: "",
        wearerNbr: 0
    };
    fetchFilteredCustomers(payload);
});

document.getElementById("btnResetFilter")?.addEventListener("click", function () {
    document.getElementById("custSearchNbr").value = "";
    document.getElementById("custSearchName").value = "";
    document.getElementById("custSearchRoute").value = "";
    document.getElementById("btnFilterCustomers").click();
});