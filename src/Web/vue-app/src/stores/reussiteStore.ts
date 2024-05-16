import {defineStore} from 'pinia'
import {Guid} from "@/types";
import {useCoursAssisteService} from "@/inversify.config";
import {Note} from "@/types/enums/note";

interface ReussiteState {
    // [idProgramme][idSessionEtude][note]
    _reussiteParSessionDeProgramme: Record<string, Record<string, Record<Note, number>>>
    // [idProgramme][idSessionEtude][idCours][note]
    _reussiteParCoursPourProgrammeEtSession: Record<string, Record<string, Record<string, Record<Note, number>>>>
    // [idProgramme][idSessionEtude][idCours][note]
    _reussitePourCoursDeSessionDeProgramme: Record<string, Record<string, Record<string, Record<Note, number>>>>
}

export const useReussiteStore = defineStore('reussite', {
    state: (): ReussiteState => ({
        _reussiteParSessionDeProgramme: {} as Record<string, Record<string, Record<Note, number>>>,
        _reussiteParCoursPourProgrammeEtSession: {} as Record<string, Record<string, Record<string, Record<Note, number>>>>,
        _reussitePourCoursDeSessionDeProgramme: {} as Record<string, Record<string, Record<string, Record<Note, number>>>>
    }),

    actions: {
        async refreshReussiteParSessionPourProgramme(idProgramme: Guid) {
            this._reussiteParSessionDeProgramme[idProgramme.toString()] = await useCoursAssisteService().getReussiteParSessionPourProgramme(idProgramme);
        },
        getReussiteParSessionPourProgramme(idProgramme: Guid) {
            if (!Reflect.has(this._reussiteParSessionDeProgramme, idProgramme.toString())) {
                this._reussiteParSessionDeProgramme[idProgramme.toString()] = {};
                void this.refreshReussiteParSessionPourProgramme(idProgramme);
            }
            return this._reussiteParSessionDeProgramme[idProgramme.toString()];
        },
        async refreshReussiteParCoursPourProgrammeEtSession(idProgramme: Guid, idSessionEtude: Guid) {
            if (!Reflect.has(this._reussiteParCoursPourProgrammeEtSession, idProgramme.toString()))
                this._reussiteParCoursPourProgrammeEtSession[idProgramme.toString()] = {};
            this._reussiteParCoursPourProgrammeEtSession[idProgramme.toString()][idSessionEtude.toString()] = await useCoursAssisteService().getReussiteParCoursPourProgrammeEtSession(idProgramme, idSessionEtude);
        },
        async refreshReussitePourCoursDeSessionDeProgramme(idProgramme: Guid, idSessionEtude: Guid, idCours: Guid) {
            if (!Reflect.has(this._reussitePourCoursDeSessionDeProgramme, idProgramme.toString()))
                this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()] = {};
            if (!Reflect.has(this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()], idSessionEtude.toString()))
                this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()][idSessionEtude.toString()] = {};
            this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()][idSessionEtude.toString()][idCours.toString()] = await useCoursAssisteService().getReussitePourCoursDeSessionDeProgramme(idProgramme, idSessionEtude, idCours);
        },
        getReussiteParCoursPourProgrammeEtSession(idProgramme: Guid, idSessionEtude: Guid) {
            if (!Reflect.has(this._reussiteParCoursPourProgrammeEtSession, idProgramme.toString()) || !Reflect.has(this._reussiteParCoursPourProgrammeEtSession[idProgramme.toString()], idSessionEtude.toString())) {
                if (!Reflect.has(this._reussiteParCoursPourProgrammeEtSession, idProgramme.toString()))
                    this._reussiteParCoursPourProgrammeEtSession[idProgramme.toString()] = {};
                this._reussiteParCoursPourProgrammeEtSession[idProgramme.toString()][idSessionEtude.toString()] = {};
                void this.refreshReussiteParCoursPourProgrammeEtSession(idProgramme, idSessionEtude);
            }
            return this._reussiteParCoursPourProgrammeEtSession[idProgramme.toString()][idSessionEtude.toString()];
        },
        getReussitePourCoursDeSessionDeProgramme(idProgramme: Guid, idSessionEtude: Guid, idCours: Guid) {
            if (!Reflect.has(this._reussitePourCoursDeSessionDeProgramme, idProgramme.toString()) || !Reflect.has(this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()], idSessionEtude.toString()) || !Reflect.has(this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()][idSessionEtude.toString()], idCours.toString())) {
                if (!Reflect.has(this._reussitePourCoursDeSessionDeProgramme, idProgramme.toString()))
                    this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()] = {};
                if (!Reflect.has(this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()], idSessionEtude.toString()))
                    this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()][idSessionEtude.toString()] = {};
                this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()][idSessionEtude.toString()][idCours.toString()] = {} as Record<Note, number>;
                void this.refreshReussiteParCoursPourProgrammeEtSession(idProgramme, idSessionEtude);
            }
            return this._reussitePourCoursDeSessionDeProgramme[idProgramme.toString()][idSessionEtude.toString()][idCours.toString()];
        }
    },
    getters: {
        reussiteParSessionDeProgramme: (state: ReussiteState) => state._reussiteParSessionDeProgramme,
        reussiteParCoursPourProgrammeEtSession: (state: ReussiteState) => state._reussiteParCoursPourProgrammeEtSession,
        reussitePourCoursDeSessionDeProgramme: (state: ReussiteState) => state._reussitePourCoursDeSessionDeProgramme,
    },
    persist: {
        storage: sessionStorage
    }
});