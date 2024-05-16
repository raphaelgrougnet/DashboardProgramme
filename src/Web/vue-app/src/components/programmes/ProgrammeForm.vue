<template>
  <form
      class="form"
      enctype="application/x-www-form-urlencoded"
      novalidate
      @submit.prevent="handleSubmit"
  >

    <FormRow>
      <FormInput
          :ref="addFormInputRef"
          v-model="programme.nom"
          :label="t('programme.name')"
          name="name"
          type="text"
          @validated="handleValidation"/>
      <FormInput
          :ref="addFormInputRef"
          v-model="programme.numero"
          :label="t('programme.number')"
          :rules="rules"
          name="number"
          type="text"
          @validated="handleValidation"/>
    </FormRow>


    <button v-if="!props.programme" class="form__submit btn btn--fullscreen" type="submit">
      {{ t("global.add") }}
    </button>
    <button v-else class="form__submit btn btn--fullscreen" type="submit">
      {{ t("global.save") }}
    </button>
  </form>
</template>

<script lang="ts" setup>
import FormRow from "@/components/forms/FormRow.vue"
import FormInput from "@/components/forms/FormInput.vue"
import {ref} from "vue"
import {useI18n} from "vue3-i18n"
import {notifyError} from "@/notify"
import {Status} from "@/validation"
import {Programme} from "@/types/entities/programme";
import {mustMatchNumeroProgrammeFormat, Rule} from "@/validation/rules";

// eslint-disable-next-line no-undef
const props = defineProps<{
  programme?: Programme
}>();

// eslint-disable-next-line
const emit = defineEmits<{
  (event: "formSubmit", programme: Programme): void
}>();

const {t} = useI18n();

const programme = ref<Programme>(props.programme ?? {});

const formInputs = ref<any[]>([]);
const inputValidationStatuses: any = {};
const rules: Rule[] = [mustMatchNumeroProgrammeFormat];

function addFormInputRef(ref: typeof FormInput) {
  if (!formInputs.value.includes(ref) && ref)
    formInputs.value.push(ref)
}

async function handleValidation(name: string, validationStatus: Status) {
  inputValidationStatuses[name] = validationStatus.valid;
}

async function handleSubmit() {
  formInputs.value.forEach((x: typeof FormInput) => x.validateInput());
  if (Object.values(inputValidationStatuses).some(x => x === false)) {
    notifyError(t('global.formErrorNotification'));
    return
  }
  emit("formSubmit", programme.value)
}
</script>