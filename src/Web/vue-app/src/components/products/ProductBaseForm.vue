<template>
  <div>
    <Fieldset :title="props.title">
      <FormRow>
        <FormFiles
            :ref="addFormInputRef"
            v-model="product.cardImage"
            :default-selected-files="product.savedCardImage != null ? [product.savedCardImage] : []"
            :label="t('products.form.image-card')"
            :possiblesTypes="['image/png', 'image/jpg', 'image/jpeg', 'image/webp']"
            :tooltip="t(`products.form.image-card-tooltip`)"
            can-delete
            name="cardImage"
            @change="onImageChange($event, 'card')"
            @validated="handleValidation"
            @on-default-selected-files-change="onSavedImageChange($event, 'card')"
        />
        <slot name="image"></slot>
      </FormRow>

      <slot name="after-images"></slot>

      <FormRow>
        <FormInput
            :ref="addFormInputRef"
            v-model="product.nameFr"
            :label="t(props.nameFrLabel ?? 'products.form.name-fr')"
            name="name-fr"
            type="text"
            @validated="handleValidation"/>
        <FormInput
            :ref="addFormInputRef"
            v-model="product.nameEn"
            :label="t(props.nameEnLabel ?? 'products.form.name-en')"
            name="name-en"
            type="text"
            @validated="handleValidation"/>
      </FormRow>

      <FormTextEditor
          :ref="addFormInputRef"
          v-model="product.descriptionFr"
          :label="t(props.descriptionFrLabel ?? 'products.form.description-fr')"
          name="description-fr"
          @validated="handleValidation"
      />
      <FormTextEditor
          :ref="addFormInputRef"
          v-model="product.descriptionEn"
          :label="t(props.descriptionEnLabel ?? 'products.form.description-en')"
          name="description-en"
          @validated="handleValidation"
      />

      <slot name="before-prices"></slot>

    </Fieldset>

    <Fieldset :title="t('products.form.price')">
      <FormRow>
        <FormInput
            :ref="addFormInputRef"
            v-model.number="product.price"
            :label="t('products.form.price')"
            :rules="[required, min(0)]"
            :tooltip="t('products.form.no-taxes')"
            name="member-price"
            type="number"
            @validated="handleValidation"
        />
      </FormRow>
    </Fieldset>
  </div>
</template>

<script lang="ts" setup>
import {ref} from "vue";
import {Status} from "@/validation";
import {min, required} from "@/validation/rules";
import {useI18n} from "vue3-i18n";
import FormInput from "@/components/forms/FormInput.vue";
import Fieldset from "@/components/forms/Fieldset.vue";
import FormRow from "@/components/forms/FormRow.vue";
import FormFiles from "@/components/forms/FormFiles.vue";
import {Product} from "@/types/entities/product";
import FormTextEditor from "@/components/forms/FormTextEditor.vue";

const {t} = useI18n();

// eslint-disable-next-line no-undef
const props = defineProps<{
  product: Product,
  title: string
  nameFrLabel?: string
  nameEnLabel?: string
  descriptionFrLabel?: string
  descriptionEnLabel?: string
}>();

// eslint-disable-next-line
const emit = defineEmits<{
  (event: "handleValidation", name: string, validationStatus: Status): void
  (event: "registerFormInputRef", ref: typeof FormInput): void
}>();

const product = ref<Product>(props.product);

function addFormInputRef(ref: typeof FormInput) {
  emit("registerFormInputRef", ref);
}

async function onImageChange(event: any, type: string) {
  const file = event?.target?.files[0];
  if (file && type === "card") product.value.cardImage = file as File
}

async function onSavedImageChange(url: string, type: string) {
  if (type === "card") product.value.savedCardImage = url
}

async function handleValidation(name: string, validationStatus: Status) {
  emit("handleValidation", name, validationStatus);
}
</script>
