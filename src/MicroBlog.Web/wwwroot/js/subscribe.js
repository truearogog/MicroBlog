SubscribeJs = (function() {
    function init() {
        $('.subscribe-form').on('submit', (e) => handleSubmit(e, '.subscribe-form', '.unsubscribe-form'));
        $('.unsubscribe-form').on('submit', (e) => handleSubmit(e, '.unsubscribe-form', '.subscribe-form'));
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
