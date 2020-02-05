function EduProgramProfileUniqueValidator () {
    GridAndFormUniqueValidator.apply (this, ["EduProgramProfileID"]);
}

EduProgramProfileUniqueValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
EduProgramProfileUniqueValidator.prototype.constructor = EduProgramProfileUniqueValidator;
EduProgramProfileUniqueValidator.prototype.getSelectedFieldId = function (valContext) {
    return valContext.find ("[id $= '_ddlEduProfile']").val ();
};
EduProgramProfileUniqueValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= '_hiddenEduProgramProfileID']").val ();
};

eduProgramProfileUniqueValidator = new EduProgramProfileUniqueValidator ();