import {defineStore} from 'pinia'
import {Guid, SessionEtude} from "@/types";
import {useSessionEtudeService} from "@/inversify.config";
import {useProgrammeStore} from "@/stores/programmeStore";

interface SessionEtudeState {
    _slugSessionEtudesParNumeroProgramme: Record<string, string[]>
    _sessionEtudesParSlug: Record<string, SessionEtude>
    _slugSessionEtudesParGuid: Record<string, string>
}

export const useSessionEtudeStore = defineStore('sessionEtudes', {
    state: (): SessionEtudeState => ({
        _slugSessionEtudesParNumeroProgramme: {} as Record<string, string[]>,
        _slugSessionEtudesParGuid: {} as Record<string, string>,
        _sessionEtudesParSlug: {} as Record<string, SessionEtude>
    }),

    actions: {
        addSessionEtudes(sessionEtudes: SessionEtude[]) {
            if (!sessionEtudes) return;
            sessionEtudes.forEach(sessionEtude =>
                Reflect.set(this._sessionEtudesParSlug, sessionEtude.slug!.trim().toUpperCase(), Object.freeze(sessionEtude)) &&
                Reflect.set(this._slugSessionEtudesParGuid, sessionEtude.id!.toString(), sessionEtude.slug!.trim().toUpperCase()));
        },
        addSessionEtudesPourProgramme(numeroProgramme: string, sessionEtudes: SessionEtude[]) {
            numeroProgramme = numeroProgramme.trim().toUpperCase();
            this.addSessionEtudes(sessionEtudes);

            if (this._slugSessionEtudesParNumeroProgramme[numeroProgramme] === undefined) {
                this._slugSessionEtudesParNumeroProgramme[numeroProgramme] = [];
            }

            this._slugSessionEtudesParNumeroProgramme[numeroProgramme] = [...new Set([
                ...sessionEtudes.map(s => s.slug!.trim().toUpperCase()),
                ...this._slugSessionEtudesParNumeroProgramme[numeroProgramme]
            ])];
        },
        getSessionEtudesPourProgramme(numeroProgramme: string): SessionEtude[] {
            return Reflect
                    .get(this._slugSessionEtudesParNumeroProgramme, numeroProgramme.trim().toUpperCase())
                    ?.map(slug => this.getSessionEtudeParSlug(slug)!)
                ?? [];
        },
        async rafraichirPourProgramme(numeroProgramme: string) {
            this.addSessionEtudesPourProgramme(numeroProgramme, await useSessionEtudeService()
                .getSessionEtudesForProgramme(useProgrammeStore().getProgramme(numeroProgramme)?.id?.toString() ?? "") ?? []);
        },
        getSessionEtudeParId(id: Guid): SessionEtude | null {
            if (!Reflect.has(this._slugSessionEtudesParGuid, id.toString())) {
                return null
            }
            const slug = this._slugSessionEtudesParGuid[id.toString()];
            return this.getSessionEtudeParSlug(slug);
        },
        getSessionEtudeParSlug(slug: string): SessionEtude | null {
            if (!Reflect.has(this._sessionEtudesParSlug, slug)) {
                return null
            }
            return this._sessionEtudesParSlug[slug];
        },
    },
    getters: {
        slugSessionEtudesParNumeroProgramme: (state: SessionEtudeState) => state._slugSessionEtudesParNumeroProgramme,
        sessionEtudesParSlug: (state: SessionEtudeState) => state._sessionEtudesParSlug,
        slugSessionEtudesParGuid: (state: SessionEtudeState) => state._slugSessionEtudesParGuid
    },
    persist: {
        storage: sessionStorage
    }
});