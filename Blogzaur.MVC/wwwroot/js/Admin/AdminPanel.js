$(document).ready(function () {
    $("#createCategoryModal form").submit(function (event) {
        event.preventDefault();

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            data: $(this).serialize(),
            success: function (data) {
                toastr["success"]("Added category")
            },
            error: function () {
                toastr["error"]("Something went wrong")
            }
        })
    });
});