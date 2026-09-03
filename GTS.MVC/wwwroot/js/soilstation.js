document.addEventListener("DOMContentLoaded", function () {
    // 1. Element References
    const txtReceiver = document.getElementById("txtReceiver");
    const ddlPickUpDay = document.getElementById("ddlPickUpDay");
    const txtPickUpDate = document.getElementById("txtPickUpDate");
    const txtCustNo = document.getElementById("txtCustNo");
    const txtCustomerName = document.getElementById("txtCustomerName");
    const txtCreatedBy = document.getElementById("txtCreatedBy");

    const txtContainer = document.getElementById("txtContainer");
    const btnStartContainer = document.getElementById("btnStartContainer");

    const txtGarmentId = document.getElementById("txtGarmentId");
    const btnSpecialInstructions = document.getElementById("btnSpecialInstructions");

    const statValid = document.getElementById("statValid");
    const statNotOnFile = document.getElementById("statNotOnFile");
    const statDuplicates = document.getElementById("statDuplicates");
    const statDayErrors = document.getElementById("statDayErrors");
    const statReqPull = document.getElementById("statReqPull");

    const btnStartReceiver = document.getElementById("btnStartReceiver");
    const btnUndo = document.getElementById("btnUndo");
    const btnRepair = document.getElementById("btnRepair");
    const btnExit = document.getElementById("btnExit");

    // State Variables
    let isUndoMode = false;
    let isRepairMode = false;
    let currentReceiverId = null;

    const daysMap = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
    function getPickupDayNumber(dayName) {
        return daysMap.indexOf(dayName) + 1;
    }

    // Auto-load if seed receiver exists
    if (txtReceiver.value.trim() !== "") {
        loadReceiver(txtReceiver.value.trim());
    } else {
        txtReceiver.focus();
    }

    // ==========================================
    // 1. RECEIVER WORKFLOW
    // ==========================================
    txtReceiver.addEventListener("keydown", function (e) {
        if (e.key === "Enter" && this.value.trim() !== "") {
            e.preventDefault();
            loadReceiver(this.value.trim());
        }
    });

    async function loadReceiver(receiverId) {
        try {
            const res = await fetch(`/SoilStation/ValidateReceiver?receiverId=${receiverId}`);
            const data = await res.json();

            if (data.success && data.data) {
                const rcv = data.data;
                currentReceiverId = rcv.receiverId;
                txtCustNo.value = rcv.custNbr || "";
                txtCustomerName.value = rcv.customerName || "";
                txtCreatedBy.value = rcv.userName || "System";

                // Unlock container and buttons
                txtReceiver.disabled = true;
                txtContainer.disabled = false;
                btnStartContainer.disabled = false;
                btnSpecialInstructions.disabled = false;
                btnUndo.disabled = false;
                btnRepair.disabled = false;

                txtContainer.focus();
            } else {
                alert(data.message || "Receiver not found.");
                txtReceiver.select();
            }
        } catch (err) {
            console.error("Error loading receiver:", err);
        }
    }

    // ==========================================
    // 2. CONTAINER WORKFLOW
    // ==========================================
    txtContainer.addEventListener("keydown", async function (e) {
        if (e.key === "Enter" && this.value.trim() !== "") {
            e.preventDefault();
            const cntrId = this.value.trim();

            const res = await fetch(`/SoilStation/ValidateContainer?containerId=${cntrId}`);
            const data = await res.json();

            if (data.success) {
                txtContainer.disabled = true;
                txtGarmentId.disabled = false;
                txtGarmentId.placeholder = "Scan Garment...";
                txtGarmentId.focus();
            } else {
                alert(data.message || "Invalid container.");
                this.select();
            }
        }
    });

    // Start New Container Button
    btnStartContainer.addEventListener("click", function () {
        txtContainer.disabled = false;
        txtContainer.value = "";
        txtGarmentId.disabled = true;
        txtGarmentId.placeholder = "Scan container first...";
        txtContainer.focus();
    });

    // ==========================================
    // 3. GARMENT SCAN WORKFLOW (SCAN / UNDO / REPAIR)
    // ==========================================
    txtGarmentId.addEventListener("keydown", async function (e) {
        if (e.key === "Enter" && this.value.trim() !== "") {
            e.preventDefault();
            const barcode = this.value.trim();
            this.value = "";

            // If Undo Mode is Active
            if (isUndoMode) {
                await executeUndo(barcode);
                return;
            }

            // Normal Scan or Repair Scan
            const payload = {
                garmentBarcode: barcode,
                receiverId: currentReceiverId,
                containerId: txtContainer.value.trim(),
                pickupDay: getPickupDayNumber(ddlPickUpDay.value),
                userId: 1,
                isRepair: isRepairMode
            };

            const res = await fetch("/SoilStation/ScanGarment", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            const result = await res.json();

            if (result.success) {
                statValid.value = parseInt(statValid.value) + 1;
                if (isRepairMode) {
                    toggleRepair(); // Turn repair mode off after tagging the garment
                }
            } else {
                if (result.statusCode === "NOT_FOUND") {
                    statNotOnFile.value = parseInt(statNotOnFile.value) + 1;
                } else if (result.statusCode === "DAY_ERROR") {
                    statDayErrors.value = parseInt(statDayErrors.value) + 1;
                } else if (result.statusCode === "MAX_WASH") {
                    statReqPull.value = parseInt(statReqPull.value) + 1;
                } else {
                    statDuplicates.value = parseInt(statDuplicates.value) + 1;
                }
                alert(result.message);
            }
            txtGarmentId.focus();
        }
    });

    // ==========================================
    // 4. ACTION BUTTONS (UNDO, REPAIR, RESET, EXIT)
    // ==========================================

    // Undo Button
    btnUndo.addEventListener("click", function () {
        isUndoMode = !isUndoMode;
        if (isUndoMode) {
            btnUndo.style.backgroundColor = "#ffc107";
            btnUndo.style.fontWeight = "bold";
            txtGarmentId.placeholder = "SCAN GARMENT TO UNDO...";
            txtGarmentId.style.border = "2px solid #ffc107";
        } else {
            resetActionButtons();
        }
        txtGarmentId.focus();
    });

    async function executeUndo(barcode) {
        const res = await fetch("/SoilStation/UndoScan", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                garmentBarcode: barcode,
                receiverId: currentReceiverId,
                userId: 1
            })
        });

        const result = await res.json();
        if (result.success) {
            if (parseInt(statValid.value) > 0) {
                statValid.value = parseInt(statValid.value) - 1;
            }
            alert(`Scan reversed for Garment: ${barcode}`);
        } else {
            alert(result.message || "Undo failed.");
        }
        resetActionButtons();
        txtGarmentId.focus();
    }

    // Repair Button
    btnRepair.addEventListener("click", toggleRepair);

    function toggleRepair() {
        isRepairMode = !isRepairMode;
        if (isRepairMode) {
            btnRepair.style.backgroundColor = "#dc3545";
            btnRepair.style.color = "#ffffff";
            btnRepair.innerText = "Repairing...";
            txtGarmentId.placeholder = "SCAN REPAIR ITEM...";
            txtGarmentId.style.border = "2px solid #dc3545";
        } else {
            resetActionButtons();
        }
        txtGarmentId.focus();
    }

    function resetActionButtons() {
        isUndoMode = false;
        isRepairMode = false;
        btnUndo.style.backgroundColor = "";
        btnUndo.style.fontWeight = "normal";
        btnRepair.style.backgroundColor = "";
        btnRepair.style.color = "";
        btnRepair.innerText = "Repair";
        txtGarmentId.style.border = "";
        txtGarmentId.placeholder = "Scan Garment...";
    }

    // Start New Receiver
    btnStartReceiver.addEventListener("click", function () {
        if (confirm("Start a new receiver? Current session counters will reset.")) {
            location.reload();
        }
    });

    // Special Instructions Modal
    const modalElement = document.getElementById("specialInstructionsModal");
    const specialModal = new bootstrap.Modal(modalElement);

    btnSpecialInstructions.addEventListener("click", async function () {
        if (!currentReceiverId) return;

        try {
            const res = await fetch(`/SoilStation/GetSpecialInstructions?receiverId=${currentReceiverId}`);
            const result = await res.json();

            // Handle both direct object or wrapper { success: true, data: { ... } }
            const data = result.data || result;

            if (data) {
                const wash = data.washComments || data.WashComments || "None";
                const soil = data.soilComments || data.SoilComments || "None";

                document.getElementById("lblWashCom").innerText = wash;
                document.getElementById("lblSoilCom").innerText = soil;

                specialModal.show();
            } else {
                alert("No special instructions found.");
            }
        } catch (err) {
            console.error("Error fetching special instructions:", err);
        }
    });

    // Exit Button
    btnExit.addEventListener("click", function () {
        window.location.href = "/";
    });
});