document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form[action="/BlogEntry/Create"]');
    const contentDiv = document.querySelector('.form-control.content[contenteditable="true"]');
    const hiddenInput = document.querySelector('input[type="hidden"][name="Content"]');

    form.addEventListener('submit', function () {
        hiddenInput.value = contentDiv.innerHTML;
    });
});

// Elements
const elements = document.querySelectorAll('.btn');


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