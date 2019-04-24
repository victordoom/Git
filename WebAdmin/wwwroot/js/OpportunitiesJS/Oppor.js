var memoria;
var indexuser;
var indexcate;
var indexhow;
var indexrating;
var indexstatus;

var leadonline;
var reasonfilter;
$('#advancedSearchModal').on('shown.bs.modal', function () {

});


$(document).ready(function () {

    leadonline = ""
    reasonfilter = "";
    
    var y = $('input[name=Esadmin]')[0].value;
    var idtabla = $('input[name=tablaid]')[0].value;
    if (y == "0") {
        $('#listausu').show();
    } else {
        
        $('#listausu').hide();
    }

   
    //Load Table Opportunities
    var empTable = $("#user" + idtabla + "").DataTable({

        "ajax": {
            "url": "/Opportunities/GetList",
            "type": "POST",
            "datatype": "json",
             
        },
        
        "columnDefs":
            [{
                
               // "targets": [13],
                "visible": false,
                //"searchable": false,
            }],
        "columns": [
            {
                data: "id",
                name: "ID",
                render: function (data, type, row) {

                    var op = '<a onclick="CargarEdit(' + row.id + ')" class="btn btn-warning" asp-route-id=""><span class="zmdi zmdi-edit"></span></a>';
                    op += '<a href="Opportunities/Details/' + row.id + '" class="btn btn-info" ><span class="zmdi zmdi-square-right"></span></a>';
                    if (row.closed == false) {
                        op += '<a class="btn btn-success" data-toggle="modal" data-target="#opportunitiesclosed" onclick="OpporDataByClosed(' + row.id + ',' + row.userID + ')"><span class="zmdi zmdi-square-right"></span></a>';
                    }
                    return op;
                        
                },
            },
            {
                data: "createdDate",
                name: "CreatedDate",
                render: function (data, type, row) {
                    return moment(row.createdDate).format("MM/DD/YYYY");
                },
            },
            {
                data: "companyName",
                name: "CompanyName",
                width: "15%",
                render: function (data, type, row) {
                    if (row.timeZone == null) {
                        row.timeZone = "";
                    }
                    var Info = '<b> ' + row.companyName + '</b>';
                    Info += '<br>';
                    Info += '' + row.ownerName + '';
                    Info += '<br>';
                    Info += ' Phone: ' + row.phoneNumber + '';
                    Info += '<br>';
                    Info += ' Address: ' + row.city + ' ' + row.state + '';
                    Info += '<br>';
                    Info += ' Time Zone: ' + row.timeZone + '';


                    return Info;
                },
            },
            //{ "data": "ownerName", "name": "OwnerName", "autoWidth": true },
            {
                data: "visitedDate",
                name: "VisitedDate",
                render: function (data, type, row) {
                    return moment(row.visitedDate).format("MM/DD/YYYY");
                },
            },
            {
                data: "userID",
                name: "UserID",
                render: function (data, type, row) {
                    var user = document.getElementById("user");
                    var usertext = "";
                    for (var i = 0; i < user.length; i++) {
                        var id = user[i].value;
                        if (id == row.userID) {
                            usertext =  user[i].text;
                        }
                    }
                     
                    return usertext;
                },
            },
            {
                data: "categoryID",
                name: "CategoryID",
                render: function (data, type, row) {
                    var category = document.getElementById("category");
                    var categorytext;
                    for (var i = 0; i < category.length; i++) {
                        var id = category[i].value;
                        if (id == row.categoryID) {
                            categorytext = category[i].text;
                        }
                    }

                    return categorytext;
                },
            },
            {
                data: "howFoundID",
                name: "HowFoundID",
                render: function (data, type, row) {
                    var howfound = document.getElementById("howfound");
                    var howfoundtext;
                    for (var i = 0; i < howfound.length; i++) {
                        var id = howfound[i].value;
                        if (id == row.howFoundID) {
                            howfoundtext = howfound[i].text;
                        }
                    }

                    if (row.howFoundID == 10) {
                        return '<span class="label label-primary">' + howfoundtext + '</span><br>' + row.numberLeadToFollowUp + '';;
                    }

                    return howfoundtext + '<br>' + row.numberLeadToFollowUp + '';
                },
            },
            {
                data: "closed",
                name: "Closed",
                render: function (data, type, row) {
                    
                    if (row.closed == false) {
                        return '<span class="label label-success">Open</span>';
                    }
                    else {
                        return '<span class="label label-danger">Closed</span>';
                    }

                },
            },
            //{ "data": "numberLeadToFollowUp", "name": "NumberLeadToFollowUp", "autoWidth": true },
            //{ "data": "timeZone", "name": "TimeZone", "autoWidth": true },
            
            {
                data: "rating",
                name: "Rating",
                render: function (data, type, row) {
                    return '<img src="../images/' + row.rating + '.png" alt = "Image" />';
                },
            },
            {
                data: "lastComment",
                name: "LastComment",
                render: function (data, type, row) {
                    return row.lastComment;
                },
            },
            {
                data: "closingReasonID",
                name: "ClosingReasonID",
                render: function (data, type, row) {
                    var Reason = "";
                    var Comment = "";
                    var closingreason = document.getElementById("SelectReasonFilter");
                    for (var i = 0; i < closingreason.length; i++) {
                        var id = closingreason[i].value;
                        if (id == row.closingReasonID) {
                            Reason = closingreason[i].text;
                        }
                    }
                    if (row.closedComment != null) {
                        Comment = row.closedComment;
                    }

                    return '<b>' + Reason + '</b><br>Comment: ' + Comment + '';
                }
            },
            
        ],

        "processing": true, // for show progress bar  
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "serverSide": true, // for process server side  
        stateSave: true,
        //"dom": '<"top"l>rt<"bottom"ip><"clear">',
        "initComplete": function (oSettings, json) {
            memobusqueda(json);

            mostrarLeadonline();
            mostrarClosingReason();
          
        }
        
          

    });
    //Show Closing Reason to Admin
    if (y == "0") {
        empTable.columns(10).visible(true);
    } else {
        empTable.columns(10).visible(false);
    }
    //Save Table in Memo Temp
    memoria = empTable;
   //Filter Lead Online
    FiltroLeadOnline();
    
});

