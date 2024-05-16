<template>
  <apexchart :options="chartOptions" :series="series" width="100%"></apexchart>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";
import {computed, defineProps} from "vue";

const {t} = useI18n();

const props = defineProps({
  series: {
    type: Array<{ name: string, data: Array<number>, color: string }>,
    required: true,
  },
  categories: {
    type: Array<string>,
    required: true,
  },
  titre: {
    type: String,
    required: true,
  },
  titre_x: {
    type: String,
    required: true,
  },
  titre_y: {
    type: String,
    required: true,
  },
  format_valeur_y: {
    type: Function,
    required: false,
  },
  format_valeur_x: {
    type: Function,
    required: false,
  },
  format_tooltip_y: {
    type: Function,
    required: false,
  },
});

const chartOptions = computed(() => ({
  title: {
    text: props.titre,
    align: "center",
  },
  chart: {
    type: "bar",
  },
  plotOptions: {
    bar: {
      columnWidth: "100%",
      dataLabels: {
        total: {
          enabled: true,
        },
      }
    },
  },
  tooltip: {
    y: {
      formatter: props.format_tooltip_y,
    },
  },
  yaxis: {
    title: {
      text: props.titre_y
    },
    labels: {
      formatter: props.format_valeur_y
    },
  },
  xaxis: {
    title: {
      text: props.titre_x
    },
    categories: props.categories,
    labels: {
      formatter: props.format_valeur_x
    }
  },
}));

</script>