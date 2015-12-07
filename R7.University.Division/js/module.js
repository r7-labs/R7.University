$(function () {
    // init division barcode dialog
    $(".dialog-division-barcode").dialog({
        dialogClass:"dnnFormPopup",
        resizable: false,
        modal: true,
        autoOpen: false
    });
});

function showDivisionBarcodeDialog(element) {
    var dialogSelector = "#dialog-division-barcode-" + element.getAttribute("data-module-id");
    $(dialogSelector)
        .dialog("option", "title", element.getAttribute("data-dialog-title"))
        .dialog("open");
}