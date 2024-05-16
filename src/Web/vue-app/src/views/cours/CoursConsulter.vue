<template>
  <div v-if="isLoaded" class="content-grid">
    <BackLinkTitle :title="titre"/>
    <div v-if="reussite" class="row row-cols-2 gap-3 mt-2 mb-4">
      <div class="col-5"></div>
      <div v-if="labelsSessions.length > 1" class="col-5">
        <FormDoubleSlider :label="true"
                          :labels="labelsSessions"
                          :maxCaption="sliderTexteMax"
                          :maxValue="sliderValeurMax"
                          :minCaption="sliderTexteMin"
                          :minValue="sliderValeurMin"
                          class="mt-4 mb-0"
                          @input="updateSlider"
                          @deposer-curseur="modifierURL">
        </FormDoubleSlider>
      </div>
    </div>
    <div v-if="reussite" class="row row-cols-2 gap-3">
      <Histogramme :categories="categories" :format_tooltip_y="format_tooltip_y" :format_valeur_x="format_valeur_x"
                   :format_valeur_y="format_valeur_y" :series="series"
                   :titre="t('reussite.parSessionTitre', {session: slugSession})" :titre_x="t('reussite.note')"
                   :titre_y="t('reussite.nbEtudiants')"
                   class="col-5">
      </Histogramme>
      <div class="col-5">
        <Histogramme v-if="labelsSessions.length > 1"
                     :categories="categories"
                     :format_tooltip_y="format_tooltip_y"
                     :format_valeur_x="format_valeur_x"
                     :format_valeur_y="format_valeur_y"
                     :series="seriesComparaison"
                     :titre="t('reussite.parSessionTitre', {session: titreComparaison})"
                     :titre_x="t('reussite.note')"
                     :titre_y="t('reussite.nbEtudiants')">
        </Histogramme>
        <Card v-else class="h-100 bg-transparent align-content-center">
          <h1 class="mx-auto fs-3 fw-bold" v-text="aucuneSession"></h1>
        </Card>
      </div>

    </div>
    <div v-else class="row row-cols-2 gap-3">
      <div class="bg-light text-bg-info p-4 rounded border border-info">
        <h2 class="fs-4 mb-3">{{ t('programme.:id.session.:id.cours.:id.vierge.titre') }}</h2>
        <p>{{ t('programme.:id.session.:id.cours.:id.vierge.message') }}</p>
      </div>
    </div>

    <Card>
      <ValeurP
          :coursActuel="cours.id"
          class="mb-5"
      />
      <EchelleP/>
    </Card>

  </div>
  <div v-else>
    <Loader/>
  </div>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";
import {useRoute, useRouter} from "vue-router";
import {Cours, Guid, Programme, SessionEtude} from "@/types";
import {useCoursAssisteService, useCoursService} from "@/inversify.config";
import {useCoursStore} from "@/stores/coursStore";
import {useSessionEtudeStore} from "@/stores/sessionEtudeStore";
import {useProgrammeStore} from "@/stores/programmeStore";
import {useReussiteStore} from "@/stores/reussiteStore";
import {computed, ComputedRef, onMounted, Ref, ref, watch} from "vue";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import {Note} from "@/types/enums/note";
import Histogramme from "@/components/diagrammes/Histogramme.vue";
import FormDoubleSlider from "@/components/forms/FormDoubleSlider.vue";
import ValeurP from "@/components/correlations/ValeurP.vue";
import Loader from "@/components/layout/Loader.vue";
import {Saison} from "@/types/enums/Saison";
import Card from "@/components/layout/Card.vue";
import EchelleP from "@/assets/images/EchelleValeurP.svg";

const coursService = useCoursService();
const coursAssisteService = useCoursAssisteService();
const sessionEtudeStore = useSessionEtudeStore();
const programmeStore = useProgrammeStore();
const coursStore = useCoursStore();
const reussiteStore = useReussiteStore();

const {t} = useI18n();

const route = useRoute();
const router = useRouter();

const codeProgramme = route.params.programme.toString().trim().toUpperCase();
const slugSession = route.params.session.toString().trim().toUpperCase();
const numeroCours = route.params.cours.toString().trim().toUpperCase();

const programme = computed(() => programmeStore.programmes.find(p => p.numero == codeProgramme) as Programme);
const session = computed(() => sessionEtudeStore.sessionEtudesParSlug[slugSession] as SessionEtude);
const cours = computed(() => coursStore.coursParId[coursStore.coursIdParCode[numeroCours]] as Cours);
const isLoaded = computed(() => programme.value && session.value && cours.value);

const titre = computed(() =>
    `${programme.value.numero} – ${session.value.slug} — ${cours.value.code} – ${cours.value.nom}`);
const reussite = computed(() =>
    reussiteStore.reussitePourCoursDeSessionDeProgramme[programme.value.id!]?.[session.value.id!]?.[cours.value.id!]
    ?? {});

