import {defineStore} from 'pinia'
import {useCorrelationsService} from "@/inversify.config";
import {notifyError} from "@/notify";

interface CorrelationsState {
    _critereSelectionne: string,
    _coursSelectionne: string,
    _tourAdmission: number,
    _etudiantInternational: string,
    _valeurP: number
}

export const useCorrelationsStore = defineStore('correlations', {
    state: (): CorrelationsState => ({
        _critereSelectionne: '' as string,
        _coursSelectionne: '' as string,
        _tourAdmission: 0 as number,
        _etudiantInternational: '' as string,
        _valeurP: 0 as number
    }),

    actions: { // methods
        setCritereSelectionne(critere: string) {
            this._critereSelectionne = critere;
        },

        setProgrammeSelectionne(programme: string) {
            this._coursSelectionne = programme;
        },

        setTourAdmission(tour: number) {
            this._tourAdmission = tour;
        },

        setEtudiantInternational(choix: string) {
            this._etudiantInternational = choix;
        },

        setValeurP(valeurP: number) {
            this._valeurP = valeurP;
        },

        async getValeurP() {
            this.setValeurP(0);
            const valP = useCorrelationsService().getValeurP(this._critereSelectionne, this._coursSelectionne)
                .then(valeurP => {
                    const formattedNum = parseFloat(valeurP.toFixed(4));
                    this.setValeurP(formattedNum);
                })
                .catch(err => notifyError(`Erreur lors de la récupération de la valeur P: ${err.toString()}`)
                );

        },

        async getValeurPGENMELS() {
            this.setValeurP(0);
            const valP = useCorrelationsService().getValeurPGENMELS(this._coursSelectionne)
                .then(valeurP => {
                    const formattedNum = parseFloat(valeurP.toFixed(4));
                    this.setValeurP(formattedNum);
                })
                .catch(err => notifyError(`Erreur lors de la récupération de la valeur P: ${err.toString()}`)
                );

        },

        async getValeurPTourAdmission() {
            this.setValeurP(0);
            const valP = useCorrelationsService().getValeurPTourAdmission(this._coursSelectionne, this._tourAdmission)
                .then(valeurP => {
                    const formattedNum = parseFloat(valeurP.toFixed(4));
                    this.setValeurP(formattedNum);
                })
                .catch(err => notifyError(`Erreur lors de la récupération de la valeur P: ${err.toString()}`)
                );

        },

        async getValeurPInternational() {
            this.setValeurP(0);
            console.log(this._coursSelectionne, this._etudiantInternational);
            const valP = useCorrelationsService().getValeurPInternational(this._coursSelectionne, this._etudiantInternational)
                .then(valeurP => {
                    const formattedNum = parseFloat(valeurP.toFixed(4));
                    this.setValeurP(formattedNum);
                })
                .catch(err => notifyError(`Erreur lors de la récupération de la valeur P: ${err.toString()}`)
                );

        }

    },

    getters: { // computed
        coursSelectionne: state => state._coursSelectionne,
        critereSelectionne: state => state._critereSelectionne,
        tourAdmission: state => state._tourAdmission,
        etudiantInternational: state => state._etudiantInternational,
        valeurP: state => state._valeurP
    },

    persist: {
        storage: sessionStorage
    }

});