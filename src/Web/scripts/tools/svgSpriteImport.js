let svgSprite = () =>
    new Promise(resolve => {
        let ajax = new XMLHttpRequest();

        ajax.open("GET", "/assets/sprite.svg", true);

        ajax.onload = evt => {

            // create hidden wrapper element at top of document, containing sprite
            let div = document.createElement("div");
            div.innerHTML = ajax.responseText;
            div.style.position = 'absolute';
            div.style.width = 0;
            div.style.height = 0;
            div.style.overflow = 'hidden';
            document.body.insertBefore(div, document.body.childNodes[0]);

            // replace all .js-svg elements with matching sprite element
            Array.prototype.forEach.call(document.querySelectorAll('.js-svg'), (el, i) => {

                // get sprite element name
                let name = el.getAttribute('data-name');
                let cssClass = el.className;
                let ariaHidden = el.getAttribute('aria-hidden');

                // If no match, return false
                if (!div.querySelector(`svg#${name}`)) return false;

                // create clone of matching element
                const clone = div.querySelector(`#${name}`).cloneNode(true);

                // add a prefix to the id to avoid duplicate id
                clone.id = `icon__${name}`;

                // add classes initially set
                clone.setAttribute('class', cssClass);

                // add aria-hidden attribute if necessary
                if (ariaHidden) {
                    clone.setAttribute('aria-hidden', ariaHidden)
                }

                // perform element replacement
                el.parentNode.replaceChild(clone, el)
            });

            resolve()
        };

        ajax.onerror = err => {
        };

        ajax.send()
    });

export default svgSprite
