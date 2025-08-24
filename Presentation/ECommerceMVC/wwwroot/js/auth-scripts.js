// Authentication Scripts

document.addEventListener('DOMContentLoaded', function() {
    initializeAuthScripts();
});

function initializeAuthScripts() {
    // Password toggle functionality
    setupPasswordToggles();
    
    // Password strength indicator
    setupPasswordStrength();
    
    // Form validation
    setupFormValidation();
    
    // Forgot password functionality
    setupForgotPassword();
    
    // Add animations
    addAnimations();
}

// Password Toggle Functionality
function setupPasswordToggles() {
    const passwordToggles = document.querySelectorAll('.password-toggle');
    
    passwordToggles.forEach(toggle => {
        toggle.addEventListener('click', function() {
            const input = this.previousElementSibling;
            const icon = this.querySelector('i');
            
            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('fa-eye');
                icon.classList.add('fa-eye-slash');
            } else {
                input.type = 'password';
                icon.classList.remove('fa-eye-slash');
                icon.classList.add('fa-eye');
            }
        });
    });
}

// Password Strength Indicator
function setupPasswordStrength() {
    const passwordInput = document.getElementById('register-password');
    if (!passwordInput) return;
    
    const strengthFill = document.getElementById('strength-fill');
    const strengthText = document.getElementById('strength-text');
    
    passwordInput.addEventListener('input', function() {
        const password = this.value;
        const strength = calculatePasswordStrength(password);
        updatePasswordStrengthIndicator(strength, strengthFill, strengthText);
    });
}

function calculatePasswordStrength(password) {
    let score = 0;
    let feedback = [];
    
    // Length check
    if (password.length >= 8) {
        score += 1;
    } else {
        feedback.push('At least 8 characters');
    }
    
    // Lowercase check
    if (/[a-z]/.test(password)) {
        score += 1;
    } else {
        feedback.push('Include lowercase letters');
    }
    
    // Uppercase check
    if (/[A-Z]/.test(password)) {
        score += 1;
    } else {
        feedback.push('Include uppercase letters');
    }
    
    // Numbers check
    if (/\d/.test(password)) {
        score += 1;
    } else {
        feedback.push('Include numbers');
    }
    
    // Special characters check
    if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
        score += 1;
    } else {
        feedback.push('Include special characters');
    }
    
    return {
        score: score,
        feedback: feedback,
        strength: score < 2 ? 'weak' : score < 4 ? 'medium' : 'strong'
    };
}

function updatePasswordStrengthIndicator(strength, strengthFill, strengthText) {
    // Remove existing classes
    strengthFill.classList.remove('weak', 'medium', 'strong');
    
    // Add new class
    strengthFill.classList.add(strength.strength);
    
    // Update text based on localization
    const strengthLabels = {
        weak: getLocalizedText('Password.WeakPassword'),
        medium: getLocalizedText('Password.MediumStrength'),
        strong: getLocalizedText('Password.StrongPassword')
    };
    
    strengthText.textContent = strengthLabels[strength.strength];
    strengthText.className = 'strength-text ' + strength.strength;
}

// Form Validation
function setupFormValidation() {
    const forms = document.querySelectorAll('.login-form, .register-form');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!validateForm(this)) {
                e.preventDefault();
                showFormErrors(this);
            }
        });
        
        // Real-time validation
        const inputs = form.querySelectorAll('input[required]');
        inputs.forEach(input => {
            input.addEventListener('blur', function() {
                validateField(this);
            });
            
            input.addEventListener('input', function() {
                clearFieldError(this);
            });
        });
    });
    
    // Registration form specific validation
    const registerForm = document.getElementById('register-form');
    if (registerForm) {
        setupRegistrationValidation(registerForm);
    }
}

function validateForm(form) {
    let isValid = true;
    const inputs = form.querySelectorAll('input[required]');
    
    inputs.forEach(input => {
        if (!validateField(input)) {
            isValid = false;
        }
    });
    
    // Check password confirmation
    const passwordInput = form.querySelector('#register-password');
    const confirmPasswordInput = form.querySelector('#confirmPassword');
    
    if (passwordInput && confirmPasswordInput) {
        if (passwordInput.value !== confirmPasswordInput.value) {
            showFieldError(confirmPasswordInput, getLocalizedText('Error.PasswordsDoNotMatch'));
            isValid = false;
        }
    }
    
    // Check terms agreement
    const termsCheckbox = form.querySelector('#agreeTerms');
    if (termsCheckbox && !termsCheckbox.checked) {
        showFieldError(termsCheckbox, getLocalizedText('Error.AgreeToTerms'));
        isValid = false;
    }
    
    return isValid;
}

function validateField(field) {
    const value = field.value.trim();
    let isValid = true;
    let errorMessage = '';
    
    // Required field validation
    if (field.hasAttribute('required') && !value) {
        isValid = false;
        errorMessage = getLocalizedText('Error.FieldRequired');
    }
    
    // Email validation
    if (field.type === 'email' && value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            isValid = false;
            errorMessage = getLocalizedText('Error.InvalidEmail');
        }
    }
    
    // Password validation
    if (field.id === 'register-password' && value) {
        if (value.length < 6) {
            isValid = false;
            errorMessage = getLocalizedText('Error.PasswordTooShort');
        }
    }
    
    // Name validation
    if (field.name === 'NameSurname' && value) {
        if (value.length < 2) {
            isValid = false;
            errorMessage = getLocalizedText('Error.NameTooShort');
        }
    }
    
    if (!isValid) {
        showFieldError(field, errorMessage);
    } else {
        clearFieldError(field);
    }
    
    return isValid;
}

