// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function ($) {
    'use strict';

    var $accountDelete = $('#delete-account'),
        $accountDeleteDialog = $('#confirm-delete'),
        transition;

    $accountDelete.on('click', function () {
        $accountDeleteDialog[0].showModal();
        transition = window.setTimeout(function () {
            $accountDeleteDialog.addClass('dialog-scale');
        }, 0.5);
    });

    $('#cancel').on('click', function () {
        $accountDeleteDialog[0].close();
        $accountDeleteDialog.removeClass('dialog-scale');
        clearTimeout(transition);
    });

})(jQuery);
