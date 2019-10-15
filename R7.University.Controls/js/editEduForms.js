function validateTimeToLearn (sender, e) {
    var years = jQuery ("[id $= '_textTimeToLearnYears']").val ();
    var months = jQuery ("[id $= '_textTimeToLearnMonths']").val ();
    var hours = jQuery ("[id $= '_textTimeToLearnHours']").val ();
    if (years == 0 && months == 0 && hours == 0) {
        e.IsValid = false;
    }
}

function EduFormUniqueValidator () {
    GridAndFormUniqueValidator.apply (this, ["EduFormID"]);
}

EduFormUniqueValidator.prototype = Object.create (GridAndFormUniqueValidator.prototype);
EduFormUniqueValidator.prototype.constructor = EduFormUniqueValidator;
EduFormUniqueValidator.prototype.getSelectedFieldId = function (valContext) {
    return valContext.find ("input[id *= '_radioEduForm_']:checked").val ();
};

EduFormUniqueValidator.prototype.getEditedFieldId = function (valContext) {
    return valContext.find ("[id $= '_hiddenEduFormID']").val ();
};

eduFormUniqueValidator = new EduFormUniqueValidator ();