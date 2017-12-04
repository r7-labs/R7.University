function validateTimeToLearn (sender, e) {
    var years = jQuery ("[id $= 'textTimeToLearnYears']").val ();
    var months = jQuery ("[id $= 'textTimeToLearnMonths']").val ();
    var hours = jQuery ("[id $= 'textTimeToLearnHours']").val ();
    if (years == 0 && months == 0 && hours == 0) {
        e.IsValid = false;
    }
}