function showFieldError(field, message) {
    field.classList.add('is-invalid');
    
    // Remove existing error message
    let errorElement = field.parentNode.querySelector('.validation-error');
    if (!errorElement) {
        errorElement = document.createElement('span');
        errorElement.className = 'validation-error';
        field.parentNode.appendChild(errorElement);
    }
    
    errorElement.textContent = message;
}

function clearFieldError(field) {
    field.classList.remove('is-invalid');
    
    const errorElement = field.parentNode.querySelector('.validation-error');
    if (errorElement) {
        errorElement.textContent = '';
    }
}

function showFormErrors(form) {
    const firstInvalidField = form.querySelector('.is-invalid');
    if (firstInvalidField) {
        firstInvalidField.focus();
    }
    
    // Show general error message
    showAlert(getLocalizedText('Error.PleaseCorrectErrors'), 'danger');
}

function setupRegistrationValidation(form) {
    const confirmPasswordInput = form.querySelector('#confirmPassword');
    const passwordInput = form.querySelector('#register-password');
    
    if (confirmPasswordInput && passwordInput) {
        confirmPasswordInput.addEventListener('input', function() {
            if (this.value && passwordInput.value !== this.value) {
                showFieldError(this, getLocalizedText('Error.PasswordsDoNotMatch'));
            } else {
                clearFieldError(this);
            }
        });
    }
}

// Forgot Password Functionality
function setupForgotPassword() {
    const sendResetButton = document.getElementById('sendResetEmail');
    if (!sendResetButton) return;
    
    sendResetButton.addEventListener('click', function() {
        const emailInput = document.getElementById('resetEmail');
        const email = emailInput.value.trim();
        
        if (!email) {
            showFieldError(emailInput, getLocalizedText('Error.FieldRequired'));
            return;
        }
        
        if (!isValidEmail(email)) {
            showFieldError(emailInput, getLocalizedText('Error.InvalidEmail'));
            return;
        }
        
        sendPasswordResetEmail(email, this);
    });
}

function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

function sendPasswordResetEmail(email, button) {
    // Show loading state
    const originalText = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i> ' + getLocalizedText('Auth.SendResetLink');
    button.disabled = true;
    
    const requestData = {
        Email: email
    };
    
    fetch('/Auth/ForgotPassword/', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
        },
        body: JSON.stringify(requestData)
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showAlert(getLocalizedText('Success.PasswordResetSent'), 'success');
            // Close modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('forgotPasswordModal'));
            if (modal) {
                modal.hide();
            }
        } else {
            showAlert(data.message || getLocalizedText('Error.FailedToSendResetEmail'), 'danger');
        }
    })
    .catch(error => {
        console.error('Error:', error);
        showAlert(getLocalizedText('Error.AnErrorOccurred'), 'danger');
    })
    .finally(() => {
        // Restore button state
        button.innerHTML = originalText;
        button.disabled = false;
    });
}

// Alert System
function showAlert(message, type = 'info') {
    // Remove existing alerts
    const existingAlerts = document.querySelectorAll('.alert');
    existingAlerts.forEach(alert => alert.remove());
    
    // Create new alert
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show`;
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    `;
    
    // Insert alert at the top of the form
    const form = document.querySelector('.login-form, .register-form');
    if (form) {
        form.parentNode.insertBefore(alertDiv, form);
    }
    
    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.remove();
        }
    }, 5000);
}

// Animations
function addAnimations() {
    // Add fade-in animation to main containers
    const containers = document.querySelectorAll('.login-container, .register-container');
    containers.forEach(container => {
        container.classList.add('fade-in');
    });
    
    // Add slide-in animation to cards
    const cards = document.querySelectorAll('.login-card, .register-card');
    cards.forEach(card => {
        card.classList.add('slide-in');
    });
    
    // Add hover effects to buttons
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-2px)';
        });
        
        button.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
        });
    });
}

// Utility Functions
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Enhanced form submission with loading states
function setupFormSubmission() {
    const forms = document.querySelectorAll('.login-form, .register-form');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitButton = this.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.classList.add('loading');
                submitButton.disabled = true;
            }
        });
    });
}

// Initialize form submission handlers
setupFormSubmission();

// Localization helper function
function getLocalizedText(key) {
    // This function should be implemented to get localized text
    // For now, we'll return the key as fallback
    // In a real implementation, this would call the localization service
    const localizedTexts = {
        'Password.WeakPassword': 'Weak password',
        'Password.MediumStrength': 'Medium strength',
        'Password.StrongPassword': 'Strong password',
        'Error.PasswordsDoNotMatch': 'Passwords do not match',
        'Error.AgreeToTerms': 'You must agree to the terms and conditions',
        'Error.FieldRequired': 'This field is required',
        'Error.InvalidEmail': 'Please enter a valid email address',
        'Error.PasswordTooShort': 'Password must be at least 6 characters long',
        'Error.NameTooShort': 'Name must be at least 2 characters long',
        'Error.FailedToSendResetEmail': 'Failed to send reset email',
        'Error.AnErrorOccurred': 'An error occurred while processing your request.',
        'Error.PleaseCorrectErrors': 'Please correct the errors in the form',
        'Success.PasswordResetSent': 'Password reset link has been sent to your email',
        'Auth.SendResetLink': 'Send Reset Link'
    };
    
    return localizedTexts[key] || key;
}

// Export functions for potential external use
window.AuthScripts = {
    showAlert,
    validateForm,
    calculatePasswordStrength,
    getLocalizedText
};
