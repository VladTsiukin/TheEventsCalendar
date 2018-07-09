/* site.js */
'use strict';

/* DATEPICKER */
$('#datepicker').datepicker({
    language: 'ru',
    todayHighlight: false,
    todayBtn: true,
    templates: {
        leftArrow: '<i class="fas fa-arrow-circle-left"></i>',
        rightArrow: '<i class="fas fa-arrow-circle-right"></i>'
    }
});

/* set css */
$(document).ready(function () {
    const todayBtn = document.querySelector('#datepicker>div>div.datepicker-days>table>tfoot>tr>th.today');
    const tableCalendar = document.querySelector('#datepicker>div>div.datepicker-days>table');
    const tableTr = document.querySelector('#datepicker>div>div.datepicker-days>table>thead>tr:nth-child(3)');

    if (tableCalendar != null) {
        tableCalendar.classList.add('table'); 
        tableTr.classList.add('table-dark');
    }

    // today button event handler
    if (todayBtn != null) {
        todayBtn.addEventListener('click', (event) => {
            setBgTd();
        });
    }
});

// set background today
function setBgTd() {
    $(document).ready(function () {
        const tDate = new Date();
        tDate.setHours(3, 0, 0, 0);
        let ml = tDate.getTime();
        const td =
            document.querySelector('.datepicker-days>table>tbody>tr>td.day[data-date="' + ml + '"]');
        td.style.backgroundColor = 'rgba(23, 162, 184, 0.5)';
    });
}

/* Container d-picker */
$('.container-fields').height(($(document).height() - 180));

/* d-picker-btn */
$('#d-picker-btn').click((e) => {
    const el = document.createElement('h5');
    el.textContent = $('#datepicker').datepicker('getFormattedDate');
     $('.event-h4').after(el);      
});

/* disable button popover  */
$('span.disabledSpan').popover();

/* enable ttoltips */
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

// add zero to the digit if necessary
function addZero(val) {
    if (val >= 10) {
        return val;
    } else {
        return '0' + val;
    };
}

// send promise post xhr request
function sendPostData(url, data, token) {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest();
        xhr.open('POST', url);
        xhr.setRequestHeader('RequestVerificationToken', token);
        xhr.onload = () => {
            if (xhr.status == 200) {
                resolve(xhr.response);
            } else {
                reject(xhr.response);
            }
        };
        xhr.onerror = () => {
            reject(xhr.response);
        };
        xhr.send(data);
    });
}

