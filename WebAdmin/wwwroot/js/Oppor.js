var memoria;
var indexuser;
var indexcate;
var indexhow;
var indexrating;
var indexstatus;
$('#advancedSearchModal').on('shown.bs.modal', function () {

});


$(document).ready(function () {

    
    
    var y = $('input[name=Esadmin]')[0].value;
    var idtabla = $('input[name=tablaid]')[0].value;
    if (y == "0") {
        $('#listausu').show();
    } else {
        
        $('#listausu').hide();
    }



    var empTable = $("#user" + idtabla + "").DataTable({

        "ajax": {
            "url": "/Opportunities/GetList",
            "type": "POST",
            "datatype": "json",
            //"data": function (data) {
            
        },
        "columnDefs":
            [{
                //"targets": [0],
                "visible": false,
                "searchable": false,

            }],
        "columns": [
            {
                data: "id",
                name: "ID",
                render: function (data, type, row) {
                    return '<a href="Opportunities/Edit/' + row.id + '" class="btn btn-warning" asp-route-id=""><span class="zmdi zmdi-edit"></span></a>' +
                        '<a href="Opportunities/Details/' + row.id + '" class="btn btn-info" ><span class="zmdi zmdi-square-right"></span></a>' +
                        '<a class="btn btn-success" data-toggle="modal" data-target="#opportunitiesclosed" onclick="OpporDataByClosed(' + row.id + ',' + row.id + ')"><span class="zmdi zmdi-square-right"></span></a>';
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
                render: function (data, type, row) {
                    return '' + row.companyName + ' Phone: ' + row.phoneNumber + ' Address: ' + row.city + ' ' + row.state + ''+ row.timeZone + '';
                },
            },
            { "data": "ownerName", "name": "OwnerName", "autoWidth": true },
            {
                data: "visitedDate",
                name: "VisitedDate",
                render: function (data, type, row) {
                    return moment(row.createdDate).format("MM/DD/YYYY");
                },
            },
            {
                data: "userID",
                name: "UserID",
                render: function (data, type, row) {
                    var user = document.getElementById("user");
                    var usertext;
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
                        return '<span class="label label-primary">' + howfoundtext + '</span>';
                    }

                    return howfoundtext;
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
            { "data": "numberLeadToFollowUp", "name": "NumberLeadToFollowUp", "autoWidth": true },
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
           // alert("Datos cargados exitosamente");
        }
        
           
                
            
        
            
        

        

    });

    memoria = empTable;
    
});


$(document).ready(function () {
  //  memobusqueda();  
});


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

   
    memoria.columns(5).search(x);
    memoria.columns(6).search(y);
    memoria.columns(7).search(z);
    memoria.columns(10).search(a);
    memoria.columns(8).search(b);
    memoria.draw();

}  

function reset() {
    var x = document.getElementById("user");
    var y = document.getElementById("category");
    var z = document.getElementById("howfound");
    var a = document.getElementById("rating");
    var b = document.getElementById("status");

    x.selectedIndex = 0;
    y.selectedIndex = 0;
    z.selectedIndex = 0;
    a.selectedIndex = 0;
    b.selectedIndex = 0;
}

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
    //alert("asta qui");
}

