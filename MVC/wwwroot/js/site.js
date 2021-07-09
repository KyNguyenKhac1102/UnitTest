// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var deleteModal = document.getElementById("DeleteModal");
var closeSymbol = document.getElementById("Close");
var closeButton = document.getElementById("CloseButton");

function OnClickClose() {
    deleteModal.hidden = true;
}

function OnClickDelete() {
    deleteModal.hidden = false;
}
