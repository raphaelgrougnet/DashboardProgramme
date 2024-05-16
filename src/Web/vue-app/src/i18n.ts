import {createI18n} from "vue3-i18n";
import {defaultLocale, messages} from "@/locales";

function getDefaultLocale(): string {
    return defaultLocale;
}

const i18n = createI18n({
    locale: getDefaultLocale(),
    messages
});

export default i18n;
