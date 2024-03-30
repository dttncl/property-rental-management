console.log("Hi")

document.addEventListener('DOMContentLoaded', function () {
    const textarea = document.getElementById('message');
    const placeholder = textarea.getAttribute('placeholder');

    // Set initial placeholder value
    textarea.value = placeholder;

    // When textarea is focused, clear the placeholder value
    textarea.addEventListener('focus', function () {
        if (textarea.value === placeholder) {
            textarea.value = '';
        }
    });

    // When textarea loses focus and is empty, restore the placeholder value
    textarea.addEventListener('blur', function () {
        if (textarea.value.trim() === '') {
            textarea.value = placeholder;
        }
    });
});