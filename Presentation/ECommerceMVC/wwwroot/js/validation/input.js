class Validation {
    static required = value => (!!value);

    static maxLength = max => value => !(value && value.length > max);

    static minLength = min => value => !(value && value.length < min);

    static number = value => !(value && isNaN(Number(value)));

    static minValue = min => value => isNaN(value) || value >= min;

    static maxValue = max => value => isNaN(value) || value <= max;

    static phoneNumber = value =>
        !(value && isNaN(Number(value)));

    static email = value => !(value && !/[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}/i.test(value));

    static compose = (...validators) => value => validators.reduce((error, validator) => error && validator(value), true)
}

const input_classes = {
    required: '.required-js',
    email: '.email-js',
    checkbox: 'checkbox-js'
};

$(document).ready(() => {
    $(input_classes.email).keyup(event => {
        inputEventValidator(event.target, Validation.compose(...[Validation.email, Validation.required]), 'Xanani düzgün doldurun');
    });

    $(input_classes.required).keyup(event => {
        inputEventValidator(event.target, Validation.required, 'Xanani doldurun');
    });

});

selector => {
    const required_fields = $(selector).find('input.required-js');
    required_fields.each((index, element) => {
        inputEventValidator(element, Validation.required);
    });
    const email_fields = $(selector).find('input.email-js');
    email_fields.each((index, element) => {
        inputEventValidator(element, Validation.compose(...[Validation.email, Validation.required]))
    });
    const checkbox_fields = $(selector).find("input[type='checkbox'].checkbox-js");
    checkbox_fields.each((index, element) => {
        let noneError = $(element).siblings('.error-message').length <= 0;
        if (!$(element).is(':checked')) {
            let error_msg = '';
            noneError && addErrorInfo(element, error_msg);
        } else {
            !noneError && removeErrorInfo(element);
        }
    });
    return $(selector).find('input.error-innput').length <= 0
}


// check functions
const inputEventValidator = (target, validator_callback, error_msg) => {
    const isValid = validator_callback(target.value);
    const noneError = $(target).siblings('.error-message').length <= 0;
    if (!isValid) {
        noneError &&
            addErrorInfo(target, error_msg)

    } else {
        !noneError && removeErrorInfo(target);
    }
};

const appendErrorDiv = (element, error_msg = "") => {
    if ($(element).find('.error-message').length <= 0) {
        const errorDiv = document.createElement('p');
        errorDiv.classList.add('error-message');
        errorDiv.innerText = error_msg;
        errorDiv.style.margin = '0';
        element.append(errorDiv);
    }
};

const addErrorInfo = (element, error_msg) => {
    appendErrorDiv(element.parentElement, error_msg);
    element.classList.add('error-input');
};

const removeErrorInfo = element => {
    element.classList.remove('error-input');
    const errorDiv = element.parentElement.querySelector('.error-message');
    element.parentElement.removeChild(errorDiv);
};