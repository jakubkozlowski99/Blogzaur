// Pre-populate the contenteditable div with the existing content
document.addEventListener('DOMContentLoaded', function () {
    // category select -> append hidden input + badge (same UX as Create)
    const categorySelect = document.getElementById('categorySelect');
    const selectedContainer = document.getElementById('selectedCategories');

    function alreadySelected(id) {
        return !!document.querySelector('input[name="CategoryIds"][value="' + id + '"]');
    }

    if (categorySelect && selectedContainer) {
        categorySelect.addEventListener('change', function () {
            const value = this.value;
            if (!value) return;

            if (alreadySelected(value)) {
                // optionally show a brief feedback
                this.value = '';
                return;
            }

            const text = this.options[this.selectedIndex].text || value;

            // hidden input for model binding
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'CategoryIds';
            input.value = value;

            // badge with remove button
            const span = document.createElement('span');
            span.className = 'badge bg-secondary me-1 selected-category';
            span.setAttribute('data-id', value);
            span.innerText = text;

            const btn = document.createElement('button');
            btn.type = 'button';
            btn.className = 'btn-close btn-close-white btn-sm ms-2 remove-category';
            btn.setAttribute('aria-label', 'Remove');
            btn.setAttribute('data-id', value);

            span.appendChild(btn);

            selectedContainer.appendChild(input);
            selectedContainer.appendChild(span);

            // reset select
            this.value = '';
        });

        // delegated removal (handles pre-rendered badges too)
        selectedContainer.addEventListener('click', function (e) {
            if (e.target && (e.target.classList.contains('remove-category') || e.target.closest('.remove-category'))) {
                e.preventDefault();
                const btn = e.target.closest('.remove-category');
                const id = btn.getAttribute('data-id');
                if (!id) return;
                // remove inputs and badge(s)
                document.querySelectorAll('input[name="CategoryIds"][value="' + id + '"]').forEach(i => i.remove());
                const badge = btn.closest('.selected-category');
                if (badge) badge.remove();
            }
        });
    }

    // existing Edit.js logic: pre-populate contenteditable from hidden Content field
    var contentDiv = document.querySelector('.form-control.content[contenteditable="true"]');
    var hiddenInput = document.querySelector('input[type="hidden"][name="Content"]');
    if (contentDiv && hiddenInput && hiddenInput.value) {
        contentDiv.innerHTML = hiddenInput.value;
    }
});