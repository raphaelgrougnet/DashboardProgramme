<template>
  <div :class="{ error: !status.valid }" class="form__field">
    <select
        :id="name"
        v-model="inputValue"
        :aria-describedby="`error__${name}`"
        :aria-invalid="!status.valid"
        :name="name"
        @blur="handleBlur"
        @change="handleChange"
    >
      <option v-for="option in options" :key="option.name" :selected="modelValue === option.name" :value="option.name">
        {{ option.label }}
      </option>
    </select>
    <label v-if="!noLabel" :for="name">
      {{ label ? label : name }}
      <span v-if="isRequired" class="form__indicator">*</span>
    </label>

    <span v-if="!status.valid" :id="`error__${name}`" class="form__error-message">
      {{ t('validation.select') }}
    </span>
  </div>
</template>

<script lang="ts" setup>
import {FormOption} from "@/types/formOption";
import {Status, validate} from '@/validation'
import {required, Rule} from '@/validation/rules'
import {ref, watch} from "vue";
import {useI18n} from "vue3-i18n";

// eslint-disable-next-line
const props = defineProps<{
  name: string;
  modelValue: string;
  label?: string;
  rules?: Rule[];
  options: FormOption[];
  noLabel?: boolean;
}>();

// eslint-disable-next-line
defineExpose({
  //to call validation in parent.
  validateInput
});

// eslint-disable-next-line
const emit = defineEmits<{
  // states that the event has to be called 'update:modelValue'
  (event: "update:modelValue", value: string): void;
  (event: "validated", name: string, validationStatus: Status): void;
}>();

const {t} = useI18n();
const status = ref<Status>({valid: true});
const isRequired = !(props.rules != null && props.rules.length == 0);
const inputValue = ref<string>(props.modelValue as string);
watch(props, newProps => inputValue.value = newProps.modelValue);

function handleChange() {
  validateInput();
  emit("update:modelValue", inputValue.value);
}

function handleBlur() {
  validateInput()
}

function validateInput() {
  let validationRules = props.rules ? props.rules : [required];
  status.value = validate(inputValue.value, validationRules);

  emit("validated", props.name, status.value);
}
</script>
