import {notify} from "@kyvg/vue3-notification";

export function notifySuccess(text: string) {
    notify({
        text,
        type: "success",
    })
}

export function notifyError(text: string) {
    notify({
        text,
        type: "error",
    })
}