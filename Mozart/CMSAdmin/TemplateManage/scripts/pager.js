
function goToPage(page) {
    $("#curPage").val(page);
    var list = "list";
    var listId = $("#listId");
    if (listId[0]) {
        list = listId.val();
    }
    var form = "form";
    var formId = $("#formId");
    if (formId[0]) {
        form = formId.val();
    }
    AjaxRequest.formRequest(form, list);
}

function changePageSize() {
    goToPage(1);
}