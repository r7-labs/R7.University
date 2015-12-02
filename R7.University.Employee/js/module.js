$(function () {
    // init description dialog
    $("#dialog-description").dialog({
        dialogClass:"dnnFormPopup",
        autoOpen: false
    });
});

function showDescription(element) {
    $("#dialog-description").html("<p>" + element.getAttribute("data-description") + "</p>");
    $("#dialog-description")
        .dialog("option", "title", element.innerHTML)
        .dialog("open");
}