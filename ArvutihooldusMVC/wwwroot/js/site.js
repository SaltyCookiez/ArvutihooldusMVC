// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$.get(`${window.location.origin}/orders/GetOrderPrice`, { computerType: "Laptop", service: "Cleaning" }, function (data) {
    $('#Price').val(data);
});

$('#ComputerType, #Service').change(function () {
    var computerType = $('#ComputerType').val();
    var service = $('#Service').val();

    $.get(`${window.location.origin}/orders/GetOrderPrice`, { computerType: computerType, service: service }, function (data) {
        $('#Price').val(data);
    });
});