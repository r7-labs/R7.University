function EduProfileValidator () {
    GridAndFormUniqueValidator.apply (this, ["EduProgramProfileID"]);
}

EduProfileValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
EduProfileValidator.prototype.constructor = EduProfileValidator;
EduProfileValidator.prototype.getSelectedFieldId = function (valContext) {
    return valContext.find ("[id $= '_ddlEduProfile']").val ();
};
EduProfileValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= '_hiddenEduProgramProfileID']").val ();
};
EduProfileValidator.prototype.validate2 = function (sender, e) {
	var valContext = jQuery (sender).closest (".dnnForm");
    if (valContext.length === 0) {
        e.IsValid = false;
        throw "Could not find validation context for " + sender;
    }
	var selectedFieldId = this.getSelectedFieldId (valContext);
	if (selectedFieldId < 0) {
		e.IsValid = false;
		return;
	}
	return this.validate (sender, e);
};

eduProfileValidator = new EduProfileValidator ();