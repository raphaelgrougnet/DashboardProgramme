<template>
  <div class="search-input">
    <input
        id="search"
        :placeholder="t('global.search')"
        :value="modelValue"
        name="search"
        type="search"
        @input="handleInput"
    />
    <label for="search">
      <IconSearch class="icon icon--black"/>
    </label>
  </div>
</template>

<script lang="ts" setup>
import IconSearch from "@/assets/icons/icon__search.svg";
import {useI18n} from "vue3-i18n";

const {t} = useI18n();

// eslint-disable-next-line
defineProps<{
  modelValue: string | undefined
}>();

// eslint-disable-next-line
const emit = defineEmits<{
  // states that the event has to be called 'update:modelValue'
  (event: "update:modelValue", value: string): void;
}>();

function handleInput(e: Event) {
  const value = (e.target as HTMLInputElement).value;
  emit("update:modelValue", value);
}
</script>
