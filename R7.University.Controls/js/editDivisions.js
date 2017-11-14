function DivisionUniqueValidator () {
    GridAndFormUniqueValidator.apply (this, ["DivisionId"]);
}

DivisionUniqueValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
DivisionUniqueValidator.prototype.constructor = DivisionUniqueValidator;
DivisionUniqueValidator.prototype.getSelectedFieldId = function (valContext) {
    var selectedDivisionId = valContext.find ("[id $= 'divisionSelector_comboDivision']").val ();
    if (!selectedDivisionId) {
        var treeClientId = valContext.find ("[id $= 'divisionSelector_treeDivision']").attr ("id");
        selectedDivisionId = $find (treeClientId).get_selectedNodes ()[0].get_value ();
    }

    return selectedDivisionId;
};
DivisionUniqueValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= 'hiddenDivisionID']").val ();
};

divisionUniqueValidator = new DivisionUniqueValidator ();