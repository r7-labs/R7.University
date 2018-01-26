function ed_shorten (n, text) {
    return (text.length > n) ? text.substring (0, n-1).trim () + "\u2026" : text;
}

function ed_treeNodeClicked (sender, eventArgs) {
    var nodeText = eventArgs.get_node ().get_text ();
    $(".employee-directory #linkDivisions").text (ed_shorten (35, nodeText));
    $(".employee-directory #linkDivisions").attr ("title", nodeText);
    $(".employee-directory #hiddenDivisions").hide ();
}

function ed_treeLoad (sender, eventArgs) {
    var nodes = sender.get_selectedNodes ();
    if (nodes.length > 0) {
        var nodeText = nodes [0].get_text ();
         $(".employee-directory #linkDivisions").text (ed_shorten (35, nodeText));
         $(".employee-directory #linkDivisions").attr ("title", nodeText);
    }
}