import 'babel-polyfill'
import svgSprite from './tools/svgSpriteImport';
import cssVars from 'css-vars-ponyfill';
import {F as form} from './modules/form.js';

(() => {
    // DOM ready
    setTimeout(function () {
        svgSprite();
        cssVars();
    }, 50);

    // load event
    // wait until DOM is ready (html and svg markup)
    document.addEventListener("DOMContentLoaded", function () {
        // wait until window is loaded (images, external JS, external stylesheets, fonts, links, and other media assets)
        window.addEventListener("load", function () {
            // makes sure it runs after last render tick
            window.requestAnimationFrame(function () {
                form.init();
            });
        });
    });
})();
