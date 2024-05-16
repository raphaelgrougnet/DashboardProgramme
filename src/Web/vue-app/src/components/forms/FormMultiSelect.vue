<template>
  <div :class="{ error: !status.valid }" class="form__field form-select-multiple">
    <VueMultiselect
        v-model="selectedOptions"
        :multiple="true"
        :options="options"
        :show-labels="true"
        deselectLabel="Appuyer sur Entrée pour retirer"
        label="label"
        placeholder="Choisir parmi la liste..."
        selectLabel="Appuyer sur Entrée pour ajouter"
        selectedLabel="Sélectionné(e)"
        track-by="name"
        @remove="onSelect"
        @select="onSelect"
    />

    <label v-if="!noLabel" :for="name">
      {{ label ? label : name }}
      <span v-if="isRequired" class="form__indicator">*</span>
    </label>

    <span v-if="!status.valid" :id="`error__${name}`" class="form__error-message">
      {{ t('validation.select') }}
    </span>

  </div>
</template>

<style lang="scss">
.multiselect {
  border: 0 !important;
  border-radius: 4px !important;
  background-color: #F8F8F8 !important;

  .multiselect__tags {
    border: 1px solid rgba(92, 92, 92, 0.2) !important;
    background: transparent !important;
  }
}

.multiselect__input {
  border: 0 !important;
  background: transparent !important;
}
</style>

<script lang="ts" setup>
import {FormOption} from "@/types/formOption";
import {Status, validateArray} from '@/validation'
import {requiredArray, RuleArray} from '@/validation/rules'
import {ref, watch} from "vue";
import {useI18n} from "vue3-i18n";
import VueMultiselect from 'vue-multiselect';
import "vue-multiselect/dist/vue-multiselect.css";

// eslint-disable-next-line
const props = defineProps<{
  name: string;
  modelValue: FormOption[];
  label?: string;
  rules?: RuleArray[];
  options: FormOption[];
  noLabel?: boolean;
  maxItems?: number;
}>();

// eslint-disable-next-line
defineExpose({
  //to call validation in parent.
  validateInput
});

// eslint-disable-next-line
const emit = defineEmits<{
  // states that the event has to be called 'update:modelValue'
  (event: "onOptionChange", value: FormOption[]): void;
  (event: "validated", name: string, validationStatus: Status): void;
}>();

const {t} = useI18n();
const status = ref<Status>({valid: true});
const isRequired = !(props.rules != null && props.rules.length == 0);
const selectedOptions = ref<FormOption[]>(props.modelValue as FormOption[]);

watch(props, newProps => selectedOptions.value = newProps.modelValue);

function validateInput() {
  let validationRules = props.rules ? props.rules : [requiredArray];
  status.value = validateArray(selectedOptions.value, validationRules);

  emit("validated", props.name, status.value);
}

const onSelect = () => {
  emit("onOptionChange", selectedOptions.value);
  validateInput();
};
</script>