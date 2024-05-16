import {defineStore} from 'pinia'
import {Programme} from "@/types";
import {useProgrammeService} from "@/inversify.config";

interface ProgrammeState {
    _programmes: Record<string, Programme>
}

export const useProgrammeStore = defineStore('programmes', {
    state: (): ProgrammeState => ({
        _programmes: {} as Record<string, Programme>
    }),

    actions: {
        addProgrammes(programmes: Programme[]) {
            if (!programmes) return;
            programmes.forEach(programme => Reflect.set(this._programmes, programme.numero!.trim().toUpperCase(), Object.freeze(programme)));
        },
        getProgramme(numero: string): Programme | null {
            return Reflect.get(this._programmes, numero.trim().toUpperCase()) ?? null;
        },
        async rafraichirDepuisApi() {
            this.addProgrammes(await useProgrammeService().getAllProgrammes() ?? []);
        }
    },

    getters: { // computed
        programmes: (state: ProgrammeState) => Object.values(state._programmes),
    },

    persist: {
        storage: sessionStorage
    }
});