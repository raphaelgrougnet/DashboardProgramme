<template>
  <div class="content-grid content-grid--subpage">
    <Breadcrumbs :title="t(`routes.admin.children.programmes.add.name`)"/>
    <BackLinkTitle :title="t(`routes.admin.children.programmes.add.name`)"/>
    <Card>
      <ProgrammeForm @formSubmit="handleSubmit"/>
    </Card>
  </div>
</template>

<script lang="ts" setup>
import Breadcrumbs from "@/components/layout/Breadcrumbs.vue";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import ProgrammeForm from "@/components/programmes/ProgrammeForm.vue";
import Card from "@/components/layout/Card.vue";
import {useI18n} from "vue3-i18n";
import {Programme} from "@/types/entities/programme";
import {notifyError, notifySuccess} from "@/notify";
import {useProgrammeService} from "@/inversify.config";
import {useRouter} from "vue-router";
import {ICreateProgrammeRequest} from "@/types/requests";

const {t} = useI18n();
const router = useRouter();
const programmeService = useProgrammeService();

async function handleSubmit(programme: Programme) {
  let succeededOrNotResponse = await programmeService.createProgramme(programme as ICreateProgrammeRequest);
  if (succeededOrNotResponse && succeededOrNotResponse.succeeded) {
    notifySuccess(t('validation.programme.add.success'));
    setTimeout(() => router.back(), 1500);
  } else {
    let errorMessages = succeededOrNotResponse.getErrorMessages('validation.programme.add');
    if (errorMessages.length == 0)
      notifyError(t('validation.programme.add.errorOccured'));
    else
      notifyError(errorMessages[0])
  }
}
</script>
