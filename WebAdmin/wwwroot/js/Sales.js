$('#modalComments').on('shown.bs.modal', function () {
    
});

var enmemo;
var enmemoria;
var j = 0;

var userLogeado;

function getComments(email, action) {
    $.ajax({
        type: "POST",
        url: action,
        data: { email },
        success: function (response) {

            if (response.length == 0) {
                nohaycomments()

            } else {
            //los datos que obtenemos con nuestra funcion se las vamos a pasar a la funcion mostarUsuario
            $('input[name=Idby]').val(response[0].userLogeado);
            $('input[name=User]').val(response[0].userLogeadoNombre);
            $('input[name=Idto]').val(response[0].userSelect);
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
    

    $('#modalcomment').html(row);
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
            row += '<button class="direct-chat-name pull-left btn btn-success">' + val.nombre + '</button>';
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
            row += '<button class="direct-chat-name pull-right btn btn-info">' + val.nombre + '</button>';
            row += '<span class="direct-chat-timestamp pull-left">' + val.commentDatetime + '</span>';
            row += '</div>';




            // row += '<img class="direct-chat-img" src="../dist/img/user1-128x128.jpg" alt="message user image">';

            row += '<div class="direct-chat-text">' + val.comment + '</div>';

            row += '</div>';




            row += '</div>';
        }

    });


    row += '<div><span id="final"></span></div>';

   
    
    $('#modalcomment').html(row);
    document.getElementById('final').scrollIntoView(true);
}

function getAdministra(action) {
    $.ajax({
        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j == 0) {
                for (var i = 0; i < response.length; i++) {
                    document.getElementById('Select').options[i] = new Option(response[i].text, response[i].value);
                   // document.getElementById('SelectNuevo').options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }
        }
    });
}


function agregarComment(action) {
    var idby = $('input[name=Idby]')[0].value;
    var idto;
    var esadmin = $('input[name=EsAdmin]')[0].value;

    if (esadmin == "Admin") {
        var siadmin = $('input[name=Idto]')[0].value;
        idto = siadmin

    } else {
       
        var selectAdmin = $('input[name=Idtoadmin]')[0].value;

        idto = selectAdmin
    }

    var comment = $('textarea[name=Comment]')[0].value;
    var title = $('input[name=Title]')[0].value;


    if (idto == "0") {
        $('#Select').focus();
    } else {
        if (title == "") {
            $('#Title').focus();
        } else {

            if (title == "") {
                $('#Title').focus();
            } else {

                if (comment == "") {
                    $('#Comment').focus();
                } else {


                    $.ajax({
                        type: "POST",
                        url: action,
                        data: {
                            idby, idto, comment, title
                        },
                        success: function (response) {
                            if (response = true) {
                                //$('#modalComments').modal('show');
                                $('textarea[name=Comment]').val('');
                               getComments(enmemoria, enmemo);
                            } else {
                                alert("The comment could not be saved");
                            }



                        }
                    });

                }
            }
        }

    }
    
    
}





