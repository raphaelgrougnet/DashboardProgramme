<template>
  <div class="d-flex flex-column">
    <h1 class="mb-3">{{ t("correlations.titre") }}</h1>
    <form v-if="critere != 'TourAdmission' && critere != 'EtudiantInternational'" class="mb-3">
      <FormSelect
          v-model="critereSelectionne"
          :label="t('correlations.critere')"
          :options="criteres"
          :value="'SN4'"
          name="critere"
          @change="handleSubmit"
      />
    </form>
    <form v-else-if="critere != 'EtudiantInternational'" class=" m-0 d-flex flex-row">
      <FormSelect
          v-model="critereSelectionne"
          :label="t('correlations.critere')"
          :options="criteres"
          :value="'TourAdmission'"
          class="flex-grow-1"
          name="critere"
          @change="handleSubmit"
      />
      <FormSelect
          v-model="tourAdmissionSelectionneStr"
          :label="t('correlations.tour-admission')"
          :options="toursAdmission"
          :value="'1'"
          class="flex-grow-1"
          name="tourAdmission"
          @change="handleTourAdmissionChange"
      />
    </form>
    <form v-else class=" m-0 d-flex flex-row">
      <FormSelect
          v-model="critereSelectionne"
          :label="t('correlations.critere')"
          :options="criteres"
          :value="'EtudiantInternational'"
          class="flex-grow-1"
          name="critere"
          @change="handleSubmit"
      />
      <FormSelect
          v-model="etudiantInternationalSelectionne"
          :label="t('correlations.statut-international')"
          :options="etudiantInternationalOptions"
          :value="'CitoyenCanadien'"
          class="flex-grow-1"
          name="etudiantInternationalOpt"
          @change="handleEtudiantInternationalChange"
      />
    </form>

    <p class="fs-2 fw-bold mb-2">{{ t('correlations.valeur-p') }} {{ pValue }}*</p>

    <p v-if="pValue===0" class="m-0 fst-italic">{{ t('correlations.chargement') }}</p>
    <p v-if="pValue !== 0" class="m-0 fst-italic">*{{ t('correlations.valeur-reflete-prefix') }}<span
        class="fw-bold">{{ phrase }}</span>{{ t('correlations.valeur-reflete-suffix') }}</p>
    <p v-if="critere == 'GENMELS'" class="m-0 fst-italic">{{ t('correlations.GENMELS') }}</p>

  </div>
</template>

<script lang="ts" setup>

import FormSelect from "@/components/forms/FormSelect.vue";
import {useI18n} from "vue3-i18n";
import {computed, defineProps, onMounted, ref} from "vue";
import {useCorrelationsStore} from "@/stores/correlationsStore";

const inputValidationStatuses: any = {};
const formInputs = ref<any[]>([]);
const {t} = useI18n();
const correlationsStore = useCorrelationsStore();
const props = defineProps<{
  coursActuel: string;
}>();

const pValue = computed(() => correlationsStore.valeurP);
const critere = computed(() => correlationsStore.critereSelectionne);
const programme = computed(() => correlationsStore.coursSelectionne);
const tourAdmission = computed(() => correlationsStore.tourAdmission);
const etudiantInternational = computed(() => correlationsStore.etudiantInternational);

let critereSelectionne = 'SN4';
let tourAdmissionSelectionneStr = '1';
let tourAdmissionSelectionne = computed(() => parseInt(tourAdmissionSelectionneStr));
let etudiantInternationalSelectionne = 'CitoyenCanadien';

const critereTourAdmission = "TourAdmission";
const critereGENMELS = "GENMELS";
const critereEtudiantInternational = "EtudiantInternational";

let phrase = computed(() => {
  if (pValue.value <= 0.01) {
    return t('correlations.really-strong-influence');
  } else if (pValue.value <= 0.05) {
    return t('correlations.strong-influence');
  } else if (pValue.value <= 0.1) {
    return t('correlations.moderate-influence');
  } else {
    return t('correlations.weak-influence');
  }
});

correlationsStore.setProgrammeSelectionne(props.coursActuel);
correlationsStore.setCritereSelectionne(critereSelectionne);

const criteres = [
  {name: 'SN4', label: t('correlations.criteres.SN4')},
  {name: 'TS_SN4', label: t('correlations.criteres.TS_SN4')},
  //{name: 'TS4_SN4%2B', label: t('correlations.criteres.TS4_SN4+')},
  {name: 'CST5', label: t('correlations.criteres.CST5')},
  {name: 'TS_SN5', label: t('correlations.criteres.TS_SN5')},
  {name: 'TS5', label: t('correlations.criteres.TS5')},
  {name: '436', label: t('correlations.criteres.436')},
  {name: '514', label: t('correlations.criteres.514')},
  //{name: '514%2B', label: t('correlations.criteres.514+')},
  {name: '526', label: t('correlations.criteres.526')},
  //{name: '526%2B', label: t('correlations.criteres.526+')},
  {name: '536', label: t('correlations.criteres.536')},
  {name: 'GENMELS', label: t('correlations.criteres.GENMELS')},
  {name: 'TourAdmission', label: t('correlations.criteres.tour-admission')},
  {name: 'EtudiantInternational', label: t('correlations.criteres.etudiant-international')}
];

const toursAdmission = [
  {name: '1', label: '1'},
  {name: '2', label: '2'},
  {name: '3', label: '3'},
  {name: '4', label: '4'}
];

const etudiantInternationalOptions = [
  {name: 'CitoyenCanadien', label: 'Citoyen Canadien'},
  {name: 'ResidentPermanent', label: 'Résident Permanent'},
  {name: 'ResidentTemporaire', label: 'Résident Temporaire'},
];

function handleSubmit() {
  if (critereSelectionne === critereTourAdmission) {
    handleTourAdmissionChange();
    return;
  } else if (critereSelectionne === critereEtudiantInternational) {
    handleEtudiantInternationalChange();
    return;
  }
  correlationsStore.setCritereSelectionne(critereSelectionne);
  if (critereSelectionne === critereGENMELS) {
    correlationsStore.getValeurPGENMELS();
    return;
  }
  correlationsStore.getValeurP();
}

function handleTourAdmissionChange() {
  correlationsStore.setCritereSelectionne(critereSelectionne);
  correlationsStore.setTourAdmission(tourAdmissionSelectionne.value);
  correlationsStore.getValeurPTourAdmission();
}

function handleEtudiantInternationalChange() {
  correlationsStore.setCritereSelectionne(critereSelectionne);
  correlationsStore.setEtudiantInternational(etudiantInternationalSelectionne);
  correlationsStore.getValeurPInternational();
}

onMounted(() => {
  correlationsStore.setCritereSelectionne(critereSelectionne);
  correlationsStore.setProgrammeSelectionne(props.coursActuel);
  correlationsStore.setTourAdmission(tourAdmissionSelectionne.value);
  correlationsStore.getValeurP();

});
</script>

<style lang="scss" scoped>

</style>