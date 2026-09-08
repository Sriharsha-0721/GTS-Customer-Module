using Microsoft.AspNetCore.Mvc;
using GTSCustomerAPI.Web.Models.CustomerDeletion;

namespace GTSCustomerAPI.Web.Controllers
{
    public class CustomerDeletionController : Controller
    {
        // ============================================================
        // CUSTOMER
        // ============================================================

        [HttpGet]
        public IActionResult Index()
        {
            var model =
                new CustomerDeletionViewModel
                {
                    MarketCenter = 569,

                    SelectedCustId = null,

                    SelectedCustomerName =
                        string.Empty,

                    Customers =
                        new List<CustomerDeletionCustomerViewModel>
                        {
                            new CustomerDeletionCustomerViewModel
                            {
                                CustId = 1,

                                CustomerName =
                                    "ENDO USA B100 D-C",

                                CID =
                                    "792486027",

                                Status =
                                    "Open",

                                StartDate =
                                    new DateTime(
                                        2024,
                                        5,
                                        10
                                    ),

                                LastUpdated =
                                    DateTime.Now,

                                LastUpdatedBy =
                                    "Admin",

                                Stage =
                                    "Customer",

                                SystemRestrictions =
                                    false
                            },


                            new CustomerDeletionCustomerViewModel
                            {
                                CustId = 2,

                                CustomerName =
                                    "ABC MEDICAL",

                                CID =
                                    "792486028",

                                Status =
                                    "Open",

                                StartDate =
                                    new DateTime(
                                        2024,
                                        6,
                                        1
                                    ),

                                LastUpdated =
                                    DateTime.Now,

                                LastUpdatedBy =
                                    "Admin",

                                Stage =
                                    "Customer",

                                SystemRestrictions =
                                    false
                            }
                        }
                };


            return View(
                model
            );
        }


        // ============================================================
        // RECEIVERS
        // ============================================================

        [HttpGet]
        public IActionResult Receivers(
            int custId
        )
        {
            var receivers =
                new List<CustomerDeletionReceiverViewModel>();


            if (
                custId == 1
            )
            {
                receivers.Add(
                    new CustomerDeletionReceiverViewModel
                    {
                        Receiver =
                            "RCV10001",

                        CID =
                            "792486027",

                        ShipName =
                            "Main Plant",

                        RcvCreateDate =
                            new DateTime(
                                2026,
                                8,
                                20
                            ),

                        Status =
                            "Open"
                    }
                );


                receivers.Add(
                    new CustomerDeletionReceiverViewModel
                    {
                        Receiver =
                            "RCV10002",

                        CID =
                            "792486027",

                        ShipName =
                            "Warehouse",

                        RcvCreateDate =
                            new DateTime(
                                2026,
                                8,
                                22
                            ),

                        Status =
                            "Open"
                    }
                );
            }
            else if (
                custId == 2
            )
            {
                receivers.Add(
                    new CustomerDeletionReceiverViewModel
                    {
                        Receiver =
                            "RCV20001",

                        CID =
                            "792486028",

                        ShipName =
                            "ABC Main Facility",

                        RcvCreateDate =
                            new DateTime(
                                2026,
                                8,
                                25
                            ),

                        Status =
                            "Open"
                    }
                );


                receivers.Add(
                    new CustomerDeletionReceiverViewModel
                    {
                        Receiver =
                            "RCV20002",

                        CID =
                            "792486028",

                        ShipName =
                            "ABC Warehouse",

                        RcvCreateDate =
                            new DateTime(
                                2026,
                                8,
                                27
                            ),

                        Status =
                            "Open"
                    }
                );
            }


            return PartialView(
                "_Receivers",
                receivers
            );
        }


        // ============================================================
        // RECONCILIATION
        // ============================================================

        [HttpGet]
        public IActionResult Reconciliation(
            int custId
        )
        {
            var reconciliation =
                new List<CustomerDeletionReconciliationViewModel>();


            if (
                custId == 1
            )
            {
                reconciliation.Add(
                    new CustomerDeletionReconciliationViewModel
                    {
                        LineNumber = 1,

                        WearerNumber =
                            "W1001",

                        WearerName =
                            "John Smith",

                        Item =
                            "SHIRT001",

                        Size =
                            "L",

                        RasQty = 5,

                        GtsQty = 3,

                        PendingRts = 2
                    }
                );


                reconciliation.Add(
                    new CustomerDeletionReconciliationViewModel
                    {
                        LineNumber = 2,

                        WearerNumber =
                            "W1002",

                        WearerName =
                            "David Miller",

                        Item =
                            "PANT002",

                        Size =
                            "32",

                        RasQty = 4,

                        GtsQty = 3,

                        PendingRts = 1
                    }
                );
            }
            else if (
                custId == 2
            )
            {
                reconciliation.Add(
                    new CustomerDeletionReconciliationViewModel
                    {
                        LineNumber = 1,

                        WearerNumber =
                            "W2001",

                        WearerName =
                            "Robert Jones",

                        Item =
                            "SHIRT002",

                        Size =
                            "M",

                        RasQty = 6,

                        GtsQty = 4,

                        PendingRts = 2
                    }
                );


                reconciliation.Add(
                    new CustomerDeletionReconciliationViewModel
                    {
                        LineNumber = 2,

                        WearerNumber =
                            "W2002",

                        WearerName =
                            "Michael Brown",

                        Item =
                            "PANT003",

                        Size =
                            "34",

                        RasQty = 5,

                        GtsQty = 4,

                        PendingRts = 1
                    }
                );


                reconciliation.Add(
                    new CustomerDeletionReconciliationViewModel
                    {
                        LineNumber = 3,

                        WearerNumber =
                            "W2003",

                        WearerName =
                            "James Wilson",

                        Item =
                            "JACKET001",

                        Size =
                            "XL",

                        RasQty = 2,

                        GtsQty = 1,

                        PendingRts = 1
                    }
                );
            }


            return PartialView(
                "_Reconciliation",
                reconciliation
            );
        }


