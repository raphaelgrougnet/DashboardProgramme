<template>
  <div v-if="isLoaded" class="content-grid">
    <BackLinkTitle :title="titre"/>

    <div v-if="lstCours.length > 0" class="row row-cols-2 gap-3">
      <div v-for="cours in lstCours" :key="cours.code" class="col fs-4 bg-slate-400 carte-programme">
        <div class="linkcard-container position-relative">
          <LinkCard :link-path="{name: 'programme:id.session:id.cours:id', params: {programme: numeroProgramme, session: slugSession, cours: cours.code}}"
                    :link-text="t('programme.:id.session.:id.afficherCours')"
                    :title="cours.code">
            <span v-if="showHistogram[cours.id]">
              <p>Nombre d'étudiants : {{ studentCounts[cours.id] }}</p>
              <Histogramme class="mt-4" titre_y="Nombre d'étudiants"
                           titre_x="Note"
                           titre="Moyenne générale de tous les étudiants du groupe"
                           :format_valeur_x="format_valeur_x"
                           :categories="categories" :series="genererSeriesParIdCours(cours.id!!.toString())">
              </Histogramme>
            </span>
          </LinkCard> 
          <button class="toggle-button mt-2" @click="toggleHistogram(cours.id!!)"><i :class="'bi ' + (showHistogram[cours.id] ? 'bi-dash-circle-dotted' : 'bi-plus-circle') "></i></button>
        </div>
      </div>
    </div>
    <div v-else class="row row-cols-2 gap-3">
      <div class="bg-light text-bg-info p-4 rounded border border-info">
        <h2 class="fs-4 mb-3">{{ t('programme.:id.session.:id.vierge.titre') }}</h2>
        <p>{{ t('programme.:id.session.:id.vierge.message') }}</p>
      </div>
    </div>
  </div>
  <div v-else>
    <Loader/>
  </div>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";
import {useRoute, useRouter} from "vue-router";
import {useProgrammeStore} from "@/stores/programmeStore";
import {Cours, Guid, Programme, SessionEtude} from "@/types";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import {computed, ComputedRef, onMounted, reactive, ref} from "vue";
import {useSessionEtudeStore} from "@/stores/sessionEtudeStore";
import {useEtudiantService, useSessionEtudeService} from "@/inversify.config";
import {useReussiteStore} from "@/stores/reussiteStore";
import {Note} from "@/types/enums/note";
import {useCoursStore} from "@/stores/coursStore";
import LinkCard from "@/components/layout/LinkCard.vue";
import Loader from "@/components/layout/Loader.vue";
import Histogramme from "@/components/diagrammes/Histogramme.vue";

const reussiteStore = useReussiteStore();
const coursStore = useCoursStore();
const route = useRoute();
const router = useRouter();
const {t} = useI18n();
const sessionEtudeStore = useSessionEtudeStore();
const programmeStore = useProgrammeStore();
const sessionEtudeService = useSessionEtudeService();
const etudiantsService = useEtudiantService();

const categories: Note[] = [ Note.A, Note.B, Note.C, Note.D, Note.Echec, ];

const codeProgramme = route.params.programme.toString().trim().toUpperCase();
const slugSession = route.params.session.toString().trim().toUpperCase();

const programme = computed(() => programmeStore.programmes.find(p => p.numero == codeProgramme) as Programme);
const session = computed(() => sessionEtudeStore.sessionEtudesParSlug[slugSession] as SessionEtude);

const idSession = computed(() => session.value?.id as Guid);
const numeroProgramme = computed(() => programme.value?.numero as string);
const idProgramme = computed(() => programme.value?.id as Guid);
const titre = computed(() => `${numeroProgramme.value} – ${programme.value?.nom} - ${t(`enums.Saison.${session.value?.saison}`)} ${session.value?.annee}`);

const reussite: ComputedRef<Record<string, Record<Note, number>>> = computed(() =>
    idProgramme.value && idSession.value ?
        reussiteStore.reussiteParCoursPourProgrammeEtSession[idProgramme.value]?.[idSession.value] ?? {}: {});
const isLoaded = computed(() => session.value && Reflect.ownKeys(reussite.value).length);

const lstCoursIds = computed(() => Reflect.ownKeys(reussite.value));
const lstCours = computed(() => Object.entries(coursStore.coursParId).filter(([id, _]) =>
    lstCoursIds.value.includes(id)).map(([_, c]) => c) as Cours[]);

const isDataLoaded = ref(false);

const format_valeur_x = (valeur: string) => t("enums.Note." + valeur);

async function loadClassStudentCount(coursId: Guid) {
  try {
    studentCounts[coursId.toString()] = await calculNbEtudiants(coursId);
  } catch (error) {
    console.error('Error loading student count for class:', error);
  }
}

async function loadClassGrades(coursId: Guid) {
  try {
    studentGradesCount[coursId.toString()] = await calculMoyenneNoteEtudiants(coursId);
  } catch (error) {
    console.error('Error loading grades for class:', error);
  }
}

async function calculNbEtudiants(idCours: Guid | undefined): Promise<number> {
  return etudiantsService.getStudentsForCours(idSession.value, idCours!, idProgramme.value);
}
async function calculMoyenneNoteEtudiants(idCours:Guid| undefined): Promise<Record<Note, number>>{
  return etudiantsService.getAverageGradesForStudentsInClass(idSession.value,idCours!,idProgramme.value);
}

function genererSeriesParIdCours(idCours: string){
  return [{
    name: "Nombre d'étudiant(s) avec cette note",
    data: Object.values(studentGradesCount[idCours]),
    color: "#4299C2",
  }];
}

async function toggleHistogram(coursId: Guid) {
  if (!showHistogram[coursId.toString()] && !studentGradesCount[coursId.toString()]) {
    
    await loadClassStudentCount(coursId);
    await loadClassGrades(coursId);
  }
  
  showHistogram[coursId.toString()] = !showHistogram[coursId.toString()];
}

const studentCounts = reactive<{ [id: string]: number }>({});
const studentGradesCount = reactive<{ [id: string]: Record<Note, number> }>({});
const showHistogram: { [key: string]: boolean } = reactive({});

onMounted(programmeStore.rafraichirDepuisApi);

programmeStore.$subscribe(() =>
    sessionEtudeStore.rafraichirPourProgramme(numeroProgramme.value));
sessionEtudeStore.$subscribe(() =>
    coursStore.rafraichirPourProgrammeEtSession(new Guid(idProgramme.value?.toString() ?? ""), idSession.value));

coursStore.$subscribe(() =>
    reussiteStore.refreshReussiteParCoursPourProgrammeEtSession(new Guid(idProgramme.value?.toString() ?? ""), idSession.value));

</script>

<style lang="scss">

.info-message{
  margin-bottom: 20px; /* Adjust margin as needed */
  font-style: italic;
}

.toggle-button {
  border: 1px solid #7AC142; 
  border-radius: 4px;
  padding: 6px 12px; 
  background-color: #ABEFAA; 
  color: black;
  font-size: 22px; 
  cursor: pointer;
  position: absolute;
  bottom: 15px;
  left: 17px;
}


.label-axe-x {
  cursor: pointer;

  &:hover {
    text-decoration: underline;
  }
}
</style>