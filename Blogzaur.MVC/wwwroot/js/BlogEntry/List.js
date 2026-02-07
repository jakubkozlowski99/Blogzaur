(function () {
    const maxCategories = 3;
    const addBtn = document.getElementById('addCategoryBtn');
    const select = document.getElementById('categoryAddSelect');
    const container = document.getElementById('selectedCategoriesContainer');
    const warning = document.getElementById('categoryWarning');

    const searchCard = document.getElementById('searchCard');
    const panelToggleText = document.getElementById('panelToggleText');
    const panelToggleTextContainer = document.getElementById('panelToggleTextContainer');
    const panelHideLink = document.getElementById('panelHideLink');
    const clearBtn = document.getElementById('clearBtn');

    function currentCount() {
        return container ? container.querySelectorAll('input[name="selectedCategories"]').length : 0;
    }

    function showWarning(show) {
        if (!warning) return;
        warning.style.display = show ? 'block' : 'none';
    }

    function createBadge(value, text) {
        if (!container) return;

        // match Create/Edit markup: badge without "small" for consistent sizing
        const span = document.createElement('span');
        span.className = 'badge bg-secondary me-1 selected-category';
        span.setAttribute('data-value', value);

        const btn = document.createElement('button');
        btn.type = 'button';
        btn.className = 'btn-close btn-close-white btn-sm ms-2 remove-selected-category';
        btn.setAttribute('aria-label', 'Remove');
        btn.setAttribute('data-value', value);

        span.appendChild(document.createTextNode(text));
        span.appendChild(btn);

        const input = document.createElement('input');
        input.type = 'hidden';
        input.name = 'selectedCategories';
        input.value = value;

        container.appendChild(input);
        container.appendChild(span);
    }

    // Add category button logic
    addBtn?.addEventListener('click', function () {
        if (!select || !container) return;

        const val = select.value;
        if (!val) return;

        const exists = Array.from(container.querySelectorAll('input[name="selectedCategories"]')).some(i => i.value === val);
        if (exists) return;

        if (currentCount() >= maxCategories) {
            showWarning(true);
            return;
        }

        const opt = select.querySelector(`option[value="${CSS.escape(val)}"]`) || select.selectedOptions[0];
        const text = opt ? opt.textContent.trim() : val;

        createBadge(val, text);
        showWarning(currentCount() >= maxCategories);
    });

    // Remove category badges
    container?.addEventListener('click', function (e) {
        const btn = e.target.closest('.remove-selected-category');
        if (!btn) return;
        const val = btn.getAttribute('data-value');
        if (!val) return;

        const inputs = container.querySelectorAll(`input[name="selectedCategories"][value="${CSS.escape(val)}"]`);
        inputs.forEach(i => i.remove());

        const badge = container.querySelector(`.selected-category[data-value="${val}"]`);
        if (badge) badge.remove();

        showWarning(currentCount() >= maxCategories);
    });

    // Prevent Enter from adding accidentally
    select?.addEventListener('keydown', function (e) {
        if (e.key === 'Enter') e.preventDefault();
    });

    // LEFT-SIDE TOGGLE: show panel when clicked (does not submit)
    panelToggleText?.addEventListener('click', function () {
        if (!searchCard) return;
        searchCard.style.display = 'block';
        if (panelToggleTextContainer) panelToggleTextContainer.style.display = 'none';
        if (panelHideLink) panelHideLink.style.display = 'inline-block';
        // focus first input
        const first = searchCard.querySelector('input[name="searchTitle"], select');
        if (first) first.focus();
    });

    // HIDE LINK INSIDE PANEL: hide panel and show left-side toggle
    panelHideLink?.addEventListener('click', function () {
        if (!searchCard) return;
        searchCard.style.display = 'none';
        if (panelToggleTextContainer) panelToggleTextContainer.style.display = 'block';
        panelHideLink.style.display = 'none';
    });

    // Clear behavior: hide panel and show left-side toggle for snappy UX
    clearBtn?.addEventListener('click', function () {
        if (searchCard) searchCard.style.display = 'none';
        if (panelToggleTextContainer) panelToggleTextContainer.style.display = 'block';
        if (panelHideLink) panelHideLink.style.display = 'none';
        // allow navigation to clear filters
    });
})();