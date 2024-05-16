<template>
  <router-link v-if="path" :to="path" class="text-link">
    {{ text }}
  </router-link>
  <a
      v-else-if="href"
      :href="href"
      class="text-link"
      target="_blank"
  >
    {{ text }}
  </a>
</template>

<script lang="ts" setup>
// eslint-disable-next-line
defineProps<{
  text: string;
  path?: any;
  href?: string;
}>();
</script>
