// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    $("#adminMenuButton").on("click", function (e) {

        e.preventDefault();
        e.stopPropagation();

        $(".gts-admin-menu").toggleClass("open");
    });


    $(document).on("click", function () {

        $(".gts-admin-menu").removeClass("open");

    });


    $(".gts-admin-dropdown").on("click", function (e) {

        e.stopPropagation();

    });

});