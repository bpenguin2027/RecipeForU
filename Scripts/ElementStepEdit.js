function addElement() {
    document.getElementById('ActionNo').value = 'ElementAdd';
    document.getElementById('RowID').value = '0';
    $('#AddRecipeForm').submit();
}

function editElement(id) {
    document.getElementById('ActionNo').value = 'ElementEdit';
    document.getElementById('RowID').value = id;
    $('#AddRecipeForm').submit();
}

function deleteElement(id) {
    if (confirm('是否確定要刪除？')) {
        document.getElementById('ActionNo').value = 'ElementDelete';
        document.getElementById('RowID').value = id;
        $('#AddRecipeForm').submit();
    } else {
        return false;
    }
}

function addStep() {
    document.getElementById('ActionNo').value = 'StepAdd';
    document.getElementById('RowID').value = '0';
    $('#AddRecipeForm').submit();
}

function editStep(rowid, step_id) {
    document.getElementById('ActionNo').value = 'StepEdit';
    document.getElementById('StepNo').value = step_id;
    document.getElementById('RowID').value = rowid;
    $('#AddRecipeForm').submit();
}

function deleteStep(rowid, step_id) {
    if (confirm('是否確定要刪除？')) {
        document.getElementById('ActionNo').value = 'StepDelete';
        document.getElementById('StepNo').value = step_id;
        document.getElementById('RowID').value = rowid;
        $('#AddRecipeForm').submit();
    } else {
        return false;
    }
}

function saveRecipe() {
    document.getElementById('ActionNo').value = 'SaveRecipe';
    document.getElementById('RowID').value = '0';
    alert('新增食譜成功！');
    $('#AddRecipeForm').submit();
}

function uploadRecipeImage() {
    document.getElementById('ActionNo').value = 'RecipeUpload';
    document.getElementById('RowID').value = '0';
    $('#AddRecipeForm').submit();
}

function uploadStep(id) {
    document.getElementById('ActionNo').value = 'StepUpload';
    document.getElementById('RowID').value = id;
    $('#AddRecipeForm').submit();
}