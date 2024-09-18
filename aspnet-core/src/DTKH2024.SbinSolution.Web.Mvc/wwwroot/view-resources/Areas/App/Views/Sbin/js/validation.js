const $ = document.querySelector.bind(document);
const $$ = document.querySelectorAll.bind(document);

function Validator(options) {
    const getParent = (element , selector) => {
        while (element.parentElement) {
            if(element.parentElement.matches(selector)) {
                return element.parentElement;
            }
            element = element.parentElement;
        }
    }

    const selectorRules = {};

    const validate = (inputElement, rule) => {
        const errorElement = getParent(inputElement , options.formGroupSelector).querySelector(options.errorSelector);
        let errorMessage;
        const rules = selectorRules[rule.selector];
        
        for(let i = 0; i < rules.length; i++) {
            switch (inputElement.type) {
                case 'checkbox':
                case 'radio':
                    errorMessage = rules[i](
                        formElement.querySelector(rule.selector + ":checked")
                    );
                    break;
                default: 
                    errorMessage = rules[i](inputElement.value);
            }

            if(errorMessage) break;
        }

        if(errorMessage) {
            errorElement.textContent = errorMessage;
            getParent(errorElement , options.formGroupSelector).classList.add("invalid");
        } else {
            errorElement.textContent = "";
            getParent(errorElement , options.formGroupSelector).classList.remove("invalid");
        }
        
        inputElement.oninput = () => {
            errorElement.textContent = "";
            getParent(errorElement , options.formGroupSelector).classList.remove("invalid");
        }
        
        return !errorMessage;
    }

    const formElement = $(options.form);
    if(formElement) {
        formElement.onsubmit = (e) => {
            e.preventDefault();
            let isFormValid = true;

            options.rules.forEach((rule) => {
                const inputElements = formElement.querySelectorAll(rule.selector);
                Array.from(inputElements).forEach((inputElement) => {
                    const isValid = validate(inputElement, rule);
                    if(!isValid) { 
                        isFormValid = false;
                    }
                });
            });

            if(isFormValid) {  
                console.log("Không có lỗi");

                if (typeof options.onSubmit === "function") { 
                    const enableInput = formElement.querySelectorAll('[name]:not([disabled])');

                    const formValues = Array.from(enableInput).reduce((values, input) => {
                        switch(input.type) {
                            case 'radio':
                                values[input.name] = formElement.querySelector(`input[name="${input.name}"]:checked`).value;
                                break;
                            case 'checkbox':
                                const checkedCheckboxes = formElement.querySelectorAll(`input[name="${input.name}"]:checked`);
                                values[input.name] = Array.from(checkedCheckboxes).map(checkbox => checkbox.value);
                                break;
                            case 'file':
                                values[input.name] = input.files;
                                break;
                            default:
                                values[input.name] = input.value;
                        }
                        return values;
                    }, {});

                    options.onSubmit(formValues);
                } else {
                    formElement.submit();
                }
            } else {
                console.log("Có lỗi");
            }
        }

        options.rules.forEach((rule) => {
            if(Array.isArray(selectorRules[rule.selector])) {  
                selectorRules[rule.selector].push(rule.test);
            } else { 
                selectorRules[rule.selector] = [rule.test]; 
            }

            const inputElements = formElement.querySelectorAll(rule.selector);
            Array.from(inputElements).forEach((inputElement) => {
                inputElement.onblur = () => {
                    validate(inputElement, rule);
                }
            });
        });
    }
}

Validator.isRequired = (selector, message) => {
    return {
        selector, 
        test: (value) => {
            return value ? undefined : message || "Please enter this field."; 
        }
    };
}

Validator.isEmail = (selector, message) => {
    return {
        selector, 
        test: (value) => {
            const regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
            return regex.test(value) ? undefined : message || "This field is not Email";
        }
    };
}

Validator.isMinLength = (selector, min, message) => {
    return {
        selector, 
        test: (value) => {
            return value.length >= min ? undefined : message || `Password minimum ${min} characters`;
        }
    };
}

Validator.isConfirmed = (selector, getConfirmedValue, message) => {
    return {
        selector, 
        test: (value) => {
            return value === getConfirmedValue() ? undefined : message || "The entered value do not match";
        }
    };
}
