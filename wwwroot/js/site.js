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
    /*
    var optionSelect = document.getElementById('optionSelect');
    var txtNameInput = document.getElementById('txtName');
    var txtEmailInput = document.getElementById('txtEmail');
    var txtPhoneInput = document.getElementById('txtPhone');

    
    var selectedValue = optionSelect.value;
    var userID = selectedValue.split('|')[0];
    var txtName = selectedValue.split('|')[2];
    var txtEmail = selectedValue.split('|')[1];
    var txtPhone = selectedValue.split('|')[3];

    txtNameInput.value = txtName;
    txtEmailInput.value = txtEmail;
    txtPhoneInput.value = txtPhone;

    optionSelect.addEventListener('change', function () {

        selectedValue = optionSelect.value;
        userID = selectedValue.split('|')[0];
        txtName = selectedValue.split('|')[2];
        txtEmail = selectedValue.split('|')[1];
        txtPhone = selectedValue.split('|')[3];

        txtNameInput.value = txtName;
        txtEmailInput.value = txtEmail;
        txtPhoneInput.value = txtPhone;
    });
    */
    var selectedOption = localStorage.getItem('selectedOption');
    if (selectedOption) {
        dropdownButton.innerText = selectedOption;
    }

});

var dropdownLinks = document.getElementsByClassName('rs-dd-option');
var dropdownButton = document.getElementById('managerDropdown');

for (var i = 0; i < dropdownLinks.length; i++) {
    dropdownLinks[i].addEventListener('click', function () {
        var selectedOptionText = this.innerText.trim();
        dropdownButton.innerText = selectedOptionText;
        localStorage.setItem('selectedOption', selectedOptionText);
    });
}

// Set the dropdown button text from local storage on page load
document.addEventListener('DOMContentLoaded', function () {
    var selectedOption = localStorage.getItem('selectedOption');
    if (selectedOption) {
        dropdownButton.innerText = selectedOption;
    }
});

var logout = document.getElementById('logoutLink');
logout.addEventListener('click', function () {
    localStorage.clear();
});
