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

    var tenantSelect = document.getElementById('tenantSelect');
    var tenantNameTextbox = document.getElementById('tenantName');
    var tenantEmailTextbox = document.getElementById('tenantEmail');
    var tenantPhoneTextbox = document.getElementById('tenantPhone');

    var selectedValue = tenantSelect.value;
    var tenantID = selectedValue.split('|')[0];
    var tenantName = tenantID = selectedValue.split('|')[2];
    var tenantEmail = tenantID = selectedValue.split('|')[1];
    var tenantPhone = tenantID = selectedValue.split('|')[3];

    tenantNameTextbox.value = tenantName;
    tenantEmailTextbox.value = tenantEmail;
    tenantPhoneTextbox.value = tenantPhone;

    tenantSelect.addEventListener('change', function () {
        var selectedValue = tenantSelect.value;
        var tenantID = selectedValue.split('|')[0];
        var tenantName = tenantID = selectedValue.split('|')[2];
        var tenantEmail = tenantID = selectedValue.split('|')[1];
        var tenantPhone = tenantID = selectedValue.split('|')[3];

        tenantNameTextbox.value = tenantName;
        tenantEmailTextbox.value = tenantEmail;
        tenantPhoneTextbox.value = tenantPhone;

    });



});
