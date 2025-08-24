// Home Page Scripts

document.addEventListener('DOMContentLoaded', function() {
    initializeHomeScripts();
});

function initializeHomeScripts() {
    // Initialize all home page functionality
    setupScrollAnimations();
    setupNewsletterForm();
    setupSuccessDialogs();
    setupSmoothScrolling();
    setupLazyLoading();
    setupInteractiveElements();
}

// Scroll Animations
function setupScrollAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
            }
        });
    }, observerOptions);

    // Observe elements for animation
    const animateElements = document.querySelectorAll('.feature-card, .category-card, .section-header');
    animateElements.forEach(el => {
        observer.observe(el);
    });
}

// Newsletter Form
function setupNewsletterForm() {
    const newsletterForm = document.querySelector('.newsletter-form');
    if (!newsletterForm) return;

    newsletterForm.addEventListener('submit', function(e) {
        e.preventDefault();
        
        const emailInput = this.querySelector('input[type="email"]');
        const email = emailInput.value.trim();
        
        if (!isValidEmail(email)) {
            showAlert(getLocalizedText('Error.InvalidEmail'), 'error');
            return;
        }
        
        // Show loading state
        const submitButton = this.querySelector('button[type="submit"]');
        const originalText = submitButton.innerHTML;
        submitButton.innerHTML = '<i class="fas fa-spinner fa-spin"></i> ' + getLocalizedText('Home.Subscribe');
        submitButton.disabled = true;
        
        // Simulate API call
        setTimeout(() => {
            showAlert(getLocalizedText('Success.ThankYouForSubscribing'), 'success');
            emailInput.value = '';
            
            // Restore button
            submitButton.innerHTML = originalText;
            submitButton.disabled = false;
        }, 2000);
    });
}

function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Success Dialogs
function setupSuccessDialogs() {
    // Override existing dialog functions if they exist
    window.showBasketSuccess = function() {
        showSuccessDialog('dialog-basket');
    };
    
    window.showDownloadSuccess = function() {
        showSuccessDialog('downloaded-dialog');
    };
    
    window.showWishlistSuccess = function() {
        showSuccessDialog('dialog-wishlist');
    };
}

function showSuccessDialog(dialogId) {
    const dialog = document.getElementById(dialogId);
    if (!dialog) return;
    
    dialog.style.display = 'block';
    
    // Auto-hide after 3 seconds
    setTimeout(() => {
        dialog.style.display = 'none';
    }, 3000);
}

// Smooth Scrolling
function setupSmoothScrolling() {
    const links = document.querySelectorAll('a[href^="#"]');
    
    links.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            
            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);
            
            if (targetElement) {
                const offsetTop = targetElement.offsetTop - 80; // Account for fixed header
                
                window.scrollTo({
                    top: offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });
}

// Lazy Loading
function setupLazyLoading() {
    const images = document.querySelectorAll('img[data-src]');
    
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });
    
    images.forEach(img => imageObserver.observe(img));
}

// Interactive Elements
function setupInteractiveElements() {
    // Add hover effects to category cards
    const categoryCards = document.querySelectorAll('.category-card');
    categoryCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-10px) scale(1.02)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
    
    // Add click effects to buttons
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('click', function() {
            // Add ripple effect
            const ripple = document.createElement('span');
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = event.clientX - rect.left - size / 2;
            const y = event.clientY - rect.top - size / 2;
            
            ripple.style.width = ripple.style.height = size + 'px';
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            ripple.classList.add('ripple');
            
            this.appendChild(ripple);
            
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });
}

