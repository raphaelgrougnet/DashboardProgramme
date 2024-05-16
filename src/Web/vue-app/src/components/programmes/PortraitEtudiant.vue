<template>
  <div id="chart">
    <apexchart :options="chartOptions" :series="series" height="350" type="line"></apexchart>
  </div>

</template>

<script lang="ts" setup>
import {computed, defineProps, onMounted, ref} from "vue";
import {usePortraitEtudiantStore} from "@/stores/portraitEtudiantStore";
import {useSessionEtudeStore} from "@/stores/sessionEtudeStore";
import {SessionEtude} from "@/types";
import {Saison} from "@/types/enums/Saison";
import {usePortraitEtudiantService} from "@/inversify.config";

const props = defineProps(["idProgramme"]);
const portraitEtudiantStore = usePortraitEtudiantStore()
const sessionEtudeStore = useSessionEtudeStore();
const seriesData = ref(portraitEtudiantStore._seriesData);

function comparerAnneeEtSaison(a: SessionEtude, b: SessionEtude): number {
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


const series = computed(() => [
  {
    name: "Résident temporaires",
    data: seriesData.value.rt
  },
  {
    name: "R18",
    data: seriesData.value.r18
  },
  {
    name: "Services adaptés",
    data: seriesData.value.sa
  },
]);
const chartOptions = {
  chart: {
    height: 350,
    type: 'line',
    dropShadow: {
      enabled: true,
      color: '#000',
      top: 18,
      left: 7,
      blur: 10,
      opacity: 0.2
    },
    zoom: {
      enabled: false
    },
    toolbar: {
      show: false
    }
  },
  colors: ['#77B6EA', '#545454', "#7ac142"],
  dataLabels: {
    enabled: true,
  },
  stroke: {
    curve: 'smooth'
  },
  title: {
    text: 'Proportion des étudiants à statut particulier par session',
    align: 'left'
  },
  grid: {
    borderColor: '#e7e7e7',
    row: {
      colors: ['#f3f3f3', 'transparent'],
      opacity: 0.5
    },
  },
  markers: {
    size: 1
  },
  xaxis: {
    categories: labelsSessions.value,
    title: {
      text: 'Session'
    }
  },
  yaxis: {
    title: {
      text: 'Proportion en %'
    },
    min: 0,
    max: 100
  },
  legend: {
    position: 'top',
    horizontalAlign: 'right',
    floating: true,
    offsetY: -15,
    offsetX: -5
  }
};


onMounted(async () => {
  portraitEtudiantStore.setIdProgramme(props.idProgramme)
  seriesData.value = await usePortraitEtudiantService().getSeriesData(props.idProgramme);
});
</script>

<style lang="scss" scoped>

</style>