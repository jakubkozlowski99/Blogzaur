$(document).ready(function () {

    LoadComments()

    $("#createComment form").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (data) {
                toastr["success"]("Comment added")
                var frm = document.getElementsByName('comment-form')[0];
                frm.reset();
                LoadComments()
            },
            error: function () {
                toastr["error"]("Something went wrong")
            }
        })
    });

    // Helper: read antiforgery token from form or page
    function getAntiForgeryToken($form) {
        var token = $form.find('input[name="__RequestVerificationToken"]').val();
        if (token) return token;
        token = $('input[name="__RequestVerificationToken"]').first().val();
        if (token) return token;
        token = $('meta[name="csrf-token"]').attr('content');
        return token || null;
    }

    // Refresh only the likeBlogEntry fragment by fetching the current page and extracting the fragment
    function refreshLikeFragment() {
        return $.get(window.location.href).then(function (html) {
            // parse returned HTML and extract the fragment
            var $parsed = $('<div>').append($.parseHTML(html));
            var $newFragment = $parsed.find('#likeBlogEntry');
            if ($newFragment.length) {
                $('#likeBlogEntry').replaceWith($newFragment);
            } else {
                console.warn('Could not find #likeBlogEntry in returned HTML.');
            }
        }).fail(function () {
            console.warn('Failed to refresh like fragment.');
        });
    }

    // Optionally update DOM if server returns lightweight JSON (keeps UX snappy)
    function applyJsonLikeResponse($form, result) {
        if (result.likeCount !== undefined) {
            $('#blog-like-count').text(result.likeCount);
        }
        if (result.liked !== undefined) {
            var $icon = $form.find('i.fa-heart');
            if (!$icon.length) $icon = $('#likeBlogEntry').find('i.fa-heart');
            if ($icon.length) {
                if (result.liked) {
                    $icon.removeClass('fa-regular').addClass('fa-solid');
                } else {
                    $icon.removeClass('fa-solid').addClass('fa-regular');
                }
            }
        }
    }

    // Intercept like/unlike form submissions and do AJAX; then refresh fragment
    $(document).on('submit', '#like-form, #unlike-form, form.like-form', function (e) {
        e.preventDefault();
        var $form = $(this);
        var url = $form.attr('action');
        var method = ($form.attr('method') || 'POST').toUpperCase();
        var data = $form.serialize();
        var token = getAntiForgeryToken($form);

        var headers = { 'X-Requested-With': 'XMLHttpRequest' };
        if (token) headers['RequestVerificationToken'] = token;

        // Disable submit button to prevent double submits
        var $btn = $form.find('button[type="submit"]');
        $btn.prop('disabled', true);

        $.ajax({
            url: url,
            type: method,
            data: data,
            headers: headers,
            success: function (result) {
                // If server returns JSON with success flag, try to use it to update UI
                if (result && typeof result === 'object' && result.success !== undefined) {
                    if (result.success) {
                        // Prefer incremental DOM update if provided
                        applyJsonLikeResponse($form, result);

                        // Still refresh fragment to ensure server-rendered state (keeps markup logic on server)
                        refreshLikeFragment();

                        // Show feedback with requested phrasing
                        toastr["success"](result.message || (result.liked ? "Blog liked" : "Blog unliked"));
                    } else {
                        toastr["error"](result.message || "Action failed");
                    }
                } else {
                    // If server returned HTML (redirect or full page), refresh the fragment
                    refreshLikeFragment().then(function () {
                        // Inspect replaced fragment to determine new state and show the exact message
                        var liked = $('#likeBlogEntry').find('i.fa-solid.fa-heart').length > 0 || $('#likeBlogEntry').find('#unlike-form').length > 0;
                        toastr["success"](liked ? "Blog liked" : "Blog unliked");
                    });
                }
            },
            error: function (xhr) {
                // If server redirects to login (302 -> HTML), the above path may return HTML; treat as failure otherwise
                toastr["error"]("Server error");
            },
            complete: function () {
                $btn.prop('disabled', false);
            }
        });
    });
});

