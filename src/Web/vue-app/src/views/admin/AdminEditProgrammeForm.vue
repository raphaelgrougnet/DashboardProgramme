<template>
  <div class="content-grid content-grid--subpage">
    <Breadcrumbs :title="t(`routes.admin.children.programmes.edit.name`)"/>
    <BackLinkTitle :title="t(`routes.admin.children.programmes.edit.name`)"/>
    <Card>
      <ProgrammeForm :programme="programme" @formSubmit="handleSubmit"/>
    </Card>
  </div>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";
import {notifyError, notifySuccess} from "@/notify";
import {useProgrammeService} from "@/inversify.config";
import {useRouter} from "vue-router";
import Breadcrumbs from "@/components/layout/Breadcrumbs.vue";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import Card from "@/components/layout/Card.vue";
import {ref} from "vue";
import ProgrammeForm from "@/components/programmes/ProgrammeForm.vue";
import {Programme} from "@/types/entities/programme";
import {IEditProgrammeRequest} from "@/types/requests";

// eslint-disable-next-line no-undef
const props = defineProps<{
  id: string
}>();

const {t} = useI18n();
const router = useRouter();
const programmeService = useProgrammeService();

const programme = ref<Programme>(await programmeService.getProgramme(props.id));

async function handleSubmit(programme: Programme) {
  let succeededOrNotResponse = await programmeService.editProgramme(programme as IEditProgrammeRequest);
  if (succeededOrNotResponse && succeededOrNotResponse.succeeded) {
    notifySuccess(t('validation.programme.edit.success'));
    setTimeout(() => router.back(), 1500);
  } else {
    let errorMessages = succeededOrNotResponse.getErrorMessages('validation.programme.edit');
    if (errorMessages.length == 0)
      notifyError(t('validation.programme.edit.errorOccured'));
    else
      notifyError(errorMessages[0])
  }
}
</script>
