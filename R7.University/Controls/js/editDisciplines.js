function validateDiscipline (sender, e) {
    var itemsData = jQuery ("[id $= 'gridDisciplines']").attr ("data-items");
    if (!!itemsData) {
        var items = JSON.parse (itemsData);
        if (items.length > 0) {
            var selectedEduProgramProfileId = jQuery ("[id $= 'comboEduProgramProfile']").val ();
            var addCmd = jQuery ("[id $= 'buttonAddDiscipline']").length === 1;
            var count = items.filter (function (i) {return i.EduProgramProfileID == selectedEduProgramProfileId && i.EditState != "Deleted"; }).length;
            if (addCmd && count === 0) { return; }
            var editedEduProgramProfileId = jQuery ("[id $= 'hiddenEduProgramProfileID']").val ();
            if (!addCmd && count === ((selectedEduProgramProfileId === editedEduProgramProfileId)? 1 : 0)) { return; }
            e.IsValid = false;
        }
    }
}