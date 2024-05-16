export const F = {
    forms: document.querySelectorAll(".js-form"),
    inputs: document.querySelectorAll(".js-input"),

    init: () => {
        if (F.forms != null && F.forms.length > 0) {
            F.forms.forEach(form => form.addEventListener("submit", F.validateForm.bind(F, form)));
        }

        if (F.inputs != null && F.inputs.length > 0) {
            F.inputs.forEach(input => {
                input.addEventListener("change", F.validateInput.bind(F, input));
                input.addEventListener("blur", F.validateInput.bind(F, input));
            });
        }
    },

    validateInput: input => {
        if (input != null) {
            const field = input.parentElement;
            const isValid = input.required === true ? input.value !== "" : true;

            if (!isValid) {
                input.setAttribute("aria-invalid", "true");
                field.classList.add("error");
            } else {
                input.setAttribute("aria-invalid", "false");
                field.classList.remove("error");
            }

            return isValid;
        }
    },

    validateForm: (form, e) => {
        e.preventDefault();

        if (form != null) {
            const inputs = form.querySelectorAll(".js-input");
            var isValid = true;

            if (inputs != null && inputs.length > 0) {
                inputs.forEach(input => {
                    if (!F.validateInput(input)) {
                        //if any input isnt valid, the form is also invalid.
                        isValid = false;
                    }
                })
            }

            if (isValid) {
                form.submit();
            }
        }
    }
};
