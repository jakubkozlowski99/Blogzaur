document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form[action="/BlogEntry/Create"]');
    const contentDiv = document.querySelector('.form-control.content[contenteditable="true"]');
    const hiddenInput = document.querySelector('input[type="hidden"][name="Content"]');

    form.addEventListener('submit', function () {
        hiddenInput.value = contentDiv.innerHTML;
    });
});

// Constants
const elements = document.querySelectorAll('.btn');
const descriptionInput = document.getElementById('description');
const charCounter = document.getElementById('char-counter');

// Events
elements.forEach(element => {
    element.addEventListener('click', () => {
        let command = element.dataset['element'];

        if (command == 'createLink' || command == 'insertImage') {
            let url = prompt('Enter the link here: ', 'http://');
            document.execCommand(command, false, url);
        }
        else document.execCommand(command, false, null);
    });
});

descriptionInput.addEventListener('input', function () {
    const charCount = this.value.length;

    if (charCount > 400) charCounter.style = "color:red;"
    else charCounter.style = "color:black;"

    charCounter.textContent = `${charCount}/400`;
});

