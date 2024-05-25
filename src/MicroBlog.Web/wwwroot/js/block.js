BlockJs = (function () {
    function init() {
        $('.block-form').on('submit', e => handleSubmit(e, '.block-form', '.unblock-form', false));
        $('.unblock-form').on('submit', e => handleSubmit(e, '.unblock-form', '.block-form', true));
    }

    function handleSubmit(e, currentFormSelector, targetFormSelector, showSubscribeForms) {
        e.preventDefault();
        const $form = $(e.target);
        const userId = $form.data('userid');
        const url = $form.attr('action');
        const data = $form.serialize();

        $.post(url, data)
            .done(function () {
                $(targetFormSelector + `[data-userId=${userId}]`).removeClass('d-none');
                $(currentFormSelector + `[data-userId=${userId}]`).addClass('d-none');
                toggleSubscribeForms(showSubscribeForms, userId);
            });
    }

    function toggleSubscribeForms(show, userId) {
        const forms = $(`.subscribe-forms[data-userId=${userId}]`);
        if (forms) {
            if (show) forms.removeClass('d-none');
            else forms.addClass('d-none');
            forms.find('.subscribe-form').removeClass('d-none');
            forms.find('.unsubscribe-form').addClass('d-none');
        }
    }

    return {
        init
    };
})();
