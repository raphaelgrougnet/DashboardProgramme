import {defineStore} from 'pinia'
import {usePortraitEtudiantService} from "@/inversify.config";
import {notifyError} from "@/notify";

interface PortraitEtudiantState {
    _idProgramme: string,
    _seriesData: { rt: number[], r18: number[], sa: number[] }
}

export const usePortraitEtudiantStore = defineStore('portraitEtudiant', {
    state: (): PortraitEtudiantState => ({
        _idProgramme: '',
        _seriesData: {
            rt: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            r18: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            sa: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        }
    }),

    actions: { // methods
        setIdProgramme(idProgramme: string) {
            this._idProgramme = idProgramme;
        },
        setSeriesData(seriesData: { rt: number[], r18: number[], sa: number[] }) {
            this._seriesData = seriesData;
        },
        async getSeriesData() {
            this._seriesData = {
                rt: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                r18: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                sa: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
            };
            await usePortraitEtudiantService().getSeriesData(this._idProgramme)
                .then(seriesData => {
                    this._seriesData = seriesData;
                })
                .catch(err => {
                        notifyError(`Erreur lors de la récupération des données pour le portrait étudiant: ${err.toString()}`);
                    }
                );
        }
    },

    getters: { // computed
        seriesData: (state) => state._seriesData
    },

    persist: {
        storage: sessionStorage
    }

});