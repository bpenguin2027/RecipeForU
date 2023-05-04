$(document).ready(
    function elementEdit() {
        $("#AddElementButton").click(function () {
            var count = parseInt($('#ElementCount').val(), 10);
            var newCount = count + 1;

            var newColumnDiv = $(document.createElement('div')).attr(
                {
                    "id": 'ElementScope' + newCount
                });

            $(newColumnDiv).append(
                '<input class="form-control col-12 col-md-7 text-box single-line" data-val="true" data-val-required="食材名稱不可空白" id="eRECIPE_'
                + newCount + '__element_id" name="eRECIPE['
                + newCount + '].element_id" placeholder = "食材" type="text" value="">'
                + '<span class="field-validation-valid text-danger" data-valmsg-for="eRECIPE['
                + newCount + '].element_id" data-valmsg-replace="true"></span>'
                + '<input class="form-control col-12 offset-md-1 col-md-4 text-box single-line" data-val="true" data-val-required="食材用量不可空白" id="eRECIPE_'
                + newCount + '__qty" name="eRECIPE['
                + newCount + '].qty" placeholder = "份量" type="text" value="">'
                + '<span class="field-validation-valid text-danger" data-valmsg-for="eRECIPE['
                + newCount + '].qty" data-valmsg-replace="true"></span>'
            );

            $("#ElementEditor").append(newColumnDiv);
            $("#ElementCount").val(newCount);
        });

        $("#RemoveElementButton").click(function () {
            var count = parseInt($('#ElementCount').val(), 10);
            if (count == 0) {
                alert("已沒有可移除的食材");
                return false;
            }
            $("#ElementScope" + count).remove();
            var newCount = count - 1;
            $('#ElementCount').val(newCount);
        });
    });

$(document).ready(
    function stepEdit() {
        $("#AddStepButton").click(function () {
            var count = parseInt($('#StepCount').val(), 10);
            var newCount = count + 1;

            var newColumnDiv = $(document.createElement('div')).attr(
                {
                    "id": 'StepScope' + newCount
                });

            $(newColumnDiv).append(
                '<input class="form-control col-12 col-md-1 text-box single-line" id="sRECIPE_'
                + newCount + '__step_id" name="sRECIPE['
                + newCount + '].step_id" type="text" value="'
                + (newCount + 1) + '">'
                + '<span class="field-validation-valid text-danger" data-valmsg-for="sRECIPE['
                + newCount + '].step_id" data-valmsg-replace="true"></span>'
                + '<textarea class="form-control col-12 offset-md-1 col-md-10" cols="20" data-val="true" data-val-required="步驟說明不可空白" id="sRECIPE_'
                + newCount + '__step_body" maxlength="250" name="sRECIPE['
                + newCount + '].step_body" rows="2"></textarea>'
                + '<span class="field-validation-valid text-danger" data-valmsg-for="sRECIPE['
                + newCount + '].step_body" data-valmsg-replace="true"></span>'
            );

            $("#StepEditor").append(newColumnDiv);
            $("#StepCount").val(newCount);
        });

        $("#RemoveStepButton").click(function () {
            var count = parseInt($('#StepCount').val(), 10);
            if (count == 0) {
                alert("已沒有可移除的步驟");
                return false;
            }
            $("#StepScope" + count).remove();
            var newCount = count - 1;
            $('#StepCount').val(newCount);
        });
    });