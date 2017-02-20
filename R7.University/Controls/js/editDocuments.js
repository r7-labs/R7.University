function validateDocumentUrl (sender, e) {
    e.IsValid = true;
    var filesComboBox = jQuery ("[id $= 'urlDocumentUrl_ctlFile_FilesComboBox_state']").first ();
    if (filesComboBox.length === 1) {
        var filesComboBoxState = JSON.parse (filesComboBox.val ());
        // TODO: Add separate validation for document URL is required
        if (filesComboBoxState.selectedItem.key != -1) {
            var selectedFileName = filesComboBoxState.selectedItem.value;
            var comboDocumentType = jQuery ("[id $= 'comboDocumentType']").first ();
            var selectedTypeId = comboDocumentType.val ();
            var regexesAttr = comboDocumentType.attr ("data-validation");
            if (!!regexesAttr) {
                var regexes = jQuery.grep (JSON.parse (regexesAttr), function (elem) {
                    return elem.id == selectedTypeId;
                });
                if (regexes.length > 0) {
                    var regex = new RegExp ("^" + regexes [0].match + "$");
                    if (!regex.test (selectedFileName)) {
                        e.IsValid = false;
                        var valDocumentUrl = jQuery ("[id $= 'valDocumentUrl']").first ();
                        valDocumentUrl.text (valDocumentUrl.attr ("data-message-template").replace ("{regex}", regexes [0].match));
                    }
                }
            }
        }
    }
}