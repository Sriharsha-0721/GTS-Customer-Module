$(document).ready(function () {

    // ============================================================
    // CUSTOMER DELETION - SHARED STATE
    // ============================================================

    let selectedCustId = null;
    let selectedCustomerName = "";
    let selectedCID = "";


    // ============================================================
    // INITIALIZATION
    // ============================================================

    initializeCustomerDeletion();


    function initializeCustomerDeletion() {

        console.log(
            "Customer Deletion initialized."
        );

        setStage(
            "customers"
        );
    }


    // ============================================================
    // CUSTOMER SEARCH
    // ============================================================

    $(document).on(
        "input",
        "#txtCustomerSearch",
        function () {

            const searchText =
                String($(this).val() || "")
                    .toLowerCase()
                    .trim();


            $("#tblCustomerDeletion tbody tr")
                .each(function () {

                    const row =
                        $(this);


                    if (
                        row.find(".customer-select").length === 0
                    ) {
                        return;
                    }


                    const rowText =
                        String(
                            row.text() || ""
                        )
                            .toLowerCase()
                            .trim();


                    row.toggle(
                        rowText.includes(searchText)
                    );
                });
        }
    );


    // ============================================================
    // CUSTOMER RADIO SELECTION
    // ============================================================

    $(document).on(
        "change",
        ".customer-select",
        function () {

            selectCustomer(
                $(this)
            );
        }
    );


    // ============================================================
    // CUSTOMER ROW CLICK
    // ============================================================

    $(document).on(
        "click",
        "#tblCustomerDeletion tbody tr",
        function (event) {

            if (
                $(event.target)
                    .is(
                        "input, button, a, label"
                    )
            ) {
                return;
            }


            const radio =
                $(this)
                    .find(
                        ".customer-select"
                    );


            if (
                radio.length === 0
            ) {
                return;
            }


            radio.prop(
                "checked",
                true
            );


            radio.trigger(
                "change"
            );
        }
    );


    // ============================================================
    // SELECT CUSTOMER
    // ============================================================

    function selectCustomer(radio) {

        if (
            !radio ||
            radio.length === 0
        ) {
            return;
        }


        selectedCustId =
            radio.val();


        selectedCustomerName =
            String(
                radio.attr("data-name") || ""
            );


        selectedCID =
            String(
                radio.attr("data-cid") || ""
            );


        $("#tblCustomerDeletion tbody tr")
            .removeClass(
                "customer-selected-row"
            );


        radio
            .closest("tr")
            .addClass(
                "customer-selected-row"
            );


        $("#selectedCustomerName")
            .text(
                selectedCustomerName
            );


        $("#btnCustomerDeletionNext")
            .prop(
                "disabled",
                false
            );


        console.log(
            "Customer selected:",
            {
                custId:
                    selectedCustId,

                customerName:
                    selectedCustomerName,

                cid:
                    selectedCID
            }
        );
    }


    // ============================================================
    // VALIDATE CUSTOMER
    // ============================================================

    function validateCustomer() {

        if (
            selectedCustId === null ||
            selectedCustId === undefined ||
            selectedCustId === ""
        ) {

            alert(
                "Please select a customer first."
            );

            return false;
        }


        return true;
    }


    // ============================================================
    // CUSTOMERS -> RECEIVERS
    // ============================================================

    $(document).on(
        "click",
        "#btnCustomerDeletionNext",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            loadReceivers();
        }
    );


    // ============================================================
    // LOAD RECEIVERS
    // ============================================================

    function loadReceivers() {

        if (
            !validateCustomer()
        ) {
            return;
        }


        showLoading(
            "Loading Receivers..."
        );


        $.ajax({

            url:
                "/CustomerDeletion/Receivers",

            type:
                "GET",

            data: {

                custId:
                    selectedCustId
            },


            success:
                function (html) {

                    $("#customerDeletionContent")
                        .html(
                            html
                        );


                    setStage(
                        "receivers"
                    );


                    refreshSelectedCustomerName();


                    console.log(
                        "Receiver stage loaded."
                    );
                },


            error:
                function (xhr) {

                    handleAjaxError(
                        "Unable to load Receivers.",
                        xhr
                    );
                }
        });
    }


    // ============================================================
    // RECEIVERS -> CUSTOMERS
    // ============================================================

    $(document).on(
        "click",
        "#btnReceiverPrevious",
        function () {

            window.location.href =
                "/CustomerDeletion";
        }
    );


    // ============================================================
    // CLOSE RECEIVERS
    // ============================================================

    $(document).on(
        "click",
        "#btnCloseReceivers",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            const confirmed =
                confirm(
                    "Are you sure you want to close the Receivers?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Close Receivers functionality will be connected after the stored procedure is received."
            );
        }
    );


    // ============================================================
    // DELETE RECEIVERS
    // ============================================================

    $(document).on(
        "click",
        "#btnDeleteReceivers",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            const confirmed =
                confirm(
                    "Are you sure you want to delete the Receivers?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Delete Receivers functionality will be connected after the stored procedure is received."
            );
        }
    );


    // ============================================================
    // RECEIVERS -> RECONCILIATION
    // ============================================================

    $(document).on(
        "click",
        "#btnReceiverNext",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            loadReconciliation();
        }
    );


    // ============================================================
    // LOAD RECONCILIATION
    // ============================================================

    function loadReconciliation() {

        if (
            !validateCustomer()
        ) {
            return;
        }


        showLoading(
            "Loading Reconciliation..."
        );


        $.ajax({

            url:
                "/CustomerDeletion/Reconciliation",

            type:
                "GET",

            data: {

                custId:
                    selectedCustId
            },


            success:
                function (html) {

                    $("#customerDeletionContent")
                        .html(
                            html
                        );


                    setStage(
                        "reconciliation"
                    );


                    refreshSelectedCustomerName();


                    console.log(
                        "Reconciliation stage loaded."
                    );
                },


            error:
                function (xhr) {

                    handleAjaxError(
                        "Unable to load Reconciliation.",
                        xhr
                    );
                }
        });
    }


    // ============================================================
    // RECONCILIATION -> RECEIVERS
    // ============================================================

    $(document).on(
        "click",
        "#btnReconciliationPrevious",
        function () {

            loadReceivers();
        }
    );


    // ============================================================
    // CONFIRM RTS COMPLETE
    // ============================================================

    $(document).on(
        "click",
        "#btnConfirmRtsComplete",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            const confirmed =
                confirm(
                    "Are you sure RTS reconciliation is complete?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Confirm RTS Complete functionality will be connected after the stored procedure is received."
            );
        }
    );


    // ============================================================
    // RECONCILIATION -> RTS RUIN
    // ============================================================

    $(document).on(
        "click",
        "#btnReconciliationNext",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            loadRTSRuin();
        }
    );


    // ============================================================
    // LOAD RTS RUIN
    // ============================================================

    function loadRTSRuin() {

        if (
            !validateCustomer()
        ) {
            return;
        }


        showLoading(
            "Loading RTS Ruin..."
        );


        $.ajax({

            url:
                "/CustomerDeletion/RTSRuin",

            type:
                "GET",

            data: {

                custId:
                    selectedCustId
            },


            success:
                function (html) {

                    $("#customerDeletionContent")
                        .html(
                            html
                        );


                    setStage(
                        "rtsruin"
                    );


                    refreshSelectedCustomerName();


                    console.log(
                        "RTS Ruin stage loaded."
                    );
                },


            error:
                function (xhr) {

                    handleAjaxError(
                        "Unable to load RTS Ruin details.",
                        xhr
                    );
                }
        });
    }


    // ============================================================
    // RTS RUIN -> RECONCILIATION
    // ============================================================

    $(document).on(
        "click",
        "#btnRTSRuinPrevious",
        function () {

            loadReconciliation();
        }
    );


    // ============================================================
    // CONFIRM GRADING COMPLETED
    // ============================================================

    $(document).on(
        "click",
        "#btnConfirmGradingCompleted",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            const confirmed =
                confirm(
                    "Are you sure grading has been completed?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Confirm Grading Completed functionality will be connected after the stored procedure is received."
            );
        }
    );


    // ============================================================
    // RTS RUIN -> LOST GARMENTS
    // ============================================================

    $(document).on(
        "click",
        "#btnRTSRuinNext",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            loadLostGarments();
        }
    );


    // ============================================================
    // LOAD LOST GARMENTS
    // ============================================================

    function loadLostGarments() {

        if (
            !validateCustomer()
        ) {
            return;
        }


        showLoading(
            "Loading Lost Garments..."
        );


        $.ajax({

            url:
                "/CustomerDeletion/LostGarments",

            type:
                "GET",

            data: {

                custId:
                    selectedCustId
            },


            success:
                function (html) {

                    $("#customerDeletionContent")
                        .html(
                            html
                        );


                    setStage(
                        "lostgarments"
                    );


                    refreshSelectedCustomerName();


                    console.log(
                        "Lost Garments stage loaded."
                    );
                },


            error:
                function (xhr) {

                    handleAjaxError(
                        "Unable to load Lost Garment details.",
                        xhr
                    );
                }
        });
    }


    // ============================================================
    // LOST GARMENTS -> RTS RUIN
    // ============================================================

    $(document).on(
        "click",
        "#btnLostGarmentsPrevious",
        function () {

            loadRTSRuin();
        }
    );


    // ============================================================
    // GENERATE LOST INVENTORY REPORT
    // ============================================================

    $(document).on(
        "click",
        "#btnGenerateLostInventoryReport",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            alert(
                "Generate Lost Inventory Report functionality will be connected with the backend report functionality."
            );
        }
    );


    // ============================================================
    // DELETE REPORTED LOST GARMENTS CHECKBOX
    // ============================================================

    $(document).on(
        "change",
        "#chkDeleteReportedLostGarments",
        function () {

            const checked =
                $(this)
                    .is(":checked");


            console.log(
                "Delete Reported Lost Garments:",
                checked
            );
        }
    );


    // ============================================================
    // BULK RTS
    // ============================================================

    $(document).on(
        "click",
        "#btnLostBulkRTS",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            const confirmed =
                confirm(
                    "Are you sure you want to Bulk RTS the pending garments?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Bulk RTS Pending Garments functionality will be connected after the stored procedure is received."
            );
        }
    );


    // ============================================================
    // CONFIRM LOST INVENTORY
    // ============================================================

    $(document).on(
        "click",
        "#btnConfirmLostInventory",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            const confirmed =
                confirm(
                    "Are you sure the Lost Inventory has been reconciled?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Confirm Lost Inventory Reconciled functionality will be connected after the stored procedure is received."
            );
        }
    );


    // ============================================================
    // LOST GARMENTS -> CUSTOMER SERVICE
    // ============================================================

    $(document).on(
        "click",
        "#btnLostGarmentsNext",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            loadCustomerService();
        }
    );


    // ============================================================
    // LOAD CUSTOMER SERVICE
    // ============================================================

    function loadCustomerService() {

        if (
            !validateCustomer()
        ) {
            return;
        }


        showLoading(
            "Loading Customer Service..."
        );


        $.ajax({

            url:
                "/CustomerDeletion/CustomerService",

            type:
                "GET",

            data: {

                custId:
                    selectedCustId
            },


            success:
                function (html) {

                    $("#customerDeletionContent")
                        .html(
                            html
                        );


                    setStage(
                        "customerservice"
                    );


                    refreshSelectedCustomerName();


                    console.log(
                        "Customer Service stage loaded."
                    );
                },


            error:
                function (xhr) {

                    handleAjaxError(
                        "Unable to load Customer Service.",
                        xhr
                    );
                }
        });
    }


    // ============================================================
    // CUSTOMER SERVICE -> LOST GARMENTS
    // ============================================================

    $(document).on(
        "click",
        "#btnCustomerServicePrevious",
        function () {

            loadLostGarments();
        }
    );


    // ============================================================
    // CUSTOMER SERVICE CHECKBOX
    // ============================================================

    $(document).on(
        "change",
        "#chkCustomerService",
        function () {

            const checked =
                $(this)
                    .is(":checked");


            $("#btnCustomerServiceConfirm")
                .prop(
                    "disabled",
                    !checked
                );
        }
    );


    // ============================================================
    // CONFIRM CUSTOMER SERVICE
    // ============================================================

    $(document).on(
        "click",
        "#btnCustomerServiceConfirm",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            if (
                !$("#chkCustomerService")
                    .is(":checked")
            ) {

                alert(
                    "Please confirm the Customer Service check."
                );

                return;
            }


            const confirmed =
                confirm(
                    "Confirm Customer Service checks?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            $("#customerServiceMessage")
                .show();


            alert(
                "Customer Service checks confirmed successfully."
            );
        }
    );


    // ============================================================
    // CUSTOMER SERVICE -> FINAL REVIEW
    // ============================================================

    $(document).on(
        "click",
        "#btnCustomerServiceNext",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            loadFinalReview();
        }
    );


    // ============================================================
    // LOAD FINAL REVIEW
    // ============================================================

    function loadFinalReview() {

        if (
            !validateCustomer()
        ) {
            return;
        }


        showLoading(
            "Loading Final Review..."
        );


        $.ajax({

            url:
                "/CustomerDeletion/FinalReview",

            type:
                "GET",

            data: {

                custId:
                    selectedCustId
            },


            success:
                function (html) {

                    $("#customerDeletionContent")
                        .html(
                            html
                        );


                    setStage(
                        "finalreview"
                    );


                    refreshSelectedCustomerName();


                    console.log(
                        "Final Review stage loaded."
                    );
                },


            error:
                function (xhr) {

                    handleAjaxError(
                        "Unable to load Final Review.",
                        xhr
                    );
                }
        });
    }


    // ============================================================
    // FINAL REVIEW -> CUSTOMER SERVICE
    // ============================================================

    $(document).on(
        "click",
        "#btnFinalReviewPrevious",
        function () {

            loadCustomerService();
        }
    );


    // ============================================================
    // FINAL REVIEW DELETE CHECKBOX
    // ============================================================

    $(document).on(
        "change",
        "#chkDeleteCustomer",
        function () {

            const checked =
                $(this)
                    .is(":checked");


            $("#btnFinalSubmit")
                .prop(
                    "disabled",
                    !checked
                );
        }
    );


    // ============================================================
    // FINAL SUBMIT
    // ============================================================

    $(document).on(
        "click",
        "#btnFinalSubmit",
        function () {

            if (
                !validateCustomer()
            ) {
                return;
            }


            if (
                !$("#chkDeleteCustomer")
                    .is(":checked")
            ) {

                alert(
                    "Please confirm customer deletion."
                );

                return;
            }


            const confirmed =
                confirm(
                    "Are you sure you want to submit this customer for deletion?"
                );


            if (
                !confirmed
            ) {
                return;
            }


            alert(
                "Customer Deletion submission will be connected after the stored procedures are available."
            );
        }
    );


    // ============================================================
    // TOP WIZARD NAVIGATION
    // ============================================================

    $(document).on(
        "click",
        ".cd-step, .deletion-step",
        function (event) {

            event.preventDefault();


            const stage =
                String(
                    $(this).attr("data-stage") || ""
                )
                    .toLowerCase()
                    .trim();


            if (
                stage === "customers"
            ) {

                window.location.href =
                    "/CustomerDeletion";

                return;
            }


            if (
                !validateCustomer()
            ) {
                return;
            }


            switch (
            stage
            ) {

                case "receivers":

                    loadReceivers();

                    break;


                case "reconciliation":

                    loadReconciliation();

                    break;


                case "rtsruin":

                    loadRTSRuin();

                    break;


                case "lostgarments":

                    loadLostGarments();

                    break;


                case "customerservice":

                    loadCustomerService();

                    break;


                case "finalreview":

                    loadFinalReview();

                    break;


                default:

                    console.warn(
                        "Unknown Customer Deletion stage:",
                        stage
                    );

                    break;
            }
        }
    );


    // ============================================================
    // SET ACTIVE STAGE
    // ============================================================

    function setStage(stage) {

        $(".cd-step, .deletion-step")
            .removeClass(
                "active current-step"
            );


        $(
            '.cd-step[data-stage="'
            +
            stage
            +
            '"], '
            +
            '.deletion-step[data-stage="'
            +
            stage
            +
            '"]'
        )
            .addClass(
                "active"
            );
    }


    // ============================================================
    // KEEP SELECTED CUSTOMER NAME
    // ============================================================

    function refreshSelectedCustomerName() {

        if (
            selectedCustomerName === null ||
            selectedCustomerName === undefined ||
            selectedCustomerName === ""
        ) {
            return;
        }


        $("#selectedCustomerName")
            .text(
                selectedCustomerName
            );
    }


    // ============================================================
    // LOADING
    // ============================================================

    function showLoading(message) {

        const safeMessage =
            escapeHtml(
                message
            );


        $("#customerDeletionContent")
            .html(
                '<div class="text-center py-5">'
                +
                '<div class="spinner-border text-primary" role="status">'
                +
                '<span class="visually-hidden">Loading...</span>'
                +
                '</div>'
                +
                '<div class="mt-2">'
                +
                safeMessage
                +
                '</div>'
                +
                '</div>'
            );
    }


    // ============================================================
    // AJAX ERROR HANDLER
    // ============================================================

    function handleAjaxError(
        userMessage,
        xhr
    ) {

        console.error(
            userMessage
        );


        if (
            xhr
        ) {

            console.error(
                "HTTP Status:",
                xhr.status
            );


            console.error(
                "Response:",
                xhr.responseText
            );
        }


        alert(
            userMessage
        );
    }


    // ============================================================
    // ESCAPE HTML
    // ============================================================

    function escapeHtml(value) {

        if (
            value === null ||
            value === undefined
        ) {
            return "";
        }


        return $("<div>")
            .text(
                value
            )
            .html();
    }

});