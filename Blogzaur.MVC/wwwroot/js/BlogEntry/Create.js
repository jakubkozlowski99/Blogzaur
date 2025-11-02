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

// Categories UI
(function () {
    const $ = window.jQuery;
    const select = document.getElementById('categorySelect');
    const selectedContainer = document.getElementById('selectedCategories');

    if (!select || !selectedContainer) return;

    function containsCategory(id) {
        return !!selectedContainer.querySelector(`input[type="hidden"][name="CategoryIds"][value="${id}"]`);
    }

    function createTag(id, text) {
        const span = document.createElement('span');
        span.className = 'badge bg-secondary me-1 mb-1 d-inline-flex align-items-center';
        span.style.userSelect = 'none';

        const txt = document.createElement('span');
        txt.textContent = text;
        span.appendChild(txt);

        const removeBtn = document.createElement('button');
        removeBtn.type = 'button';
        removeBtn.className = 'btn-close btn-close-white btn-sm ms-2';
        removeBtn.setAttribute('aria-label', 'Remove');
        removeBtn.addEventListener('click', function () {
            span.remove();
        });

        // Hidden input for model binder
        const hidden = document.createElement('input');
        hidden.type = 'hidden';
        hidden.name = 'CategoryIds';
        hidden.value = id;

        span.appendChild(removeBtn);
        span.appendChild(hidden);

        selectedContainer.appendChild(span);
    }

    select.addEventListener('change', function () {
        const id = this.value;
        const text = this.options[this.selectedIndex].text;
        if (!id) return;

        if (containsCategory(id)) {
            this.value = '';
            return;
        }

        createTag(id, text);
        this.value = '';
    });
})();