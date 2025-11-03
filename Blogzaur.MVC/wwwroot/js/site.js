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

