<template>
  <FormRow>
    <FormInput
        :ref="addFormInputRef"
        :label="labelFr"
        :modelValue="props.valueFr"
        :name="nameFr"
        :rules="rulesFr"
        type="text"
        @validated="handleValidation"/>
    <FormInput
        :ref="addFormInputRef"
        :label="labelEn"
        :modelValue="props.valueEn"
        :name="nameEn"
        :rules="rulesEn"
        type="text"
        @validated="handleValidation"/>
  </FormRow>
</template>

<script lang="ts" setup>
import FormInput from '@/components/forms/FormInput.vue'
import FormRow from '@/components/forms/FormRow.vue'
import {Status} from '@/validation'
import {ref} from "vue";
import {Rule} from '@/validation/rules';

// eslint-disable-next-line
const props = defineProps<{
  valueFr?: string
  valueEn?: string
  nameFr: string
  nameEn: string
  labelFr?: string
  labelEn?: string
  rulesFr?: Rule[]
  rulesEn?: Rule[]
}>();

// eslint-disable-next-line
defineExpose({
  //to call validation in parent.
  validateInput
});

// eslint-disable-next-line
const emit = defineEmits<{
  // states that the event has to be called 'validated'
  (event: "validated", name: string, validationStatus: Status): void;
}>();

// const valueFr = ref<string>(props.valueFr ?? '')
// const valueEn = ref<string>(props.valueEn ?? '')

const formInputs = ref<typeof FormInput[]>([]);

function addFormInputRef(ref: typeof FormInput) {
  if (!formInputs.value.includes(ref))
    formInputs.value.push(ref)
}

function validateInput() {
  formInputs.value.forEach((x: typeof FormInput) => x.validateInput())
}

async function handleValidation(name: string, validationStatus: Status) {
  emit("validated", name, validationStatus);
}
</script>