//Parameters to Search Advanced
function SelectUsuario() {
    var x = document.getElementById("user").value;
     indexuser = document.getElementById("user").selectedIndex;
    var y = document.getElementById("category").value;
     indexcate = document.getElementById("category").selectedIndex;
    var z = document.getElementById("howfound").value;
     indexhow = document.getElementById("howfound").selectedIndex;
    var a = document.getElementById("rating").value;
     indexrating = document.getElementById("rating").selectedIndex;
    var b = document.getElementById("status").value;
    indexstatus = document.getElementById("status").selectedIndex;
    var c = document.getElementById("SelectReasonFilter").value;
    indexreason = document.getElementById("SelectReasonFilter").selectedIndex;

    //How Found is Online Run Filter LeadOnline
    if (z == 10) {
        if (leadonline != "") {
            FiltroLeadOnline();
            
    }
    
    }

    //Sending search parameters according to columns to datatable
    memoria.columns(4).search(x);
    memoria.columns(5).search(y);
    memoria.columns(6).search(z);
    memoria.columns(8).search(a);
    memoria.columns(7).search(b);
    memoria.columns(10).search(c);
    memoria.draw();

    
}  
//Restart the advanced search
function reset() {
    var x = document.getElementById("user");
    var y = document.getElementById("category");
    var z = document.getElementById("howfound");
    var a = document.getElementById("rating");
    var b = document.getElementById("status");
    var c = document.getElementById("SelectReasonFilter");

    x.selectedIndex = 0;
    y.selectedIndex = 0;
    z.selectedIndex = 0;
    a.selectedIndex = 0;
    b.selectedIndex = 0;
    c.selectedIndex = 0;

    resetleadonline();
    mostrarLeadonline();
    mostrarClosingReason();
}
//Keep the search in memory
function memobusqueda(json) {
    var lx = document.getElementById("user");
    
    for (var i = 0; i < lx.length; i++) {
        var id = lx[i].value;
        if (id == json.us) {
            lx.selectedIndex = i;

        }
    }

    var lb = document.getElementById("status");
    for (var i = 0; i < lb.length; i++) {
        var id = lb[i].value;
        if (id == json.sta) {
            lb.selectedIndex = i;

        }
    }

    var ly = document.getElementById("category");
    for (var i = 0; i < ly.length; i++) {
        var id = ly[i].value;
        if (id == json.ca) {
            ly.selectedIndex = i;

        }
    }

    var lz = document.getElementById("howfound");
    for (var i = 0; i < lz.length; i++) {
        var id = lz[i].value;
        if (id == json.how) {
            lz.selectedIndex = i;

        }
    }

    var la = document.getElementById("rating");
    for (var i = 0; i < la.length; i++) {
        var id = la[i].value;
        if (id == json.ra) {
            la.selectedIndex = i;

        }
    }
    var lc = document.getElementById("SelectReasonFilter");
    for (var i = 0; i < la.length; i++) {
        var id = lc[i].value;
        if (id == json.rea) {
            lc.selectedIndex = i;

        }
    }
    
}

