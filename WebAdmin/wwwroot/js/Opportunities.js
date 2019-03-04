$('#opportunitiesclosed').on('shown.bs.modal', function () {

});

$('#opportunitiesreopen').on('shown.bs.modal', function () {

});



function OpporDataByClosed(id, iduser) {
    $('input[name=Id]').val(id);
    $('input[name=IdUser]').val(iduser);
    document.getElementById("titleid").innerText = "#" + id + "";
}
function CerrarOppor(action) {
    var closedcomment = $('textarea[name=ClosedComment]')[0].value;
    var id = $('input[name=Id]')[0].value;
    var iduser = $('input[name=IdUser]')[0].value;
    var reason = document.getElementById("SelectReason").value;

    if (reason == 0) {
        $('#SelectReason').focus();
    } else {
        
       if (closedcomment == "") {
            $('#ClosedComment').focus();
        } else {
           if (id == "") {
               alert("There was a problem getting the ID");
            } else {
                 if (iduser == "") {
                     alert("There was a problem in obtaining the User");
                   } else {

                    $.ajax({
                        type: 'POST',
                        url: action,
                        data: { id, iduser, closedcomment, reason },
                        success: function (result) {

                           if (result == "Exito") {
                            //window.location.href = "Opportunities";
                            location.reload(true);
                            //alert("Closed Opportunities");
                             } else {
                            alert("Error Closed");
                        }

                    }
                });

            }
        }
        
      }

    }

}


function OpporDataReOpen(idopor, iduser) {
    $('input[name=Id]').val(idopor);
    $('input[name=IdUser]').val(iduser);
    document.getElementById("titleid").innerText = "#" + idopor + "";
}

function ReOpenOppor(action) {
    var reopencomment = $('textarea[name=ReopenComment]')[0].value;
    var id = $('input[name=Id]')[0].value;
    var iduser = $('input[name=IdUser]')[0].value;
   // var reason = document.getElementById("SelectReason").value;

   
        if (reopencomment == "") {
            $('#ReopenComment').focus();
        } else {
            if (id == "") {
                alert("There was a problem getting the ID");
            } else {
                if (iduser == "") {
                    alert("There was a problem in obtaining the User");
                } else {

                    $.ajax({
                        type: 'POST',
                        url: action,
                        data: { id, iduser, reopencomment },
                        success: function (result) {

                            if (result == "Exito") {
                                //window.location.href = "Opportunities";
                                location.reload(true);
                                //alert("Closed Opportunities");
                            } else {
                                alert("Error Closed");
                            }

                        }
                    });

                }
            }

        }

    

}