const sessions = Object.values(sessionEtudeStore.sessionEtudesParSlug);

const categories: Note[] = [Note.A, Note.B, Note.C, Note.D, Note.Echec,];

const format_valeur_y = (valeur: number) => valeur.toFixed(0);
const format_valeur_x = (valeur: string) => t("enums.Note." + valeur);
const format_tooltip_y = (valeur: number) => `${valeur} ${t("reussite.etudiantsNote")}`;

const series = computed(() => [
  {
    name: t("reussite.note"),
    data: categories.map(cat => reussite.value[cat] || 0),
    color: "#4299C2",
  },
]);

function comparerAnneeEtSaison(a: SessionEtude, b: SessionEtude) {
  if (a.annee! === b.annee!) {
    return -a.saison!.toString().localeCompare(b.saison!.toString());
  }

  return a.annee! - b.annee!;
}

const labelsSessions = computed(() =>
    Object.values(sessionEtudeStore.sessionEtudesParSlug)
        .filter(s => s.saison != Saison.Ete)
        .slice(0, 10)
        .sort((a, b) => comparerAnneeEtSaison(a, b))
        .map(s => s.slug)
);

// Filtrer entre 2 sessions, sur le 2e graphique

const aucuneSession = t('reussite.aucuneSessionComparaison');

let reussiteComparaison: Ref<Record<Note, number>> = ref({} as Record<Note, number>);

let _sessionDebut: SessionEtude | null = null;
let _sessionFin: SessionEtude | null = null;

let comparerEntre: Ref<string | undefined> = ref(undefined);
let slugsPourComparaison: Ref<string[] | undefined> = ref(undefined);

// Les handles du slider sélectionne dynamiquement 35% et 65% de la longueur du slider
const sliderValeurMin: Ref<number> = ref(Math.floor((labelsSessions.value.length - 1) * 0.35));
const sliderValeurMax: Ref<number> = ref(Math.ceil((labelsSessions.value.length - 1) * 0.65));
let sliderTexteMin = computed(() => labelsSessions.value[sliderValeurMin.value]);
let sliderTexteMax = computed(() => labelsSessions.value[sliderValeurMax.value]);

let titreComparaison: ComputedRef<string | undefined> = computed(() => `${sliderTexteMin.value}-${sliderTexteMax.value}`);

const seriesComparaison = computed(() => [
  {
    name: t("reussite.note"),
    data: categories.map(cat => Math.round(reussiteComparaison.value[cat] || 0)),
    color: "#4299C2",
  },
]);

function updateSlider(value: { minValue: number, maxValue: number }) {
  sliderValeurMin.value = value.minValue;
  sliderValeurMax.value = value.maxValue;
}

function modifierURL(slugSessions: string[]) {
  const paramURL = slugSessions.join("-");
  router.replace({query: {"comparerEntre": paramURL}});
}

async function afficherComparaison(comparerEntre: string | undefined) {

  if (!comparerEntre) {
    comparerEntre = `${sliderTexteMin.value}-${sliderTexteMax.value}`
  }

  slugsPourComparaison.value = comparerEntre.split("-");

  if (!slugsPourComparaison.value || slugsPourComparaison.value.length < 2) {
    console.error(t('reussite.erreurComparaison'));
    router.replace({query: {}});
    return;
  }

  _sessionDebut = sessionEtudeStore.getSessionEtudeParSlug(slugsPourComparaison.value[0]);
  _sessionFin = sessionEtudeStore.getSessionEtudeParSlug(slugsPourComparaison.value[1]);

  if (_sessionDebut == null || _sessionFin == null) {
    router.replace({query: {}});
    return;
  }

  sliderValeurMin.value = labelsSessions.value.indexOf(slugsPourComparaison.value[0]);
  sliderValeurMax.value = labelsSessions.value.indexOf(slugsPourComparaison.value[1]);

  reussiteComparaison.value = await coursAssisteService.getReussitePourCoursEntre2Sessions(programme.value.id!, cours.value.id!, _sessionDebut.id!, _sessionFin.id!);
}

watch(
    () => route.query.comparerEntre?.toString().trim().toUpperCase() ?? undefined,
    async _comparerEntre => await afficherComparaison(_comparerEntre),
    {immediate: true}
);

onMounted(programmeStore.rafraichirDepuisApi);
programmeStore.$subscribe(() =>
    sessionEtudeStore.rafraichirPourProgramme(programme.value.numero!));
sessionEtudeStore.$subscribe(() =>
    coursStore.rafraichirPourProgrammeEtSession(new Guid(programme.value?.id?.toString() ?? ""),
        new Guid(session.value?.id?.toString() ?? "")));
coursStore.$subscribe(() =>
    reussiteStore.refreshReussitePourCoursDeSessionDeProgramme(new Guid(programme.value?.id?.toString() ?? ""),
        new Guid(session.value?.id?.toString() ?? ""), new Guid(cours.value?.id?.toString() ?? "")));
</script>

<style>
</style>