//Coloring active button
$("#mif0").click(function () {
    
    if ($(this).hasClass("btn-danger") == true) {
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-success");

        leadonline = "F0";
        //red 1
        if ($("#mif1").hasClass("btn-danger") == true) {

        } else {
            $("#mif1").removeClass("btn-success");
            $("#mif1").addClass("btn-danger");
        }
        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }
        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    } else {
        $(this).removeClass("btn-success");
        $(this).addClass("btn-danger");

        leadonline = "reset";

        //red 1
        if ($("#mif1").hasClass("btn-danger") == true) {

        } else {
            $("#mif1").removeClass("btn-success");
            $("#mif1").addClass("btn-danger");
        }

        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }
        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    }
});
$("#mif1").click(function () {
    
    if ($(this).hasClass("btn-danger") == true) {
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-success");

        leadonline = "F1";
        //Green 0
        if ($("#mif0").hasClass("btn-danger") == true) {
            $("#mif0").removeClass("btn-danger");
            $("#mif0").addClass("btn-success");
        } else {
           
        }
        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }
        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    } else {
        $(this).removeClass("btn-success");
        $(this).addClass("btn-danger");

        leadonline = "reset";

        //red 0
        if ($("#mif0").hasClass("btn-danger") == true) {

        } else {
            $("#mif0").removeClass("btn-success");
            $("#mif0").addClass("btn-danger");
        }
        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }
        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    }
});
$("#mif2").click(function () {
    if ($(this).hasClass("btn-danger") == true) {
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-success");

        leadonline = "F2";

        if ($("#mif0").hasClass("btn-danger") == true) {
            $("#mif0").removeClass("btn-danger");
            $("#mif0").addClass("btn-success");
        }
        if ($("#mif1").hasClass("btn-danger") == true) {
            $("#mif1").removeClass("btn-danger");
            $("#mif1").addClass("btn-success");
        }

        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }

    } else {
        $(this).removeClass("btn-success");
        $(this).addClass("btn-danger");

        leadonline = "reset";
        if ($("#mif0").hasClass("btn-danger") == true) {

        } else {
            $("#mif0").removeClass("btn-success");
            $("#mif0").addClass("btn-danger");
        }
        if ($("#mif1").hasClass("btn-danger") == true) {

        } else {
            $("#mif1").removeClass("btn-success");
            $("#mif1").addClass("btn-danger");
        }

        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    }
});
$("#mif3").click(function () {
    if ($(this).hasClass("btn-danger") == true) {
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-success");

        leadonline = "F3";

        //green 0
        if ($("#mif0").hasClass("btn-danger") == true) {
            $("#mif0").removeClass("btn-danger");
            $("#mif0").addClass("btn-success");
        }
        //green 1
            if ($("#mif1").hasClass("btn-danger") == true) {
                $("#mif1").removeClass("btn-danger");
                $("#mif1").addClass("btn-success");
        }
        //green 2
        if ($("#mif2").hasClass("btn-danger") == true) {
            $("#mif2").removeClass("btn-danger");
            $("#mif2").addClass("btn-success");
        }

        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
        
    } else {
        $(this).removeClass("btn-success");
        $(this).addClass("btn-danger");

        leadonline = "reset";

        //red 0
        if ($("#mif0").hasClass("btn-danger") == true) {

        } else {
            $("#mif0").removeClass("btn-success");
            $("#mif0").addClass("btn-danger");
        }
        //red 1
        if ($("#mif1").hasClass("btn-danger") == true) {

        } else {
            $("#mif1").removeClass("btn-success");
            $("#mif1").addClass("btn-danger");
        }
        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }

        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    }
});
$("#mif4").click(function () {
    if ($(this).hasClass("btn-danger") == true) {
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-success");

        leadonline = "F4";

        //green 0
        if ($("#mif0").hasClass("btn-danger") == true) {
            $("#mif0").removeClass("btn-danger");
            $("#mif0").addClass("btn-success");
        }
        //green 1
        if ($("#mif1").hasClass("btn-danger") == true) {
            $("#mif1").removeClass("btn-danger");
            $("#mif1").addClass("btn-success");
        }
        //green 2
        if ($("#mif2").hasClass("btn-danger") == true) {
            $("#mif2").removeClass("btn-danger");
            $("#mif2").addClass("btn-success");
        }
        //green 3
        if ($("#mif3").hasClass("btn-danger") == true) {
            $("#mif3").removeClass("btn-danger");
            $("#mif3").addClass("btn-success");
        }

        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }

    } else {
        $(this).removeClass("btn-success");
        $(this).addClass("btn-danger");

        leadonline = "reset";

        //red 0
        if ($("#mif0").hasClass("btn-danger") == true) {

        } else {
            $("#mif0").removeClass("btn-success");
            $("#mif0").addClass("btn-danger");
        }
        //red 1
        if ($("#mif1").hasClass("btn-danger") == true) {

        } else {
            $("#mif1").removeClass("btn-success");
            $("#mif1").addClass("btn-danger");
        }
        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }
        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 5
        if ($("#mif5").hasClass("btn-danger") == true) {

        } else {
            $("#mif5").removeClass("btn-success");
            $("#mif5").addClass("btn-danger");
        }
    }
});
$("#mif5").click(function () {
    if ($(this).hasClass("btn-danger") == true) {
        $(this).removeClass("btn-danger");
        $(this).addClass("btn-success");

        leadonline = "F5";

        //green 0
        if ($("#mif0").hasClass("btn-danger") == true) {
            $("#mif0").removeClass("btn-danger");
            $("#mif0").addClass("btn-success");
        }
        //green 1
        if ($("#mif1").hasClass("btn-danger") == true) {
            $("#mif1").removeClass("btn-danger");
            $("#mif1").addClass("btn-success");
        }
        //green 2
        if ($("#mif2").hasClass("btn-danger") == true) {
            $("#mif2").removeClass("btn-danger");
            $("#mif2").addClass("btn-success");
        }
        //green 3
        if ($("#mif3").hasClass("btn-danger") == true) {
            $("#mif3").removeClass("btn-danger");
            $("#mif3").addClass("btn-success");
        }
        //green 4
        if ($("#mif4").hasClass("btn-danger") == true) {
            $("#mif4").removeClass("btn-danger");
            $("#mif4").addClass("btn-success");
        }

    } else {
        $(this).removeClass("btn-success");
        $(this).addClass("btn-danger");

        leadonline = "reset";

        //red 0
        if ($("#mif0").hasClass("btn-danger") == true) {

        } else {
            $("#mif0").removeClass("btn-success");
            $("#mif0").addClass("btn-danger");
        }
        //red 1
        if ($("#mif1").hasClass("btn-danger") == true) {

        } else {
            $("#mif1").removeClass("btn-success");
            $("#mif1").addClass("btn-danger");
        }
        //red 2
        if ($("#mif2").hasClass("btn-danger") == true) {

        } else {
            $("#mif2").removeClass("btn-success");
            $("#mif2").addClass("btn-danger");
        }
        //red 3
        if ($("#mif3").hasClass("btn-danger") == true) {

        } else {
            $("#mif3").removeClass("btn-success");
            $("#mif3").addClass("btn-danger");
        }
        //red 4
        if ($("#mif4").hasClass("btn-danger") == true) {

        } else {
            $("#mif4").removeClass("btn-success");
            $("#mif4").addClass("btn-danger");
        }
    }
});

