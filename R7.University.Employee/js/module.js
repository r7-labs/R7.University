$(function () {
    // paint tables with dnnGrid classes
    var table = ".employeeDetails #employeeDisciplines table";
    $(table).addClass ("dnnGrid").css ("border-collapse", "collapse");
    // use parent () to get rows with matched cell types
    $(table + " tr:nth-child(even) td").parent ().addClass ("dnnGridItem");
    $(table + " tr:nth-child(odd) td").parent ().addClass ("dnnGridAltItem");
    // paint headers
    $(table + " tr th").parent ().addClass ("dnnGridHeader").attr ("align", "left");
});