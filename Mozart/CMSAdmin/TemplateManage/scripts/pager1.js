
function goToPage1(page) {
    $("#curPage1").val(page);
    var list = "list";
    var listId = $("#listId1");
    if (listId[0]) {
        list = listId.val();
    }
    var form = "form";
    var formId = $("#formId1");
    if (formId[0]) {
        form = formId.val();
    }
    AjaxRequest.formRequest(form, list);
}

function changePageSize() {
    goToPage1(1);
}