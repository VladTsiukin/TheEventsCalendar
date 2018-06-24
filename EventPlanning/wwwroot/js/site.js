/* site.js */
'use strict';


/*-------------------- DATEPICKER ------------------*/

$('#datepicker').datepicker({
    language: 'ru',
    todayHighlight: false,
    todayBtn: true,
    templates: {
        leftArrow: '<i class="fas fa-arrow-circle-left"></i>',
        rightArrow: '<i class="fas fa-arrow-circle-right"></i>'
    }
});


$('#datepicker').on('changeDate', function () {
    
});
/*------------------------------------------------- */


/* Container d-picker */
$('.container-fields').height(($(document).height() - 180));

/* d-picker-btn */
$('#d-picker-btn').click((e) => {
    const el = document.createElement('h5');
    el.textContent = $('#datepicker').datepicker('getFormattedDate');
     $('.event-h4').after(el);      
});

