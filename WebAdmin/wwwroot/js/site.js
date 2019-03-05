// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function filtropiechart() {
    var x = document.getElementById("Select").value;

    filtrar(x);
    filtrardos(x);
    filtrocasesopened(x);
    filtromostcategory(x);
    filtrocasesclosed(x);
    filtrovisited(x);
    filtroopportunitiesonline(x);
    ocultarchat(x);

    if (x != 0) {
        filtrochat(x);
    }
    
    


    
}

function filtrar(x) {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getconsulta/' + x + '',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'HowFoung');

        data.addColumn('number', 'cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.howFoung, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
            // title: 'HOW FOUND',
            height: 240,
            chartArea: { width: '98%', height: '80%' },
            hAxis: { showTextEvery: 7, textStyle: { fontSize: '10' } },
            legend: { position: 'top', textStyle: { color: 'blue', fontSize: 12 } },
            lineWidth: 4,
            pointShape: 'circle',
            pointSize: 6,
            vAxis: { textPosition: 'in', gridlines: { count: 3 }, minorGridlines: { count: 2 }, textStyle: { fontSize: 12 } },
            is3D: true,
            slices: { 0: { color: '#578CCF' }, 1: { color: '#85BF51' }, 2: { color: '#FFA219' }, 3: { color: '#1CB1F5' }, 4: { color: '#C5D2DF' }, 5: { color: '#9D96D1' }, 6: { color: '#C9C9C9' }, 7: { color: '#2B7BE4' } },
        };
        var piechart = new google.visualization.PieChart(document.getElementById('piechart_div'));
        piechart.draw(data, piechart_options);


    }
}
function filtrardos(x) {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdataset/' + x + '',
        success: function (result) {
            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);
            });
        }
    });

    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'Category');

        data.addColumn('number', 'Cases');


        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.category, obj.cases]);
        });

        data.addRows(dataArray);

        var piechart_options = {
            //  title: 'CATEGORY',
            height: 240,
            chartArea: { width: '98%', height: '80%' },
            hAxis: { showTextEvery: 7, textStyle: { fontSize: '10' } },
            legend: { position: 'top', textStyle: { color: 'blue', fontSize: 12 } },
            lineWidth: 4,
            pointShape: 'circle',
            pointSize: 6,
            vAxis: { textPosition: 'in', gridlines: { count: 3 }, minorGridlines: { count: 2 }, textStyle: { fontSize: 12 } },
            is3D: true,
            slices: { 0: { color: '#578CCF' }, 1: { color: '#85BF51' }, 2: { color: '#FFA219' }, 3: { color: '#1CB1F5' }, 4: { color: '#C5D2DF' }, 5: { color: '#9D96D1' }, 6: { color: '#C9C9C9' }, 7: { color: '#2B7BE4' } },
        };
        var piechart = new google.visualization.PieChart(document.getElementById('piechart_divcate'));
        piechart.draw(data, piechart_options);


    }
}


$(document).ready(function () {

    var x = $('input[name=Esadmin]')[0].value;

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/casesopened/' + x + '',
        success: function (result) {

            document.getElementById("casesopened").innerText = result;
            
        }
    });

    
});

$(document).ready(function () {
    var x = $('input[name=Esadmin]')[0].value;
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/mostcategory/' + x + '',
        success: function (result) {

            document.getElementById("mostcategory").innerText = result;

        }
    });


});

function filtrovisited(x) {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getdatasetvisitedday/' + x + '',
        success: function (result) {

            google.charts.load('current', { 'packages': ['corechart'] });

            google.charts.setOnLoadCallback(function () {
                drawChart(result);

            var suma = 0;
            for (var i = 0; i < result.length; i++) {

                suma += result[i].cases;

            }
            //const sumValues = result => Object.values(result).reduce((result.cases) => result.cases + result.cases);

                document.getElementById("visited").innerText = suma;
            });
        }

    });


    function drawChart(result) {
        var data = new google.visualization.DataTable();
        data.addColumn('string', 'date');

        data.addColumn('number', 'cases');
        data.addColumn({ type: 'string', role: 'style' });

        var dataArray = [];
        $.each(result, function (i, obj) {
            dataArray.push([obj.date, obj.cases, 'color: #1CB1F5']);
        });

        data.addRows(dataArray);

        var piechart_options = {
            //title: 'VISITED BY DATE',
            bar: { groupWidth: "90%" },
            legend: { position: "none" },


        };
        var piechart = new google.visualization.ColumnChart(document.getElementById('visiteddate_div'));
        piechart.draw(data, piechart_options);


    }
}
 


