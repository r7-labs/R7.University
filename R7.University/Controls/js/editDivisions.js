function validateDivision (sender, e) {
    var itemsData = jQuery ("[id $= 'gridDivisions']").attr ("data-items");
    if (!!itemsData) {
        var items = JSON.parse (itemsData);
        if (items.length > 0) {
            var selectedDivisionId = jQuery ("[id $= 'divisionSelector_comboDivision']").val ();
            if (!selectedDivisionId) {
                var treeClientId = jQuery ("[id $= 'divisionSelector_treeDivision']").attr ("id");
                selectedDivisionId = $find (treeClientId).get_selectedNodes ()[0].get_value ();
            }
            var addCmd = jQuery ("[id $= 'buttonAddDivision']").length === 1;
            var count = items.filter (function (i) {return i.DivisionId == selectedDivisionId && i.EditState != "Deleted"; }).length;
            if (addCmd && count === 0) { return; }
            var editedDivisionId = jQuery ("[id $= 'hiddenDivisionID']").val ();
            if (!addCmd && count === ((selectedDivisionId === editedDivisionId)? 1 : 0)) { return; }
            e.IsValid = false;
        }
    }
}