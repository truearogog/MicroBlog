BlockJs = (function () {
    function init() {
        $('.block-form').on('submit', e => handleSubmit(e, '.block-form', '.unblock-form'));
        $('.unblock-form').on('submit', e => handleSubmit(e, '.unblock-form', '.block-form'));
    }

    function handleSubmit(e, currentFormSelector, targetFormSelector) {
        e.preventDefault();
        const $form = $(e.target);
        const userId = $form.data('userid');
        const url = $form.attr('action');
        const data = $form.serialize();

        $.post(url, data)
            .done(function () {

                $(targetFormSelector + `[data-userId=${userId}]`).removeClass('d-none');
                $(currentFormSelector + `[data-userId=${userId}]`).addClass('d-none');
            });
    }

    return {
        init
    };
})();
