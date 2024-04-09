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

var managerLink = document.getElementById("rs-icon-manager-link");
var managerIcon = document.getElementById("rs-icon-manager");
var tenantLink = document.getElementById("rs-icon-tenant-link");
var tenantIcon = document.getElementById("rs-icon-tenant");
var bldgLink = document.getElementById("rs-icon-bldg-link");
var bldgIcon = document.getElementById("rs-icon-bldg");

managerLink.addEventListener("mouseover", function () {
    managerIcon.src = "/images/icon_managers.png";
});
managerLink.addEventListener("mouseout", function () {
    managerIcon.src = "/images/icon_managers_hover.png";
});

tenantLink.addEventListener("mouseover", function () {
    tenantIcon.src = "/images/icon_tenants.png";
});
tenantLink.addEventListener("mouseout", function () {
    tenantIcon.src = "/images/icon_tenants_hover.png";
});

bldgLink.addEventListener("mouseover", function () {
    bldgIcon.src = "/images/icon_bldg.png";
});
bldgLink.addEventListener("mouseout", function () {
    bldgIcon.src = "/images/icon_bldg_hover.png";
});