<template>
  <RouterView v-slot="{Component}">
    <template v-if="Component">
      <Suspense>
        <component :is="Component"/>
        <template #fallback>
          <Loader/>
        </template>
      </Suspense>
    </template>
  </RouterView>
</template>
<script lang="ts" setup>
import Loader from "@/components/layout/Loader.vue";
</script>