function validateTimeToLearn (sender, e) {
    var years = jQuery ("[id $= 'textTimeToLearnYears']").val ();
    var months = jQuery ("[id $= 'textTimeToLearnMonths']").val ();
    var hours = jQuery ("[id $= 'textTimeToLearnHours']").val ();
    if (years == 0 && months == 0 && hours == 0) {
            e.IsValid = false;
    }
}

function validateEduForm (sender, e) {
    var items = JSON.parse (jQuery ("[id $= 'gridEduForms']").attr ("data-items"));
    if (items.length > 0) {
        var selectedEduFormId = jQuery ("input[id *= '_radioEduForm_']:checked").val ();
        console.log ("selectedEduFormId=" + selectedEduFormId);
        var addCmd = jQuery ("[id $= 'buttonAddEduForm']").length === 1;
        var count = items.filter (function (i) {return i.EduFormID == selectedEduFormId; }).length;
        if (addCmd && count === 0) { return; }
        var editedEduFormId = jQuery ("[id $= 'hiddenEduFormID']").val ();
        if (!addCmd && count === ((selectedEduFormId === editedEduFormId)? 1 : 0)) { return; }
    }
    e.IsValid = false;
}