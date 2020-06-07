class EditAchievements {
    setAchievementPanelsVisibility (comboAchievement, pnlAchievementTitle, txtAchievementTitle, pnlAchievementShortTitle, pnlAchievementTypes) {
        const selectedValue = comboAchievement.options[comboAchievement.selectedIndex].value;
        if (selectedValue == -1) {
            jQuery (pnlAchievementTitle).show ();
            jQuery (pnlAchievementShortTitle).show ();
            jQuery (pnlAchievementTypes).show();
        }
        else {
            jQuery (pnlAchievementTitle).hide ();
            jQuery (pnlAchievementShortTitle).hide ();
            jQuery (pnlAchievementTypes).hide();
        }
        
        // avoid validation error on hidden textbox
        if (selectedValue == -1) {
        	if (jQuery(txtAchievementTitle).text () == "{{empty}}") {
        		jQuery(txtAchievementTitle).text ("");
        	}
        }
        else {
        	jQuery(txtAchievementTitle).text ("{{empty}}");
        }
    }
}

u8y_editAchievements = new EditAchievements ();
