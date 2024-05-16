<template>
  <form
      class="form"
      novalidate
      @submit.prevent="handleSubmit"
  >
    <FormRow>
      <FormInput
          :ref="addFormInputRef"
          v-model="member.firstName"
          :label="t('member.firstName')"
          :placeholder="t('member.placeholders.firstName')"
          name="firstName"
          type="text"
          @validated="handleValidation"/>
      <FormInput
          :ref="addFormInputRef"
          v-model="member.lastName"
          :label="t('member.lastName')"
          :placeholder="t('member.placeholders.lastName')"
          name="lastName"
          type="text"
          @validated="handleValidation"/>
    </FormRow>
    <FormInput
        :ref="addFormInputRef"
        v-model="member.email"
        :label="t('member.email')"
        :placeholder="t('member.placeholders.email')"
        :rules="[required, mustMatchEmailFormat]"
        name="email"
        type="email"
        @validated="handleValidation"/>
    <FormInput
        :ref="addFormInputRef"
        v-model="member.password"
        :label="t('member.password')"
        :placeholder="t('member.placeholders.password')"
        :rules="mode == 'edit' ? [] : rulesForPassword"
        name="password"
        type="password"
        @validated="handleValidation"/>
    <FormSelect
        :ref="addFormInputRef"
        v-model="member.roles"
        :label="t('member.roles')"
        :options="roles"
        name="roles"
        @validated="handleValidation"/>
    <FormMultiSelect
        :ref="addFormInputRef"
        :label="t('member.programmes')"
        :modelValue="programmesSelectionnes"
        :options="programmes"
        :rules="[]"
        name="programmes"
        @onOptionChange="onProgrammeChange"
        @validated="handleValidation"/>

    <button v-if="!props.member" class="form__submit btn btn--fullscreen" type="submit">
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
import {computed, ref} from "vue"
import {useI18n} from "vue3-i18n"
import {notifyError} from "@/notify"
import {Status} from "@/validation"
import {Member, Programme} from "@/types/entities"
import FormSelect from "@/components/forms/FormSelect.vue";
import {mustMatchEmailFormat, mustMatchPasswordFormat, required} from "@/validation/rules";
import FormMultiSelect from "@/components/forms/FormMultiSelect.vue";
import {FormOption, Guid} from "@/types";

// eslint-disable-next-line no-undef
const props = defineProps<{
  member?: Member,
  programmesDisponibles?: Programme[],
}>();

const mode = props.member ? "edit" : "create";

// eslint-disable-next-line
const emit = defineEmits<{
  (event: "formSubmit", member: Member): void
}>();

const {t} = useI18n();

const member = ref<Member>(props.member ? {
  ...props.member, roles:
      (props.member.roles?.length ? props.member.roles : ["user"]).join("")
} as
    Member : {});

const formInputs = ref<any[]>([]);
const inputValidationStatuses: any = {};

const rulesForPassword = [mustMatchPasswordFormat, required]

const roles = [
  {name: 'admin', label: t("user.roles.admin")},
  {name: 'user', label: t("user.roles.user")}
];

const programmes = props.programmesDisponibles?.map((programme: Programme) => ({
  name: programme.id?.toString() ?? "",
  label: programme.nom ?? ""
})) ?? [];

function addFormInputRef(ref: typeof FormInput) {
  if (!formInputs.value.includes(ref) && ref)
    formInputs.value.push(ref)
}

async function handleValidation(name: string, validationStatus: Status) {
  inputValidationStatuses[name] = validationStatus.valid
}

async function handleSubmit() {
  formInputs.value.forEach((x: typeof FormInput) => x.validateInput());
  if (Object.values(inputValidationStatuses).some(x => x === false)) {
    notifyError(t('global.formErrorNotification'));
    return
  }
  emit("formSubmit", member.value as Member)
}

const programmesSelectionnes = computed(() =>
    member.value.programmes?.map((programme: Guid) => ({
      name: programme.toString(),
      label: programmes.find(x => x.name === programme.toString())?.label
    })) as FormOption[] | null ?? [] as FormOption[]);

function onProgrammeChange(programmes: FormOption[]) {
  member.value.programmes = programmes.map(x => new Guid(x.name));
}
</script>