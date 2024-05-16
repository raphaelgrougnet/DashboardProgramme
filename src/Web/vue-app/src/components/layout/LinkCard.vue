<template>
  <RouterLink :to="linkPath">
    <Card :link-path="linkPath" :link-text="linkText" :title="title" class="link-card">
      <slot></slot>
    </Card>
  </RouterLink>
</template>

<script lang="ts" setup>
import Card from "@/components/layout/Card.vue";

// eslint-disable-next-line
defineProps<{
  title?: string;
  linkText?: string;
  linkPath?: any;
}>();
</script>

<style lang="scss" scoped>
.link-card:hover :any-link {
  background-color: #7AC142 !important;
  border-color: transparent;
  color: #000000;
}
</style>
