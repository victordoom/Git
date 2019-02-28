$('#opportunitiesclosed').on('shown.bs.modal', function () {

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
                    data: { id, iduser, closedcomment },
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



$.ajax({
    type: 'POST',
    url: action,
    data: { id, byclose, closedcomment },
    success: function (result) {

        if (result != null) {
            var re = "exito";
        }

    }
});