PostFeedJs = (function () {
    let _skip = 0;

	function init(container, postUrl, userId, before, pageSize, xsrfToken) {

		$(container).empty();

		const win = $(window);
		win.on('scroll', function () {
			if ($(document).height() - win.height() == win.scrollTop()) {
				fetch(container, postUrl, userId, before, pageSize, xsrfToken);
			}
		});

		fetch(container, postUrl, userId, before, pageSize, xsrfToken);
    }

	function fetch(container, postUrl, userId, before, pageSize, xsrfToken) {
		$('#loading').show();

		$.ajax({
			url: postUrl,
			headers: { RequestVerificationToken: xsrfToken },
			data: { userId, before, skip: _skip, take: pageSize },
			dataType: 'html',
			success: function (html) {
				if (!isEmpty(html)) {
					$(container).append(html);
					_skip += pageSize;
					ReactionsJs.init();
					CommentJs.reloadCommentForms();
					initDeletePostButtons();
				}
				$('#loading').hide();
			}
		});
	}

	function initDeletePostButtons() {
		const a = $('.delete-post-button');
		a.off('click').on('click', function () {
			const button = $(this);
			const postId = button.data('postid');
			button.addClass('d-none');
			$(`.delete-post-form[data-postid=${postId}]`).removeClass('d-none');
		});

		$('.cancel-delete-post-button').off('click').on('click', function () {
			const button = $(this);
			const postId = button.data('postid');
			$(`.delete-post-button[data-postid=${postId}]`).removeClass('d-none');
			$(`.delete-post-form[data-postid=${postId}]`).addClass('d-none');
		});

		$('.delete-post-form').off('submit').on('submit', function (e) {
			e.preventDefault();
			const $form = $(e.target);
			const postId = $form.data('postid');
			const url = $form.attr('action');
			const data = $form.serialize();

			$.post(url, data)
				.done(function () {
					_skip -= 1;
					$(`#post-card-${postId}`).remove();
				});
		});
	}

    return {
        init
    };
})();