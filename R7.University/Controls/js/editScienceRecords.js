function ScienceRecordTypeUniqueValidator () {
    GridAndFormUniqueValidator.apply (this, ["ScienceRecordTypeId"]);
}

ScienceRecordTypeUniqueValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
ScienceRecordTypeUniqueValidator.prototype.constructor = ScienceRecordTypeUniqueValidator;
ScienceRecordTypeUniqueValidator.prototype.getSelectedFieldId = function (valContext) {
    return valContext.find ("[id $= 'comboScienceRecordType']").val ();
};
ScienceRecordTypeUniqueValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= 'hiddenScienceRecordTypeID']").val ();
};

scienceRecordTypeUniqueValidator = new ScienceRecordTypeUniqueValidator ();