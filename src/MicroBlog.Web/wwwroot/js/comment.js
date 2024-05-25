CommentJs = (function () {
    let _commentSkips = {};
    let _commentUrl;
    let _pageSize;
    let _xsrfToken;

    function init(commentUrl, pageSize, xsrfToken) {
        _commentUrl = commentUrl;
        _pageSize = pageSize;
        _xsrfToken = xsrfToken;
    }

    function reloadCommentForms() {
        $('.comment-form').off('submit').on('submit', handleSubmit);
        $('.load-more-comments-button').off('click').on('click', handleLoadComments);

        $('.comment-collapse').off('show.bs.collapse').on('show.bs.collapse', function (e) {
            const collapse = $(e.target);
            const postId = collapse.data('postid');
            const button = $(`#${postId}-collapse-comments-button`);
            button.text('Hide comments');
            button.addClass('active');
        });

        $('.comment-collapse').off('hide.bs.collapse').on('hide.bs.collapse', function (e) {
            const collapse = $(e.target);
            const postId = collapse.data('postid');
            const button = $(`#${postId}-collapse-comments-button`);
            button.text('Show comments');
            button.removeClass('active');
        });


    }

    function handleSubmit(e)
    {
        e.preventDefault();
        const $form = $(e.target);
        const postId = $form.data('postid');
        const url = $form.attr('action');
        const data = $form.serialize();

        $.post(url, data)
            .done(function (html) {
                $(`.comment-form[data-postid=${postId}] textarea`).val('');
                $(`#${postId}-comment-container`).prepend(html);
            });
    }

    function handleLoadComments(e) {
        const button = $(e.target);
        button.addClass('d-none');
        const postId = button.data('postid');
        const before = button.data('before');

        if (!(postId in _commentSkips)) {
            _commentSkips[postId] = 0;
        }

        $.ajax({
            url: _commentUrl,
            headers: { RequestVerificationToken: _xsrfToken },
            data: { postId, before, skip: _commentSkips[postId], take: _pageSize },
            dataType: 'html',
            success: function (html) {
                if (!isEmpty(html)) {
                    $(`#${postId}-comment-container`).append(html);
                    _commentSkips[postId] += _pageSize;
                    button.removeClass('d-none');
                }
            }
        });
    }

    return {
        init,
        reloadCommentForms
    };
})();