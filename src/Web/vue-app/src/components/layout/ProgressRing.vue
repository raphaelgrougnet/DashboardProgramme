<template>
  <div :style="styles" class="progress-ring">
    <span class="progress-ring__label">{{ label }}</span>
    <svg :height="dimension" :width="dimension" class="progress-ring__line">
      <circle
          :cx="radius"
          :cy="radius"
          :r="normalizedRadius"
          :stroke-dasharray="circumference + ' ' + circumference"
          :stroke-width="stroke"
          :style="{ strokeDashoffset }"
      />
    </svg>
  </div>
</template>

<script lang="ts" setup>
import {computed} from "vue";

// eslint-disable-next-line
const props = defineProps<{
  label: number
  radius: number
  progress: number
  stroke: number
}>();

let normalizedRadius = computed(() => props.radius - props.stroke * 2);
let circumference = computed(() => normalizedRadius.value * 2 * Math.PI);
let strokeDashoffset = computed(() => circumference.value - props.progress / 100 * circumference.value);
let dimension = computed(() => props.radius * 2);
let styles = computed(() => ({
  height: dimension.value - 8 + 'px',
  width: dimension.value - 8 + 'px',
}))
</script>