function filtrocasesopened(x) {
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/casesopened/' + x + '',
        success: function (result) {

            document.getElementById("casesopened").innerText = result;

        }
    });
}

function filtromostcategory(x) {
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/mostcategory/' + x + '',
        success: function (result) {

            document.getElementById("mostcategory").innerText = result;

        }
    });
}

function filtrocasesclosed(x) {

    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getcurrentmonth',
        success: function (result) {

            $.each(result, function (index, val) {

                if (x == 0) {
                    document.getElementById("contract").innerText = result[0].quantityReal;
                } else {
                    var monthby = result[0].goalMonth;
                    var yearby = result[0].goalYear;

                    CasesClosed(monthby, yearby, x);

                }

            });


        }
    });
}
function CasesClosed(month, year, x) {
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getbycurrentmonth/' + month + '/' + year + '',

        success: function (response) {
            var result = response;

            for (var i = 0; i < result.length; i++) {
                if (result[i].salesID == x) {
                    document.getElementById("contract").innerText = result[i].cases;
                    break;
                } else {
                    document.getElementById("contract").innerText = 0;
                }
            }

            
        }
    });
}


function Current() {
    
    var container = $('html, body'),
        scrollTo = $('#current');

    container.animate({
        scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
    });
}

$(document).ready(function () {
    var x = $('input[name=Esadmin]')[0].value;
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getcurrentmonth',
        success: function (result) {

            $.each(result, function (index, val) {

                if (x == 0) {
                    document.getElementById("contract").innerText = result[0].quantityReal;
                } 

            });


        }
    });


});

function filtroopportunitiesonline(x) {
    var row = '';
    $.ajax({
        type: 'GET',
        dataType: "json",
        contentType: "application/json",
        url: '/api/Dashboard/getopportunitiesonline/' + x + '',
        success: function (response) {

            var result = response;

            $.each(result, function (index, val) {

                row += '<tr>';
                row += '<td colspan="2">';
                row += '<table  width="100%" class="table-bordered">';
                row += '<thead style="background: #4C8BF5; ">';

                row += '<tr>';
                row += '<th>' + val.correlativo + '</th>';
                row += '<th>' + val.date + '</th>';

                row += '<th>' + val.salesman + '</th>';

                row += '<th >' + val.howFoundName + '</th>';
                row += '<th>' + val.numberLead + '</th>';
                row += '<th colspan="6">' + val.programName + '</th>';
                row += '</tr>';


                row += '</thead>';
                row += '<tbody>';

                row += '<tr >';
                row += '<td colspan="4"><b>' + val.company + '</b></td>';
                row += '<td style="text-align:center">' + val.phoneNumber + '</td>';
                if (val.vrfd1 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd1 + '</td>';
                }
                if (val.vrfd2 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd2 + '</td>';
                }
                if (val.vrfd3 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd3 + '</td>';
                }
                if (val.vrfd4 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd4 + '</td>';
                }
                if (val.vrfd5 == 00) {
                    row += '<td></td>';
                } else {
                    row += '<td>' + val.vrfd5 + '</td>';
                }
                row += '<td>R</td>';
                row += '</tr>';
                // row += '<td colspan="6" style="text-align:right; background: #4C8BF5; color: white;">' + val.programName + '</td>';
                row += '</tr>';

                row += '<tr>';
                row += '<td colspan="4" style="text-align:center">' + val.email + '</td>';
                row += '<td style="text-align:center">' + val.date + '</td>';
                if (val.vrfd1 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd2 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                }
                else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd3 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd4 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }
                if (val.vrfd5 > 1) {
                    row += '<td><img  src="../images/che.png" width="15" height="20"/></td>';
                } else {
                    row += '<td><img  src="../images/close.png" width="15" height="20"/></td>';
                }

                if (val.rating == "Cold") {
                    row += '<td><img  src="../images/Cold.png" width="15" height="20"/></td>';
                }
                if (val.rating == "Warm") {
                    row += '<td><img  src="../images/Warm.png" width="15" height="20"/></td>';
                }

                row += '<tr>';
                row += '<td  colspan="11">' + val.lastFollowup + '</td>';
                row += '</tr>';



                row += '</tbody>';
                row += '</table>';
                row += '</td>';
                row += '</tr>';

            });


            $('#opponline').html(row);


        }
    });
}