// Alert System
function showAlert(message, type = 'info') {
    // Remove existing alerts
    const existingAlerts = document.querySelectorAll('.alert');
    existingAlerts.forEach(alert => alert.remove());
    
    // Create alert element
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type === 'error' ? 'danger' : type} alert-dismissible fade show`;
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    // Insert at the top of the page
    const container = document.querySelector('.container') || document.body;
    container.insertBefore(alertDiv, container.firstChild);
    
    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.remove();
        }
    }, 5000);
}

// Parallax Effect for Hero Section
function setupParallaxEffect() {
    const heroSection = document.querySelector('.hero-section');
    if (!heroSection) return;
    
    window.addEventListener('scroll', function() {
        const scrolled = window.pageYOffset;
        const rate = scrolled * -0.5;
        
        heroSection.style.transform = `translateY(${rate}px)`;
    });
}

// Initialize parallax effect
setupParallaxEffect();

// Counter Animation
function animateCounters() {
    const counters = document.querySelectorAll('.counter');
    
    counters.forEach(counter => {
        const target = parseInt(counter.getAttribute('data-target'));
        const duration = 2000; // 2 seconds
        const increment = target / (duration / 16); // 60fps
        let current = 0;
        
        const updateCounter = () => {
            current += increment;
            if (current < target) {
                counter.textContent = Math.floor(current);
                requestAnimationFrame(updateCounter);
            } else {
                counter.textContent = target;
            }
        };
        
        updateCounter();
    });
}

// Initialize counter animation when elements come into view
const counterObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            animateCounters();
            counterObserver.unobserve(entry.target);
        }
    });
});

document.querySelectorAll('.counter-section').forEach(section => {
    counterObserver.observe(section);
});

// Enhanced Product Interactions
function setupProductInteractions() {
    // Add to cart functionality
    document.addEventListener('click', function(e) {
        if (e.target.classList.contains('add-to-cart')) {
            e.preventDefault();
            const productId = e.target.getAttribute('data-product-id');
            addToCart(productId);
        }
        
        if (e.target.classList.contains('add-to-wishlist')) {
            e.preventDefault();
            const productId = e.target.getAttribute('data-product-id');
            addToWishlist(productId);
        }
    });
}

function addToCart(productId) {
    // Simulate API call
    showLoadingState();
    
    setTimeout(() => {
        hideLoadingState();
        showBasketSuccess();
        updateCartCount();
    }, 1000);
}

function addToWishlist(productId) {
    // Simulate API call
    showLoadingState();
    
    setTimeout(() => {
        hideLoadingState();
        showWishlistSuccess();
    }, 1000);
}

function showLoadingState() {
    // Add loading overlay
    const overlay = document.createElement('div');
    overlay.className = 'loading-overlay';
    overlay.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
    document.body.appendChild(overlay);
}

function hideLoadingState() {
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) {
        overlay.remove();
    }
}

function updateCartCount() {
    // Update cart count in header
    const cartCount = document.querySelector('.cart-count');
    if (cartCount) {
        const currentCount = parseInt(cartCount.textContent) || 0;
        cartCount.textContent = currentCount + 1;
        cartCount.classList.add('pulse');
        
        setTimeout(() => {
            cartCount.classList.remove('pulse');
        }, 1000);
    }
}

// Initialize product interactions
setupProductInteractions();

// Localization helper function
function getLocalizedText(key) {
    // This function should be implemented to get localized text
    // For now, we'll return the key as fallback
    // In a real implementation, this would call the localization service
    const localizedTexts = {
        'Error.InvalidEmail': 'Please enter a valid email address',
        'Success.ThankYouForSubscribing': 'Thank you for subscribing! You\'ll receive updates soon.',
        'Home.Subscribe': 'Subscribe',
        'Success.ItemAddedToBasket': 'Item added to basket successfully!',
        'Success.ProductDownloaded': 'Product downloaded successfully!',
        'Success.ItemAddedToWishlist': 'Item added to wishlist successfully!'
    };
    
    return localizedTexts[key] || key;
}

// CSS for additional styles
const additionalStyles = `
    .animate-in {
        animation: fadeInUp 0.6s ease-out forwards;
    }
    
    .ripple {
        position: absolute;
        border-radius: 50%;
        background: rgba(255, 255, 255, 0.3);
        transform: scale(0);
        animation: ripple 0.6s linear;
        pointer-events: none;
    }
    
    @keyframes ripple {
        to {
            transform: scale(4);
            opacity: 0;
        }
    }
    
    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 9999;
        color: white;
        font-size: 2rem;
    }
    
    .cart-count.pulse {
        animation: pulse 1s ease-in-out;
    }
    
    @keyframes pulse {
        0% { transform: scale(1); }
        50% { transform: scale(1.2); }
        100% { transform: scale(1); }
    }
    
    .lazy {
        opacity: 0;
        transition: opacity 0.3s;
    }
    
    .lazy.loaded {
        opacity: 1;
    }
`;

// Inject additional styles
const styleSheet = document.createElement('style');
styleSheet.textContent = additionalStyles;
document.head.appendChild(styleSheet);

// Export functions for external use
window.HomeScripts = {
    showAlert,
    showBasketSuccess,
    showDownloadSuccess,
    showWishlistSuccess,
    addToCart,
    addToWishlist,
    getLocalizedText
};
