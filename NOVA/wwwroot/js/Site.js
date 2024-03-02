$(function () {
    $('table').on('post-body.bs.table', function () {
        $('[data-bs-toggle="tooltip"]').tooltip({
            container: 'body',
            trigger: 'hover'
        });
    });
});

function ShowProgress() {
    $('#nova-progress-overlay').fadeIn('fast').css('display', 'flex');
}

function HideProgress() {
    $('#nova-progress-overlay').fadeOut();
}

function StringIsEmpty(text) {
    return typeof text === "string" && text.trim().length === 0;
}