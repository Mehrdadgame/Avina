// Avina Application Scripts

// Initialize Bootstrap components
document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function(popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});

// Navigation smooth scroll
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function(e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({ behavior: 'smooth' });
        }
    });
});

// Add animation to elements on scroll
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -100px 0px'
};

const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('slide-in');
            observer.unobserve(entry.target);
        }
    });
}, observerOptions);

document.querySelectorAll('.course-card, .game-card, .product-card').forEach(card => {
    observer.observe(card);
});

// Handle search input
const searchInput = document.querySelector('input[type="text"][placeholder*="جستجو"]');
if (searchInput) {
    searchInput.addEventListener('input', function(e) {
        console.log('Search term:', e.target.value);
    });
}

// Handle cart operations
function addToCart(productId) {
    console.log('Product added to cart:', productId);
    // Show a toast or notification
    showNotification('محصول به سبد خریدتان اضافه شد', 'success');
}

function removeFromCart(itemId) {
    console.log('Item removed from cart:', itemId);
    showNotification('محصول از سبد خریدتان حذف شد', 'info');
}

// Notification system
function showNotification(message, type = 'info') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.setAttribute('role', 'alert');
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    // Add to body
    document.body.insertBefore(alertDiv, document.body.firstChild);
    
    // Auto-remove after 4 seconds
    setTimeout(() => {
        alertDiv.remove();
    }, 4000);
}

// Format currency
function formatCurrency(amount) {
    return new Intl.NumberFormat('fa-IR', {
        style: 'currency',
        currency: 'IRR'
    }).format(amount);
}

// Local storage utilities
const Storage = {
    set: function(key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },
    get: function(key) {
        const item = localStorage.getItem(key);
        return item ? JSON.parse(item) : null;
    },
    remove: function(key) {
        localStorage.removeItem(key);
    },
    clear: function() {
        localStorage.clear();
    }
};

// User preferences
const UserPrefs = {
    setTheme: function(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        Storage.set('theme', theme);
    },
    getTheme: function() {
        return Storage.get('theme') || 'light';
    },
    setLanguage: function(lang) {
        document.documentElement.lang = lang;
        document.documentElement.dir = lang === 'fa' ? 'rtl' : 'ltr';
        Storage.set('language', lang);
    },
    getLanguage: function() {
        return Storage.get('language') || 'fa';
    }
};

// Initialize user preferences
document.addEventListener('DOMContentLoaded', function() {
    const savedTheme = UserPrefs.getTheme();
    UserPrefs.setTheme(savedTheme);
});

// Debounce function for input events
function debounce(func, delay) {
    let timeoutId;
    return function(...args) {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => func(...args), delay);
    };
}

// Throttle function for scroll events
function throttle(func, limit) {
    let inThrottle;
    return function(...args) {
        if (!inThrottle) {
            func(...args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    };
}

// Log page performance
if (window.performance && window.performance.timing) {
    window.addEventListener('load', function() {
        const perf = window.performance.timing;
        const pageLoadTime = perf.loadEventEnd - perf.navigationStart;
        console.log('Page load time:', pageLoadTime + 'ms');
    });
}

console.log('Avina application loaded successfully!');