const RenderComments = (comments, container) => {
    container.empty();

    // server-provided bases (optional)
    const likeUrlBase = '/Comment/LikeComment';
    const unlikeUrlBase = '/Comment/UnlikeComment';
    const token = $('input[name="__RequestVerificationToken"]').first().val() || '';

    for (const comment of comments) {
        const id = comment.id;
        const author = comment.authorName;
        const createdAt = comment.createdAt;
        const content = comment.content;
        const likeCount = comment.likeAmount;
        const isLiked = comment.isLiked;

        // Determine icon class based on liked state and use same classes as blog entry button
        const iconClass = isLiked ? 'fa-solid fa-heart' : 'fa-regular fa-heart';

        // Build action using controller action names: /Like/LikeComment and /Like/UnlikeComment
        const actionUrl = isLiked
            ? `${unlikeUrlBase}?id=${encodeURIComponent(id)}`
            : `${likeUrlBase}?id=${encodeURIComponent(id)}`;

        // Use DELETE method for unlike so client-side interceptor can send DELETE; POST for like
        const formMethod = isLiked ? 'delete' : 'post';

        container.append(
            `<div class="comment-card">
                <div class="comment-header d-flex justify-content-between align-items-center">
                    <div class="comment-meta d-flex align-items-center">
                        <span class="comment-author me-2">${author}</span>
                        <span class="comment-date text-muted small">${createdAt}</span>
                    </div>

                    <div class="comment-like-container d-flex align-items-center">
                        <form class="comment-like-form d-inline-flex align-items-center" method="${formMethod}" action="${actionUrl}" style="margin:0;">
                            <input type="hidden" name="id" value="${id}" />
                            <input type="hidden" name="__RequestVerificationToken" value="${token}" />
                            <button type="submit"
                                    class="like-heart btn p-0 d-inline-flex align-items-center"
                                    aria-label="${isLiked ? 'Unlike' : 'Like'}"
                                    title="${isLiked ? 'Unlike' : 'Like'}"
                                    style="background-color:transparent !important;border:none !important;padding:0;line-height:1;">
                                <i class="${iconClass}" style="vertical-align:middle;font-size:1.3rem;line-height:1;"></i>
                            </button>
                            <span class="comment-like-count ms-2 small" id="comment-like-count-${id}" aria-live="polite" style="line-height:1;align-self:center;">${likeCount}</span>
                        </form>
                    </div>
                </div>

                <hr class="comment-separator" />
                <div class="comment-body">
                    ${content}
                </div>
            </div>`);
    }
}


const LoadComments = () => {
    const container = $("#comments")
    const blogEntryId = container.attr("data-blog-id");

    $.ajax({
        url: `/Comment/GetComments/${blogEntryId}`,
        type: 'get',
        success: function (data) {
            if (!data || !data.length) {
                container.html("There are no comments for this blog entry")
            } else {
                RenderComments(data, container)
            }
        },
        error: function () {
            toastr["error"]("Something went wrong")
        }
    })
}


// Intercept comment like/unlike form submissions, send via AJAX and refresh comments
$(document).on('submit', '.comment-like-form', function (e) {
    e.preventDefault();

    const $form = $(this);
    const url = $form.attr('action');
    const method = ($form.attr('method') || 'post').toUpperCase();
    const data = $form.serialize();
    const token = $form.find('input[name="__RequestVerificationToken"]').val() || $('input[name="__RequestVerificationToken"]').first().val() || '';

    const headers = { 'X-Requested-With': 'XMLHttpRequest' };
    if (token) headers['RequestVerificationToken'] = token;

    const $btn = $form.find('button[type="submit"]');
    $btn.prop('disabled', true);

    $.ajax({
        url: url,
        type: method,
        data: data,
        headers: headers,
        success: function (result) {
            // show simple feedback and refresh comments to reflect new state
            if (result && typeof result === 'object' && result.success !== undefined) {
                if (result.success) {
                    toastr["success"](result.message || (method === 'DELETE' ? "Comment unliked" : "Comment liked"));
                } else {
                    toastr["error"](result.message || "Action failed");
                }
            } else {
                toastr["success"](method === 'DELETE' ? "Comment unliked" : "Comment liked");
            }

            // reload comments to show updated likes
            LoadComments();
        },
        error: function () {
            toastr["error"]("Server error");
        },
        complete: function () {
            $btn.prop('disabled', false);
        }
    });
});