//Color according to the search memory
function FiltroLeadOnline() {

    $.ajax({
        Type: 'POST',
        url: "Opportunities/FiltroLeadOnline",

        data: { leadonline },
        success: function (result) {
            if (result == "F0") {

                if ($("#mif0").hasClass("btn-danger") == true) {
                    $("#mif0").removeClass("btn-danger");
                    $("#mif0").addClass("btn-success");

                    leadonline = "F0";
                    //red 1
                    if ($("#mif1").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif1").removeClass("btn-success");
                        $("#mif1").addClass("btn-danger");
                    }
                    //red 2
                    if ($("#mif2").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif2").removeClass("btn-success");
                        $("#mif2").addClass("btn-danger");
                    }
                    //red 3
                    if ($("#mif3").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif3").removeClass("btn-success");
                        $("#mif3").addClass("btn-danger");
                    }
                    //red 4
                    if ($("#mif4").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif4").removeClass("btn-success");
                        $("#mif4").addClass("btn-danger");
                    }
                    //red 5
                    if ($("#mif5").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif5").removeClass("btn-success");
                        $("#mif5").addClass("btn-danger");
                    }
                } else {
                    
                    leadonline = "";
                    
                }
            }

            if (result == "F1") {

                if ($("#mif1").hasClass("btn-danger") == true) {
                    $("#mif1").removeClass("btn-danger");
                    $("#mif1").addClass("btn-success");

                    leadonline = "F1";
                    //red 2
                    if ($("#mif2").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif2").removeClass("btn-success");
                        $("#mif2").addClass("btn-danger");
                    }
                    //red 3
                    if ($("#mif3").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif3").removeClass("btn-success");
                        $("#mif3").addClass("btn-danger");
                    }
                    //rojo 4
                    if ($("#mif4").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif4").removeClass("btn-success");
                        $("#mif4").addClass("btn-danger");
                    }
                    //red 5
                    if ($("#mif5").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif5").removeClass("btn-success");
                        $("#mif5").addClass("btn-danger");
                    }
                } else {
                    
                    leadonline = "";
                    
                }
            }

                if (result == "F2") {

                    if ($("#mif2").hasClass("btn-danger") == true) {
                        $("#mif2").removeClass("btn-danger");
                        $("#mif2").addClass("btn-success");

                        leadonline = "F2";

                        if ($("#mif1").hasClass("btn-danger") == true) {
                            $("#mif1").removeClass("btn-danger");
                            $("#mif1").addClass("btn-success");
                        }

                        //red 3
                        if ($("#mif3").hasClass("btn-danger") == true) {

                        } else {
                            $("#mif3").removeClass("btn-success");
                            $("#mif3").addClass("btn-danger");
                        }
                        //red 4
                        if ($("#mif4").hasClass("btn-danger") == true) {

                        } else {
                            $("#mif4").removeClass("btn-success");
                            $("#mif4").addClass("btn-danger");
                        }
                        //red 5
                        if ($("#mif5").hasClass("btn-danger") == true) {

                        } else {
                            $("#mif5").removeClass("btn-success");
                            $("#mif5").addClass("btn-danger");
                        }

                    } else {
                        

                        leadonline = "";
                        if ($("#mif1").hasClass("btn-danger") == true) {
                            $("#mif1").removeClass("btn-danger");
                            $("#mif1").addClass("btn-success");

                        } else {
                           
                        }

                        
                    }

                }


            if (result == "F3") {

                if ($("#mif3").hasClass("btn-danger") == true) {
                    $("#mif3").removeClass("btn-danger");
                    $("#mif3").addClass("btn-success");

                    leadonline = "F3";

                    //green 1
                    if ($("#mif1").hasClass("btn-danger") == true) {
                        $("#mif1").removeClass("btn-danger");
                        $("#mif1").addClass("btn-success");
                    }
                    //green 2
                    if ($("#mif2").hasClass("btn-danger") == true) {
                        $("#mif2").removeClass("btn-danger");
                        $("#mif2").addClass("btn-success");
                    }

                    //red 4
                    if ($("#mif4").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif4").removeClass("btn-success");
                        $("#mif4").addClass("btn-danger");
                    }
                    //red 5
                    if ($("#mif5").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif5").removeClass("btn-success");
                        $("#mif5").addClass("btn-danger");
                    }

                } else {
                  
                }

            }

            if (result == "F4") {
                if ($("#mif4").hasClass("btn-danger") == true) {
                    $("#mif4").removeClass("btn-danger");
                    $("#mif4").addClass("btn-success");

                    leadonline = "F4";

                    //green 1
                    if ($("#mif1").hasClass("btn-danger") == true) {
                        $("#mif1").removeClass("btn-danger");
                        $("#mif1").addClass("btn-success");
                    }
                    //green 2
                    if ($("#mif2").hasClass("btn-danger") == true) {
                        $("#mif2").removeClass("btn-danger");
                        $("#mif2").addClass("btn-success");
                    }
                    //green 3
                    if ($("#mif3").hasClass("btn-danger") == true) {
                        $("#mif3").removeClass("btn-danger");
                        $("#mif3").addClass("btn-success");
                    }

                    //red 5
                    if ($("#mif5").hasClass("btn-danger") == true) {

                    } else {
                        $("#mif5").removeClass("btn-success");
                        $("#mif5").addClass("btn-danger");
                    }

                } else {
                    
                    leadonline = "";
                    
                }
            }

            if (result == "F5") {
                if ($("#mif5").hasClass("btn-danger") == true) {
                    $("#mif5").removeClass("btn-danger");
                    $("#mif5").addClass("btn-success");

                    leadonline = "F5";

                    //green 1
                    if ($("#mif1").hasClass("btn-danger") == true) {
                        $("#mif1").removeClass("btn-danger");
                        $("#mif1").addClass("btn-success");
                    }
                    //green 2
                    if ($("#mif2").hasClass("btn-danger") == true) {
                        $("#mif2").removeClass("btn-danger");
                        $("#mif2").addClass("btn-success");
                    }
                    //green 3
                    if ($("#mif3").hasClass("btn-danger") == true) {
                        $("#mif3").removeClass("btn-danger");
                        $("#mif3").addClass("btn-success");
                    }
                    //green 4
                    if ($("#mif4").hasClass("btn-danger") == true) {
                        $("#mif4").removeClass("btn-danger");
                        $("#mif4").addClass("btn-success");
                    }

                } else {
                  
                    leadonline = "";
 
                }
            }


        }
    });
}
//Color everything red
function resetleadonline() {
    leadonline = "reset";
    
    //f1
    if ($("#mif1").hasClass("btn-danger") == true) {
       
    } else {
        $("#mif1").removeClass("btn-success");
        $("#mif1").addClass("btn-danger");
    }
    //f2
    if ($("#mif2").hasClass("btn-danger") == true) {

    } else {
        $("#mif2").removeClass("btn-success");
        $("#mif2").addClass("btn-danger");
    }
    //f3
    if ($("#mif3").hasClass("btn-danger") == true) {

    } else {
        $("#mif3").removeClass("btn-success");
        $("#mif3").addClass("btn-danger");
    }
    //f4
    if ($("#mif4").hasClass("btn-danger") == true) {

    } else {
        $("#mif4").removeClass("btn-success");
        $("#mif4").addClass("btn-danger");
    }
    //f5
    if ($("#mif5").hasClass("btn-danger") == true) {

    } else {
        $("#mif5").removeClass("btn-success");
        $("#mif5").addClass("btn-danger");
    }
}

//Show hidden fields of advanced search
function mostrarLeadonline() {
    var estado = document.getElementById("howfound").value;
    var status = document.getElementById("status").value;
    var reason = document.getElementById("SelectReasonFilter");

    if (estado == 10) {
        $('#LeadOnline').show();
            if (status == 1) {
               $('#ReasonFilter').show();
            }  
        
    } else {
        $('#LeadOnline').hide();
        $('#ReasonFilter').hide();
        reason.selectedIndex = 0;
        resetleadonline();
    }
    
   
}
function mostrarClosingReason() {

    var estado = document.getElementById("howfound").value;
    var status = document.getElementById("status").value;
    var reason = document.getElementById("SelectReasonFilter");

    if (estado == 10 && status == 1) {
        $('#ReasonFilter').show();
    } else {
        $('#ReasonFilter').hide();
        reason.selectedIndex = 0;
    }
}