function ocultarchat(x) {
    var y = $('input[name=Esadmin]')[0].value;
    if (x == 0 && y == 0) {
        $('#chat').hide();
    } else {
        $('#chat').show();
    }
    

    
}

$(document).ready(function () {
    var x = $('input[name=Esadmin]')[0].value;
    var y = document.getElementById("Select").value;

    if (x == 0 && y == 0) {
        $('#chat').hide();
    } else {
        $('#chat').show();
    }


});


function filtrochat(x) {
    var email = $('input[name=Email]')[0].value;

    getCommentsfiltro(email, x);

}

function getCommentsfiltro(email, x) {
    var usuarionormal = x

    $.ajax({
        type: "POST",
        url: '../Opportunities/filtromostarCommentsOppor',
        data: { email, usuarionormal },
        success: function (response) {

            if (response.length == 0) {
                nohaycomments()

            } else {

                //los datos que obtenemos con nuestra funcion se las vamos a pasar a la funcion mostarUsuario
                $('input[name=Idby]').val(response[0].userLogeado);
                $('input[name=User]').val(response[0].userLogeadoNombre);
                // $('input[name=Idto]').val(response[0].userSelect);
                $('input[name=UserSelect]').val(response[0].userSelectNombre);
                mostrarComments(response);
                enmemoria = email;
                enmemo = action;
            }

        }
    });
}

function nohaycomments() {
    var row = '';


    $('#opporcomment').html(row);
}
function mostrarComments(response) {
    j = 0;

    var row = '';

    items = response;


    $.each(items, function (index, val) {

        if (val.userLogeado == val.commentBy) {
            // row += '<div class="box-body">';

            // row += '<div class="direct-chat-messages">';

            row += '<div class="direct-chat-msg">';
            row += '<div class="direct-chat-info clearfix">';
            row += '<span class="direct-chat-name pull-left  btn-success">' + val.nombre + '</span>';
            row += '<span class="direct-chat-timestamp pull-right">' + val.commentDatetime + '</span>';
            row += '</div>';




            // row += '<img class="direct-chat-img" src="../dist/img/user1-128x128.jpg" alt="message user image">';

            row += '<div class="direct-chat-text">' + val.comment + '</div>';

            row += '</div>';




            row += '</div>';

            // row += '</div>';

            //  row +=        '</div>'




        } else {

            row += '<div class="direct-chat-msg right">';
            row += '<div class="direct-chat-info clearfix">';
            row += '<span class="direct-chat-name pull-right  btn-info">' + val.nombre + '</span>';
            row += '<span class="direct-chat-timestamp pull-left">' + val.commentDatetime + '</span>';
            row += '</div>';




            // row += '<img class="direct-chat-img" src="../dist/img/user1-128x128.jpg" alt="message user image">';

            row += '<div class="direct-chat-text">' + val.comment + '</div>';

            row += '</div>';




            row += '</div>';
        }

    });


    row += '<div><span id="final"></span></div>';



    $('#opporcomment').html(row);
    var container = $('#opporcomment'),
        scrollTo = $('#final');

    container.animate({
        scrollTop: scrollTo.offset().top - container.offset().top + container.scrollTop()
    });

    //document.getElementById('final').scrollIntoView(true);
}