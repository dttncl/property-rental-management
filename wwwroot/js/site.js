console.log("Hi")

document.addEventListener('DOMContentLoaded', function () {

    // textarea
    const textarea = document.getElementById('message');
    const placeholder = textarea.getAttribute('placeholder');

    textarea.value = placeholder;

    // textarea is focused, clear the placeholder value
    textarea.addEventListener('focus', function () {
        if (textarea.value === placeholder) {
            textarea.value = '';
        }
    });

    // textarea loses focus and is empty, restore the placeholder value
    textarea.addEventListener('blur', function () {
        if (textarea.value.trim() === '') {
            textarea.value = placeholder;
        }
    });

    // subject
    const subject = document.getElementById('subject');
    const subplaceholder = subject.getAttribute('placeholder');

    subject.value = subplaceholder;

    subject.addEventListener('focus', function () {
        if (subject.value === subplaceholder) {
            subject.value = '';
        }
    });

    subject.addEventListener('blur', function () {
        if (subject.value.trim() === '') {
            subject.value = subplaceholder;
        }
    });


    // dropdowns
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


    // NEW ONE
    var optionSelect = document.getElementById('optionSelect');
    var txtName = document.getElementById('txtName');
    var txtEmail = document.getElementById('txtEmail');
    var txtPhone = document.getElementById('txtPhone');

    var selectedValue = optionSelect.value;
    var userID = selectedValue.split('|')[0];
    var txtName = userID = selectedValue.split('|')[2];
    var txtEmail = userID = selectedValue.split('|')[1];
    var txtPhone = userID = selectedValue.split('|')[3];

    txtName.value = txtName;
    txtEmail.value = txtEmail;
    txtPhone.value = txtPhone;

    optionSelect.addEventListener('change', function () {
        var selectedValue = optionSelect.value;
        var userID = selectedValue.split('|')[0];
        var txtName = userID = selectedValue.split('|')[2];
        var txtEmail = userID = selectedValue.split('|')[1];
        var txtPhone = userID = selectedValue.split('|')[3];

        txtName.value = txtName;
        txtEmail.value = txtEmail;
        txtPhone.value = txtPhone;

    });




});
