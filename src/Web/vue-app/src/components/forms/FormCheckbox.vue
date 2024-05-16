<template>
  <div :class="{'error':!status.valid}" class="form__field">
    <input
        :id="name"
        v-model="checked"
        :aria-describedby="'error__' + name"
        :aria-invalid="!status.valid"
        :name="name"
        type="checkbox"
        @blur="handleBlur"
        @change="handleChange"
    />
    <label :for="name">
      {{ label ? label : name }}
      <span v-if="required" class="form__indicator">*</span>
    </label>

    <span v-if="!status.valid" :id="'error' + name" class="form__error-message">
      {{ status.message }}
    </span>
  </div>
</template>

<script lang="ts" setup>
import {Status, validateBoolean} from '@/validation'
import {requiredBoolean} from '@/validation/rules';
import {ref} from "vue";

// eslint-disable-next-line
const props = defineProps<{
  name: string
  modelValue: boolean,
  label?: string,
  required?: boolean
}>();

// eslint-disable-next-line
defineExpose({
  //to call validation in parent.
  validateInput: validateCheckbox
});

// eslint-disable-next-line
const emit = defineEmits<{
  // states that the event has to be called 'update:modelValue'
  (event: "update:modelValue", value: boolean): void;
  (event: "validated", name: string, validationStatus: Status): void;
}>();

const status = ref<Status>({valid: true});
const checked = ref<boolean>(props.modelValue);

function handleChange(e: Event) {
  const checked = (e.target as HTMLInputElement).checked;

  validateCheckbox(checked);

  emit("update:modelValue", checked);
}

function handleBlur(e: Event) {
  const checked = (e.target as HTMLInputElement).checked;
  validateCheckbox(checked)
}

function validateCheckbox(checked: boolean) {
  status.value = validateBoolean(checked, props.required ? [requiredBoolean] : []);
  emit("validated", props.name, status.value);
}
</script>
