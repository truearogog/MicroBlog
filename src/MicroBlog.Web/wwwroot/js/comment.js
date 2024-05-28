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

        $('.comment-container').each(function () {
            const container = $(this);
            const postId = container.data('postid');
            const before = container.data('before');
            const button = $(`.load-more-comments-button[data-postid=${postId}]`);

            if (!(postId in _commentSkips)) {
                _commentSkips[postId] = 0;

                loadComments(button, container, postId, before);
            }
        })
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
                reloadCommentEditButtons();
            });
    }

    function handleLoadComments(e) {
        const button = $(e.target);
        const postId = button.data('postid');
        const before = button.data('before');
        const container = $(`#${postId}-comment-container`);

        loadComments(button, container, postId, before);
    }

    function loadComments(button, container, postId, before) {
        button.addClass('d-none');

        $.ajax({
            url: _commentUrl,
            headers: { RequestVerificationToken: _xsrfToken },
            data: { postId, before, skip: _commentSkips[postId], take: _pageSize },
            dataType: 'html',
            success: function (html) {
                if (!isEmpty(html)) {
                    container.append(html);
                    _commentSkips[postId] += _pageSize;
                    button.removeClass('d-none');
                    reloadCommentEditButtons();
                }
            }
        });
    }

    function reloadCommentEditButtons() {
        $('.delete-comment-button').off('click').on('click', function () {
            const button = $(this);
            const commentId = button.data('commentid');
            button.addClass('d-none');
            $(`.delete-comment-form[data-commentid=${commentId}]`).removeClass('d-none');
        });

        $('.cancel-delete-comment-button').off('click').on('click', function () {
            const button = $(this);
            const commentId = button.data('commentid');
            $(`.delete-comment-button[data-commentid=${commentId}]`).removeClass('d-none');
            $(`.delete-comment-form[data-commentid=${commentId}]`).addClass('d-none');
        });

        $('.delete-comment-form').off('submit').on('submit', function (e) {
            e.preventDefault();
            const $form = $(e.target);
            const commentId = $form.data('commentid');
            const postId = $form.data('postid');
            const url = $form.attr('action');
            const data = $form.serialize();

            $.post(url, data)
                .done(function () {
                    _commentSkips[postId] -= 1;
                    $(`#comment-card-${commentId}`).remove();
                });
        });
    }

    return {
        init,
        reloadCommentForms
    };
})();