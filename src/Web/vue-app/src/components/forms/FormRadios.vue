<template>
  <div :class="{ error: !status.valid }" class="form__fields-container">
    <p v-if="title" class="form__fields-title">{{ title }}</p>
    <ul class="form__field form__radios">
      <li v-for="option in options" :key="option.name" class="form__radio">
        <input
            :id="`${name}-${option.name}`"
            :aria-describedby="`error__${name}`"
            :aria-invalid="!status.valid"
            :checked="modelValue === option.name"
            :name="name"
            :value="option.name"
            type="radio"
            @blur="handleBlur"
            @change="handleChange"
        />
        <label :for="`${name}-${option.name}`">
          {{ option.label }}
          <span v-if="required" class="form__indicator">*</span>
        </label>
      </li>
    </ul>
    <span
        v-if="!status.valid"
        :id="`error__${name}`"
        class="form__error-message"
    >
      {{ t('validation.select') }}
    </span>
  </div>
</template>

<script lang="ts" setup>
import {FormOption} from "@/types/formOption";
import {Status} from '@/validation'
import {ref} from "vue";
import {useI18n} from "vue3-i18n";

// eslint-disable-next-line
const props = defineProps<{
  title?: string;
  name: string;
  modelValue: string;
  options: FormOption[];
  required?: boolean;
}>();

// eslint-disable-next-line
defineExpose({
  //to call validation in parent.
  validateInput: validateRadios
});

// eslint-disable-next-line
const emit = defineEmits<{
  // states that the event has to be called 'update:modelValue'
  (event: "update:modelValue", value: string): void;
  (event: "validated", name: string, validationStatus: Status): void;
}>();

const {t} = useI18n();
const status = ref<Status>({valid: true});

function handleChange(e: Event) {
  const groupName = (e.target as HTMLInputElement).name;
  validateRadios(groupName);

  const value = (e.target as HTMLInputElement).value;
  emit("update:modelValue", value);
}

function handleBlur(e: Event) {
  const groupName = (e.target as HTMLInputElement).name;
  validateRadios(groupName)
}

function validateRadios(groupName: string) {
  const isChecked = document.querySelector(`[name=${groupName}]:checked`) != null;

  status.value = {valid: props.required ? isChecked : true};

  emit("validated", props.name, status.value);
}
</script>
