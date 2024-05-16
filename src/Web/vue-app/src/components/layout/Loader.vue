<template>
  <Transition name="fade">
    <div :class="{'loader--inline': isInline}" class="loader">
      <div class="loader__ring">
        <div></div>
        <div></div>
        <div></div>
        <div></div>
      </div>
    </div>
  </Transition>
</template>

<script lang="ts" setup>
// eslint-disable-next-line no-undef
defineProps<{
  isInline?: boolean;
}>();
</script>

<style lang="scss" scoped>
.fade-leave-active,
.fade-enter-active {
  transition: opacity 0.2s cubic-bezier(0.69, 0.33, 0.16, 0.97);
  overflow: hidden;
}

.fade-enter-to,
.fade-leave-from {
  opacity: 1;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
</style>
