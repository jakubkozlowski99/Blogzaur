const RenderComments = (comments, container) => {
    container.empty();

    for (const comment of comments) {
        container.append(
            `<div class="card border-secondary mb-3">
          <div class="card-header row">
              <div>
                ${comment.authorName},
              </div>
              <div>
                ${comment.createdAt}
              </div>
          </div>
          <div class="card-body">
            <h5 class="card-title">${comment.content}</h5> 
          </div>
        </div>`)
    }
}


const LoadComments = () => {
    const container = $("#comments")
    const blogEntryId = container.attr("data-blog-id");

    $.ajax({
        url: `/Comment/GetComments/${blogEntryId}`,
        type: 'get',
        success: function (data) {
            if (!data.length) {
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