        // ============================================================
        // RTS RUIN
        // ============================================================

        [HttpGet]
        public IActionResult RTSRuin(
            int custId
        )
        {
            var rtsRuin =
                new List<CustomerDeletionRTSRuinViewModel>();


            if (
                custId == 1
            )
            {
                rtsRuin.Add(
                    new CustomerDeletionRTSRuinViewModel
                    {
                        ItemCode =
                            "SHIRT001",

                        Size =
                            "L",

                        TotalGarmentsGraded =
                            3,

                        Type =
                            "Torn",

                        Grade =
                            "Ruin"
                    }
                );


                rtsRuin.Add(
                    new CustomerDeletionRTSRuinViewModel
                    {
                        ItemCode =
                            "PANT002",

                        Size =
                            "32",

                        TotalGarmentsGraded =
                            2,

                        Type =
                            "Damaged",

                        Grade =
                            "Ruin"
                    }
                );
            }
            else if (
                custId == 2
            )
            {
                rtsRuin.Add(
                    new CustomerDeletionRTSRuinViewModel
                    {
                        ItemCode =
                            "SHIRT002",

                        Size =
                            "M",

                        TotalGarmentsGraded =
                            4,

                        Type =
                            "Torn",

                        Grade =
                            "Ruin"
                    }
                );


                rtsRuin.Add(
                    new CustomerDeletionRTSRuinViewModel
                    {
                        ItemCode =
                            "PANT003",

                        Size =
                            "34",

                        TotalGarmentsGraded =
                            2,

                        Type =
                            "Damaged",

                        Grade =
                            "Ruin"
                    }
                );


                rtsRuin.Add(
                    new CustomerDeletionRTSRuinViewModel
                    {
                        ItemCode =
                            "JACKET001",

                        Size =
                            "XL",

                        TotalGarmentsGraded =
                            1,

                        Type =
                            "Stained",

                        Grade =
                            "Ruin"
                    }
                );
            }


            return PartialView(
                "_RTSRuin",
                rtsRuin
            );
        }


        // ============================================================
        // LOST GARMENTS
        // ============================================================

        [HttpGet]
        public IActionResult LostGarments(
            int custId
        )
        {
            var lostGarments =
                new List<CustomerDeletionLostGarmentViewModel>();


            if (
                custId == 1
            )
            {
                lostGarments.Add(
                    new CustomerDeletionLostGarmentViewModel
                    {
                        Line = 1,

                        ItemCode =
                            "SHIRT001",

                        Size =
                            "L",

                        InService = 10,

                        Returned = 8,

                        Lost = 2
                    }
                );


                lostGarments.Add(
                    new CustomerDeletionLostGarmentViewModel
                    {
                        Line = 2,

                        ItemCode =
                            "PANT002",

                        Size =
                            "32",

                        InService = 8,

                        Returned = 7,

                        Lost = 1
                    }
                );
            }
            else if (
                custId == 2
            )
            {
                lostGarments.Add(
                    new CustomerDeletionLostGarmentViewModel
                    {
                        Line = 1,

                        ItemCode =
                            "SHIRT002",

                        Size =
                            "M",

                        InService = 12,

                        Returned = 10,

                        Lost = 2
                    }
                );


                lostGarments.Add(
                    new CustomerDeletionLostGarmentViewModel
                    {
                        Line = 2,

                        ItemCode =
                            "PANT003",

                        Size =
                            "34",

                        InService = 9,

                        Returned = 8,

                        Lost = 1
                    }
                );


                lostGarments.Add(
                    new CustomerDeletionLostGarmentViewModel
                    {
                        Line = 3,

                        ItemCode =
                            "JACKET001",

                        Size =
                            "XL",

                        InService = 5,

                        Returned = 4,

                        Lost = 1
                    }
                );
            }


            return PartialView(
                "_LostGarments",
                lostGarments
            );
        }


        // ============================================================
        // CUSTOMER SERVICE
        // ============================================================

        [HttpGet]
        public IActionResult CustomerService(
            int custId
        )
        {
            /*
             * No mock grid data needed here.
             *
             * Customer Service UI contains:
             *
             * - Confirmation checkbox
             * - Confirm button
             *
             * Stored procedure/business logic
             * will be connected later.
             */


            return PartialView(
                "_CustomerService"
            );
        }


        // ============================================================
        // FINAL REVIEW
        // ============================================================

        [HttpGet]
        public IActionResult FinalReview(
            int custId
        )
        {
            var finalReview =
                new List<CustomerDeletionFinalReviewViewModel>();


            // ========================================================
            // MOCK DATA ONLY
            // ========================================================

            if (
                custId == 1
            )
            {
                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "All Open Receivers are either Closed or Deleted."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "Customer returned garments have been RTS'd and Confirmed."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "All RTS Garments have been Graded and Confirmed."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "Lost Garments data extracted and reviewed."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "Customer Service checks completed."
                    }
                );
            }
            else
            {
                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "All Open Receivers are either Closed or Deleted."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "Customer returned garments have been RTS'd and Confirmed."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "All RTS Garments have been Graded and Confirmed."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            false,

                        Message =
                            "Customer deletion requirement is still pending."
                    }
                );


                finalReview.Add(
                    new CustomerDeletionFinalReviewViewModel
                    {
                        Approved =
                            true,

                        Message =
                            "Customer Service checks completed."
                    }
                );
            }


            return PartialView(
                "_FinalReview",
                finalReview
            );
        }
    }
}