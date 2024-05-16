<template>
  <div v-if="programme" class="content-grid">
    <BackLinkTitle :title="titre"/>
    <h3 class="mb-0 pb-0">Nombre d'étudiants par cohorte par session</h3>
    <div class="mt-0 mb-2 py-0 row row-cols-2">
      <div>
        <GrilleSPE v-show="idProgramme" :id-programme="idProgramme.toString()" class="border"/>
      </div>
    </div>
    <div class="mt-0 mb-2 py-0 row row-cols-2">
      <div>
        <PortraitEtudiant :id-programme="idProgramme.toString()"/>
      </div>
    </div>

    <div v-if="lstSessions.length > 0" class="row row-cols-2 gap-3">
      <LinkCard v-for="session in lstSessions" :key="session.slug"
                :link-path="{name: 'programme:id.session:id', params: {programme: numeroProgramme, session:
                session.slug}}"
                :link-text="t('programme.:id.afficherSession')"
                :title="numeroProgramme + ' &ndash; ' + programme.nom" class="col fs-4 bg-slate-400 carte-programme">
        {{ t("enums.Saison." + session.saison) }} {{ session.annee }}
      </LinkCard>
    </div>
    <div v-else class="row row-cols-2 gap-3">
      <div class="bg-light text-bg-info p-4 rounded border border-info">
        <h2 class="fs-4 mb-3">{{ t('programme.:id.vierge.titre') }}</h2>
        <p>{{ t('programme.:id.vierge.message') }}</p>
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
import {Guid} from "@/types";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import {computed, onMounted} from "vue";
import {useSessionEtudeStore} from "@/stores/sessionEtudeStore";
import LinkCard from "@/components/layout/LinkCard.vue";
import {useReussiteStore} from "@/stores/reussiteStore";
import Loader from "@/components/layout/Loader.vue";
import GrilleSPE from "@/components/programmes/GrilleSPE.vue";
import PortraitEtudiant from "@/components/programmes/PortraitEtudiant.vue";
import {usePortraitEtudiantStore} from "@/stores/portraitEtudiantStore";

const route = useRoute();
const router = useRouter();
const {t} = useI18n();
const sessionEtudeStore = useSessionEtudeStore();
const programmeStore = useProgrammeStore();
const portraitEtudiantStore = usePortraitEtudiantStore();
const reussiteStore = useReussiteStore();
const codeProgramme = route.params.programme.toString().trim().toUpperCase();

const programme = computed(() => programmeStore.programmes.find(p => p.numero == codeProgramme));

const numeroProgramme = computed(() => programme.value ? programme.value?.numero as string : "");
const idProgramme = computed(() => programme.value?.id ?? new Guid(Guid.empty));
const titre = computed(() => numeroProgramme.value ? `${numeroProgramme.value} – ${programme.value?.nom}` : "");

const slugSessionPourProgramme = computed(() => numeroProgramme.value ?
    sessionEtudeStore.slugSessionEtudesParNumeroProgramme[numeroProgramme.value] ?? [] : []);

const lstSessions = computed(() =>
    Object.values(sessionEtudeStore.sessionEtudesParSlug)
        .filter(s => slugSessionPourProgramme.value.includes(s.slug ?? ""))
        .sort((a, b) => a.annee == b.annee ? 0 : (a.annee! > b.annee! ? -1 : 1)));

const reussite = computed(() => idProgramme.value ? reussiteStore.reussiteParSessionDeProgramme[idProgramme.value] ?? {} : {});

onMounted(async () => await programmeStore.rafraichirDepuisApi());
programmeStore.$subscribe(() => sessionEtudeStore.rafraichirPourProgramme(numeroProgramme.value));
programmeStore.$subscribe(() => reussiteStore.refreshReussiteParSessionPourProgramme(idProgramme.value));
programmeStore.$subscribe(() => {
  portraitEtudiantStore.setIdProgramme(idProgramme.value.toString());
  portraitEtudiantStore.getSeriesData()
});
</script>

<style lang="scss">
.label-axe-x {
  cursor: pointer;

  &:hover {
    text-decoration: underline;
  }
}
</style>