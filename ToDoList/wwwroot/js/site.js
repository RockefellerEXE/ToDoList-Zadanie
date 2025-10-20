// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// po zmianie daty automatycznie submituje formularz
document.getElementById('date').addEventListener('change', function() {
    document.getElementById('dateForm').submit();
});
