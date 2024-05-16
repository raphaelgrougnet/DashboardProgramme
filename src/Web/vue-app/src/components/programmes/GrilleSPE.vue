<template>
  <table class="m-0 p-0 table table-sm">
    <thead>
    <tr>
      <th class="col text-start" rowspan="2" scope="col">Session</th>
      <th :colspan="nbMaxSpe" class="col border-0" scope="col">Cohorte</th>
    </tr>
    <tr>
      <th v-for="num in nbMaxSpe" :key="num" class="col-1" scope="col">{{ num }}</th>
    </tr>
    </thead>
    <tbody>
    <tr>
      <th scope="row">
        <RouterLink :to="{
              name: 'programme:id.session:id', params: {programme: numeroProgramme, session: sessionActuelle?.slug ??
               '.'}
            }" class="text-primary text-decoration-underline">{{ sessionActuelle?.slug }}
        </RouterLink>
      </th>
      <td v-for="num in nbMaxSpe" :key="num" class="border-secondary-subtle">
        {{ speAggregate.sessionActuelle?.[num] ?? 0 }}
      </td>
    </tr>
    <tr class="table-secondary">
      <th class="table-secondary" scope="row">Moyenne</th>
      <td v-for="num in nbMaxSpe" :key="num">
        {{ Math.round(speAggregate.moyenneSessionsPrecedentes?.[num] ?? 0) }}
      </td>
    </tr>
    <tr v-for="(sess, k) in troisDernieresSessions" :key="sess.id">
      <th scope="row">
        <RouterLink :to="{
              name: 'programme:id.session:id', params: {programme: numeroProgramme, session: sess.slug}
            }" class="text-primary text-decoration-underline">{{ sess.slug }}
        </RouterLink>
      </th>
      <td v-for="num in nbMaxSpe" :key="num">
        {{ speAggregate.troisDernieresSessions[k]?.[num] ?? 0 }}
      </td>
    </tr>
    <tr>
      <th></th>
      <th :colspan="nbMaxSpe" class="text-center">Nombre d'étudiants</th>
    </tr>
    </tbody>
  </table>
</template>

<script lang="ts" setup>
import {computed, defineProps, onMounted, ref} from "vue";
import {SessionEtude, SpeAggregate, SpePourSession} from "@/types";
import {useSpeQueryService} from "@/inversify.config";
import {useSessionEtudeStore} from "@/stores/sessionEtudeStore";
import {useProgrammeStore} from "@/stores/programmeStore";

const props = defineProps<{
  idProgramme: string
}>();

const sessionStore = useSessionEtudeStore();
const programmeStore = useProgrammeStore();

const nbMaxSpe = 8;

const speAggregate = ref({
  sessionActuelle: {} as SpePourSession,
  moyenneSessionsPrecedentes: {} as SpePourSession,
  troisDernieresSessions: [] as SpePourSession[],
} as SpeAggregate);

const sortDesc = (a: SessionEtude, b: SessionEtude) => a.annee == b.annee ? 0 : (a.annee! > b.annee! ? -1 : 1);

const troisDernieresSessions = computed(() => Object.values(sessionStore.sessionEtudesParSlug)
    ?.sort(sortDesc)?.reverse()?.slice(-4)?.reverse()?.slice(1) ?? [] as SpePourSession[]);

const sessionActuelle = computed(() => Object.values(sessionStore.sessionEtudesParSlug)
    ?.sort(sortDesc)?.reverse()?.at(-1) ?? null);

const numeroProgramme = computed(() => programmeStore.programmes?.find(p => p.id == props.idProgramme)?.numero);

onMounted(async () => {
  speAggregate.value = await useSpeQueryService().getSpeAggregatePourProgramme(props.idProgramme);
});
</script>

<style lang="scss" scoped>
@use "@/assets/plugins/bootstrap/css/bootstrap.min";

th {
  @extend .table-light;

  :any-link {
    color: inherit;

    &:hover {
      @extend .text-primary;
      text-decoration: underline !important;
    }
  }
}

td {
  font-family: var(--bs-font-monospace);
}

thead th, td {
  text-align: center;
}
</style>