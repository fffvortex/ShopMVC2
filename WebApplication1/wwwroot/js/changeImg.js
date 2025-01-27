
function previewImage(file) {
    const reader = new FileReader();
    reader.onload = () => {
        let preview = document.getElementById('preview')
        if (file != null) {
            preview.src = reader.result;
        }
        else {
            return
        }
    }
    if (file != null) {
        reader.readAsDataURL(file);
    }
}