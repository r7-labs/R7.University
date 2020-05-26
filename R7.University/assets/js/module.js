function r7_University_selectItem ($, target) {
	$(".u8y-selected-item").removeClass ("u8y-selected-item");
	$(target).closest ("tr").addClass ("u8y-selected-item");
}

function r7_University_restoreSelectedItem ($, window, moduleId, selectedItemId) {
	var elem = $(".DnnModule-" + moduleId + " #" + selectedItemId);
    if (elem.length > 0) {
		var offset = elem.offset ();
		window.scrollTo (offset.left, offset.top - window.innerHeight/4);
		$(elem).addClass ("u8y-selected-item");
	}
}
