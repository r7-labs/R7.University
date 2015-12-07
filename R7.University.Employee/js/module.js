$(function () {
    // init description dialog
    $(".dialog-employee-achievement-description").dialog({
        dialogClass:"dnnFormPopup",
        modal: true,
        autoOpen: false,
        resizable: false
    });
});

function showEmployeeAchievementDescriptionDialog(element) {
    var dialogSelector = "#dialog-employee-achievement-description-" + element.getAttribute("data-module-id");
    $(dialogSelector).html("<p>" + element.getAttribute("data-description") + "</p>");
    $(dialogSelector)
        .dialog("option", "title", element.getAttribute("data-dialog-title"))
        .dialog("open");
}