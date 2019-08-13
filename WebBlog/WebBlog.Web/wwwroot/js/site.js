// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function clickCheck(id, el) {
    var siblings = document.getElementById("checkInput");


    if (siblings != el)
        siblings.oldChecked = false;
    if (el.oldChecked)
        el.checked = false;
    el.oldChecked = el.checked;
    siblings.value = el.checked;
    swapState(id, !el.checked);
}
function swapState(id, isDisabled) {
    document.getElementById(id).disabled = isDisabled;
}

var ul = $('.dropdown-search-box');
var input = ul.find('input');
var li = ul.find('li.result');

input.keyup(function () {
    var val = $(this).val();

    if (val.length > 1) {
        li.hide();
        li.filter(':contains("' + val + '")').show();
    } else {
        li.show();
    }
});
