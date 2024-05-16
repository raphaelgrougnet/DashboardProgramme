<template>
  <div class="content-grid content-grid--subpage">
    <Breadcrumbs :title="t(`routes.admin.children.members.edit.name`)"/>
    <BackLinkTitle :title="t(`routes.admin.children.members.edit.name`)"/>
    <Card>
      <MemberForm :member="member" :programmesDisponibles="programmesDisponibles" @formSubmit="handleSubmit"/>
    </Card>
  </div>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";
import {notifyError, notifySuccess} from "@/notify";
import {useMemberService, useProgrammeService} from "@/inversify.config";
import {useRouter} from "vue-router";
import Breadcrumbs from "@/components/layout/Breadcrumbs.vue";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import Card from "@/components/layout/Card.vue";
import {ref} from "vue";
import {Member} from "@/types/entities/member";
import {IEditMemberRequest} from "@/types/requests";
import {Programme} from "@/types";
import MemberForm from "@/components/membres/MemberForm.vue";

// eslint-disable-next-line no-undef
const props = defineProps<{
  id: string
}>();

const {t} = useI18n();
const router = useRouter();
const programmeService = useProgrammeService();
const memberService = useMemberService();

const programmesDisponibles = ref<Programme[]>(await programmeService.getAllProgrammes());
const member = ref<Member>(await memberService.getMember(props.id));

async function handleSubmit(member: Member) {
  let succeededOrNotResponse = await memberService.editMember(member as IEditMemberRequest);
  if (succeededOrNotResponse && succeededOrNotResponse.succeeded) {
    notifySuccess(t('validation.member.edit.success'));
    setTimeout(() => router.back(), 1500);
  } else {
    let errorMessages = succeededOrNotResponse.getErrorMessages('validation.member.edit');
    if (errorMessages.length == 0)
      notifyError(t('validation.member.edit.errorOccured'));
    else
      notifyError(errorMessages[0])
  }
}
</script>