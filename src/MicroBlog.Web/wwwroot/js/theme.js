(function () {
    const getStoredTheme = () => localStorage.getItem('theme');
    const setStoredTheme = theme => localStorage.setItem('theme', theme);

    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    const setTheme = theme => {
        if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark');
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme);
        }
    }

    setTheme(getPreferredTheme());

    const showActiveTheme = (theme, focus = false) => {
        if (theme == 'null' || !theme) {
            return;
        }

        const themeSwitcher = document.querySelector('#theme-switcher');
        if (!themeSwitcher) {
            return;
        }

        const themeSwitcherText = document.querySelector('#theme-text');
        const activeThemeIcon = document.querySelector('#theme-icon');
        const btnToActive = document.querySelector(`[data-bs-theme-value="${theme}"]`);
        const themeIconOfActiveBtn = btnToActive.querySelector('i').getAttribute('class');

        document.querySelectorAll('[data-bs-theme-value]').forEach(x => {
            x.classList.remove('active');
            x.setAttribute('aria-pressed', 'false');
        });
        btnToActive.classList.add('active');
        btnToActive.setAttribute('aria-pressed', 'true');
        activeThemeIcon.setAttribute('class', themeIconOfActiveBtn);
        const themeSwitcherLabel = `${themeSwitcherText.textContent} (${theme})`;
        themeSwitcher.setAttribute('aria-label', themeSwitcherLabel);

        if (focus) {
            themeSwitcher.focus();
        }
    };

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme()
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme())
        }
    });

    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme());
        document.querySelectorAll('[data-bs-theme-value]').forEach(x => {
            x.addEventListener('click', () => {
                const theme = x.getAttribute('data-bs-theme-value');
                setStoredTheme(theme);
                setTheme(theme);
                showActiveTheme(theme, true);
            })
        });
    })
})()