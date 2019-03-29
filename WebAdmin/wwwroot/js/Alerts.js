function GetTechStatusFn(companyid, locationid) {

    $.ajax({
        Type: 'POST',
        url: '/Cases/GetContractTechStaFn',
        dataType: 'json',
        data: { companyid, locationid },
        success: function (Result) {
            if (Result[0].hasTechnicalSpp == "1") {
                TechStatusSuccess();
                $('#TechSuccess').show();
                $('#TechDanger').hide();
                $('#SaveTechnical').show();
            } else {
                TechStatusDanger();
                $('#TechDanger').show();
                $('#TechSuccess').hide();
                $('#SaveTechnical').hide();
            }


        }
    })
}

function TechStatusSuccess() {
    $.notify({
        title: '<strong>Technical Support Enabled</strong>',
        message: 'The technical support service is enabled.'
    }, {
            type: 'success',
            animate: {
                enter: 'animated flipInY',
                exit: 'animated flipOutX'
            }
            
        });

    
}

function TechStatusDanger() {
    $.notify({
        title: '<strong>Technical Support Disabled</strong>',
        message: 'It does not have technical support, please get in touch to enable the service'
    }, {
            type: 'danger',
            animate: {
                enter: 'animated flipInY',
                exit: 'animated flipOutX'
            }
        });
}