import {defineStore} from 'pinia'
import {Cours, Etudiant, Guid} from "@/types";
import {useCoursService} from "@/inversify.config";

interface CoursState {
    _coursParId: Record<string, Cours>
    _coursIdParCode: Record<string, Guid>
    _coursIdPourSessionEtudeDeProgramme: Record<string, Record<string, Guid[]>>
    _etudiantsParCours: Record<string, Record<string, Record<string, Guid[]>>>
    _etudiantsParId: Record<string, Etudiant>; 
}

export const useCoursStore = defineStore('cours', {
    state: (): CoursState => ({
        _coursParId: {} as Record<string, Cours>,
        _coursIdPourSessionEtudeDeProgramme: {} as Record<string, Record<string, Guid[]>>,
        _coursIdParCode: {} as Record<string, Guid>,
        _etudiantsParCours: {} as Record<string, Record<string, Record<string, Guid[]>>>,
        _etudiantsParId: {} 
    }),

    actions: {
        addCours(cours: Cours) {
            this._coursParId[cours.id!.toString()] = cours;
            this._coursIdParCode[cours.code!.trim().toUpperCase()] = cours.id as Guid;
        },
        getCoursParId(id: Guid): Cours | null {
            if (!Reflect.has(this._coursParId, id.toString())) {
                return null;
            }
            return this._coursParId[id.toString()];
        },
        getCoursParCode(code: string): Cours | null {
            if (!Reflect.has(this._coursIdParCode, code)) {
                return null;
            }
            return this.getCoursParId(this._coursIdParCode[code]);
        },
        getEtudiantParId(id: Guid): Etudiant | null {
            if (!Reflect.has(this._etudiantsParId, id.toString())) {
                return null; 
            }
            return this._etudiantsParId[id.toString()];
        },
        async rafraichirPourProgrammeEtSession(idProgramme: Guid, idSession: Guid) {
            const lstCours = await useCoursService().getCoursForSessionOfProgramme(idProgramme, idSession) ?? [];

            if (!Reflect.has(this._coursIdPourSessionEtudeDeProgramme, idProgramme.toString()))
                this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()] = {};

            if (!Reflect.has(this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()], idSession.toString()))
                this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()][idSession.toString()] = [];

            lstCours.forEach(cours => {
                this.addCours(cours);
                this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()][idSession.toString()] = [...new Set([
                    ...this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()][idSession.toString()],
                    cours.id
                ])] as Guid[];
            });
        },
        getLstCoursPourProgrammeEtSession(idProgramme: Guid, idSession: Guid): Cours[] {
            if (!Reflect.has(this._coursIdPourSessionEtudeDeProgramme, idProgramme.toString()) || !Reflect.has(this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()], idSession.toString())) {
                void this.rafraichirPourProgrammeEtSession(idProgramme, idSession);
                return [];
            }
            const lstCours = this._coursIdPourSessionEtudeDeProgramme[idProgramme.toString()][idSession.toString()] ?? [];
            return lstCours.map(guid => this.getCoursParId(guid)!);
        },

        getEtudiantsForCours(idProgramme: Guid, idSession: Guid, idCours: Guid) : Cours[] {
            if (!Reflect.has(this._etudiantsParCours, idProgramme.toString()) ||
                !Reflect.has(this._etudiantsParCours[idProgramme.toString()], idSession.toString()) ||
                !Reflect.has(this._etudiantsParCours[idProgramme.toString()][idSession.toString()], idCours.toString())) {
                void this.rafraichirPourProgrammeEtSession(idProgramme, idSession); // You may need a different method to refresh students
                return [];
            }

            const studentGuids = this._etudiantsParCours[idProgramme.toString()][idSession.toString()][idCours.toString()] ?? [];
            return studentGuids.map((guid: Guid) => this.getEtudiantParId(guid)!); 
        }

    },
    getters: {
        coursParId: (state: CoursState) => state._coursParId,
        coursIdParCode: (state: CoursState) => state._coursIdParCode,
        coursIdPourSessionEtudeDeProgramme: (state: CoursState) => state._coursIdPourSessionEtudeDeProgramme
    },
    persist: {
        storage: sessionStorage
    }
});