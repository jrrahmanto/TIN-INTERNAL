// Get the instance of PageRequestManager.

//var tbl;
//function load(id) {
//    var prm = Sys.WebForms.PageRequestManager.getInstance();
//    // Add initializeRequest and endRequest
//    prm.add_initializeRequest(prm_InitializeRequest);
//    prm.add_endRequest(prm_EndRequest);

//    tbl = id;
//    var s = document.getElementById(tbl.id);
//    s.style.visibility = "hidden";

//    //tbl.onload = 'tbl.visible = false;';
//    
//}

//// Called when async postback begins
//function prm_InitializeRequest(sender, args) {
//    // get the divImage and set it to visible
////    var panelProg = $get('divImage');
//    //    panelProg.style.display = '';
//    //var tbl = $get('tblImage');

//    //tbl.style.display = '';
//    var s = document.getElementById(tbl.id);
//    s.style.visibility = "visible"
//    // Disable button that caused a postback
//    $get(args._postBackElement.id).disabled = true;
//}

//// Called when async postback ends
//function prm_EndRequest(sender, args) {
//    // get the divImage and hide it again
////    var panelProg = $get('divImage');
//    //    panelProg.style.display = 'none';
//   // var tbl = $get('tblImage');
//    //tbl.style.display = 'none';
//    //tbl.disable = false;
//    var s = document.getElementById(tbl.id);
//    s.style.visibility = "hidden";
//    // Enable button that caused a postback
//    $get(sender._postBackSettings.sourceElement.id).disabled = false;
//}