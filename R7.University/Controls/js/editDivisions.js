function validateDivision (sender, e) {
    var items = JSON.parse (jQuery ("[id $= 'gridDivisions']").attr ("data-items"));
    if (items.length > 0) {
        var selectedDivisionId = jQuery ("[id $= 'divisionSelector_comboDivision']").val ();
        var addCmd = jQuery ("[id $= 'buttonAddDivision']").length === 1;
        var count = items.filter (function (i) {return i.DivisionId == selectedDivisionId; }).length;
        if (addCmd && count === 0) { return; }
        var editedDivisionId = jQuery ("[id $= 'hiddenDivisionID']").val ();
        if (!addCmd && count === ((selectedDivisionId === editedDivisionId)? 1 : 0)) { return; }
    }
    e.IsValid = false;
}