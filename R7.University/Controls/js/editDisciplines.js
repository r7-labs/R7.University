function EduProgramProfileUniqueValidator () {
    GridAndFormUniqueValidator.apply (this, ["EduProgramProfileID"]);
}

EduProgramProfileUniqueValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
EduProgramProfileUniqueValidator.prototype.constructor = EduProgramProfileUniqueValidator;
EduProgramProfileUniqueValidator.prototype.getSelectedFieldId = function (valContext) {
    return valContext.find ("[id $= 'comboEduProgramProfile']").val ();
};
EduProgramProfileUniqueValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= 'hiddenEduProgramProfileID']").val ();
};

eduProgramProfileUniqueValidator = new EduProgramProfileUniqueValidator ();