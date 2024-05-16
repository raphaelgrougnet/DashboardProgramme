<template>
  <div class="form__field">
    <input
        :id="name"
        :name="name"
        :value="modelValue ? modelValue : t('global.undefined')"
        readonly
        type="text"
    />
    <label :for="name"> {{ label ? label : name }}</label>
  </div>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";

// eslint-disable-next-line no-undef
defineProps<{
  name: string
  label?: string
  modelValue: string
}>();

const {t} = useI18n()
</script>
