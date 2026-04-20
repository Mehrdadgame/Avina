// Avina Application Scripts

// Header scroll shadow
window.addEventListener('scroll', () => {
    const topbar = document.getElementById('topbar');
    if (topbar) {
        topbar.classList.toggle('scrolled', window.scrollY > 10);
    }
});

// Mobile sidebar toggle
document.addEventListener('DOMContentLoaded', function() {
    function initSidebar() {
        const toggleBtn = document.getElementById('toggleSidebar');
        const sidebar = document.querySelector('.sidebar');
        if (!toggleBtn || !sidebar) return;

        // Create backdrop if not exists
        let backdrop = document.querySelector('.sidebar-backdrop');
        if (!backdrop) {
            backdrop = document.createElement('div');
            backdrop.className = 'sidebar-backdrop';
            document.body.appendChild(backdrop);
        }

        function openSidebar() {
            sidebar.classList.add('open');
            backdrop.classList.add('active');
            document.body.style.overflow = 'hidden';
        }
        function closeSidebar() {
            sidebar.classList.remove('open');
            backdrop.classList.remove('active');
            document.body.style.overflow = '';
        }

        toggleBtn.addEventListener('click', () => {
            if (sidebar.classList.contains('open')) closeSidebar();
            else openSidebar();
        });

        backdrop.addEventListener('click', closeSidebar);

        // Close on nav link click (mobile)
        sidebar.querySelectorAll('.nav-item').forEach(link => {
            link.addEventListener('click', () => {
                if (window.innerWidth <= 768) closeSidebar();
            });
        });
    }

    initSidebar();

    // Re-init after Blazor navigation (enhanced nav)
    document.addEventListener('blazor:navigated', initSidebar);
});

// Local storage utilities
const AppStorage = {
    set: (key, value) => localStorage.setItem(key, JSON.stringify(value)),
    get: (key) => { const item = localStorage.getItem(key); return item ? JSON.parse(item) : null; },
    remove: (key) => localStorage.removeItem(key),
};

// Format currency (Persian)
function formatCurrency(amount) {
    return new Intl.NumberFormat('fa-IR').format(amount);
}

// Blazor-callable localStorage helpers
window.lsGet = (key) => localStorage.getItem(key);
window.lsSet = (key, value) => localStorage.setItem(key, value);
window.lsRemove = (key) => localStorage.removeItem(key);

// Save user to localStorage then do a full browser redirect (bypasses Blazor DI scope issues)
window.loginUser = function(userJson, accessToken, refreshToken) {
    try {
        if (userJson) localStorage.setItem('avina_user', userJson);
        if (accessToken) localStorage.setItem('accessToken', accessToken);
        if (refreshToken) localStorage.setItem('refreshToken', refreshToken);
        console.log('[loginUser] Saved to localStorage, redirecting to /');
    } catch(e) {
        console.error('[loginUser] localStorage error:', e);
    }
    window.location.href = '/';
};

// Clear auth and redirect to home
window.logoutUser = function() {
    localStorage.removeItem('avina_user');
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    window.location.href = '/';
};

console.log('Avina loaded.');

// Debug: show localStorage state on every page load
(function() {
    const u = localStorage.getItem('avina_user');
    if (u) {
        try { console.log('[AUTH] User in localStorage:', JSON.parse(u)); }
        catch { console.log('[AUTH] Raw localStorage value:', u); }
    } else {
        console.log('[AUTH] No user in localStorage - please log in at /login');
    }
})();
