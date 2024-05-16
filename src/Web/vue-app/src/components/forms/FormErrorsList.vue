<template>
  <div class="form-errors-list">
    <p v-if="title" class="form-errors-list__title">
      <IconWarning class="icon"/>
      {{ title }}
    </p>
    <ul class="form-errors-list__list">
      <li v-for="error in errorsList" :key="error" class="form-errors-list__item">{{ error }}</li>
    </ul>
  </div>
</template>

<script lang="ts" setup>
import IconWarning from "@/assets/icons/icon__warning.svg"

// eslint-disable-next-line
const props = defineProps<{
  title?: string;
  errorsList: string[];
}>();
// eslint-disable-next-line
</script>
