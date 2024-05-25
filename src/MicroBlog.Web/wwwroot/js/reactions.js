ReactionsJs = (function () {
    function init() {
        $('.add-reaction-form').off('submit').on('submit', e => handleSubmit(e, '.remove-reaction-form'));
        $('.remove-reaction-form').off('submit').on('submit', e => handleSubmit(e, '.add-reaction-form'));
    }

    function handleSubmit(e, targetFormSelector) {
        e.preventDefault();
        const $form = $(e.target);
        const postId = $form.data('postid');
        const type = $form.data('type');
        const url = $form.attr('action');
        const data = $form.serialize();

        $.post(url, data)
            .done(function () {
                $(targetFormSelector + `[data-postId=${postId}][data-type=${type}]`).removeClass('d-none');
                $form.addClass('d-none');
            });
    }

    return {
        init
    };
})();