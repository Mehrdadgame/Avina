// Avina Application Scripts

// Header scroll shadow
window.addEventListener('scroll', () => {
    const topbar = document.getElementById('topbar');
    if (topbar) {
        topbar.classList.toggle('scrolled', window.scrollY > 10);
    }
});

// Local storage utilities
const Storage = {
    set: (key, value) => localStorage.setItem(key, JSON.stringify(value)),
    get: (key) => { const item = localStorage.getItem(key); return item ? JSON.parse(item) : null; },
    remove: (key) => localStorage.removeItem(key),
};

// Format currency (Persian)
function formatCurrency(amount) {
    return new Intl.NumberFormat('fa-IR').format(amount);
}

console.log('Avina loaded.');
