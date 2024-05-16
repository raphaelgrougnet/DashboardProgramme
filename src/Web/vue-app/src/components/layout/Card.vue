<template>
  <section :class="{ 'card--expand': isExpanded }" class="card">
    <div v-if="title" class="card__expand-trigger">
      <div class="card__title-container">
        <div class="card__title-subcontainer">
          <h2 class="card__title">
            {{ title }}
          </h2>
          <slot name="on-title-right"></slot>
        </div>
        <BtnLink v-if="linkText && linkPath" :name="linkText" :path="linkPath"/>
        <button v-else-if="downloadLabel" class="btn btn--blue" @click="handleDownload()">
          <IconDownload class="icon icon--black"/>
          {{ downloadLabel }}
        </button>
      </div>

      <button
          v-if="$windowWidth < 768"
          :class="{ 'plus-sign--plus': isExpanded }"
          class="plus-sign plus-sign--minus"
          type="button"
          @click="toggleExpansion"
      ></button>
    </div>

    <Transition name="expand">
      <div
          v-show="isExpanded || $windowWidth >= 768 || title == null"
          ref="content"
          class="card__content"
      >
        <slot></slot>
      </div>
    </Transition>
  </section>
</template>

<script lang="ts" setup>
import {onMounted, ref} from "vue";
import BtnLink from "@/components/layout/BtnLink.vue";
import IconDownload from "@/assets/icons/icon__download.svg";

// eslint-disable-next-line
defineProps<{
  title?: string;
  linkText?: string;
  linkPath?: any;
  downloadLabel?: string;
}>();

let isExpanded = ref<boolean>(false);
let content = ref<HTMLElement>();
let height = ref<string>();

// eslint-disable-next-line
const emit = defineEmits<{
  (event: "download"): void
}>();

onMounted(() => {
  height.value = `${
      content.value != undefined ? content.value.scrollHeight : 0
  }px`;
});

function toggleExpansion() {
  isExpanded.value = !isExpanded.value;
}

function handleDownload() {
  emit("download")
}
</script>

<style lang="scss" scoped>
.expand-leave-active,
.expand-enter-active {
  transition: max-height 0.2s cubic-bezier(0.69, 0.33, 0.16, 0.97);
  overflow: hidden;
}

.expand-enter-to,
.expand-leave-from {
  max-height: v-bind(height);
}

.expand-enter-from,
.expand-leave-to {
  max-height: 0;
}
</style>
