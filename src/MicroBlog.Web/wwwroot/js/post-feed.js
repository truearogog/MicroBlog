PostFeedJs = (function () {
    let _pageNumber = 0;

	function init(container, postUrl, userId, pageSize, xsrfToken) {

		$(container).empty();

		const win = $(window);
		win.on('scroll', function () {
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
			headers: { RequestVerificationToken: xsrfToken },
			data: { userId, skip: _pageNumber * pageSize, take: pageSize },
			dataType: 'html',
			success: function (html) {
				if (html !== "") {
					$(container).append(html);
					_pageNumber++;
				}
				$('#loading').hide();
			}
		});
	}

    return {
        init
    };
})();