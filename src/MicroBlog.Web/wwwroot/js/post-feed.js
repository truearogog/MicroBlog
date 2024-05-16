PostFeedJs = (function () {
    let _pageNumber = 0;

	function init(container, postUrl, userId, pageSize, xsrfToken) {

		const win = $(window);
		win.scroll(function () {
			if ($(document).height() - win.height() == win.scrollTop()) {
				fetch(container, postUrl, userId, pageSize, xsrfToken);
			}
		});

		fetch(container, postUrl, userId, pageSize, xsrfToken);
    }

	function fetch(container, postUrl, userId, pageSize, xsrfToken) {
		$('#loading').show();

		$.ajax({
			url: postUrl,
			headers: {
				RequestVerificationToken: xsrfToken
			},
			data: { userId: userId, skip: _pageNumber * pageSize, take: pageSize },
			dataType: 'html',
			success: function (html) {
				$(container).append(html);
				$('#loading').hide();

				_pageNumber++;
			}
		});
	}

    return {
        init
    };
})();