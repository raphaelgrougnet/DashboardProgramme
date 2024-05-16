<template>
  <div class="content-grid content-grid--subpage">
    <Breadcrumbs :title="t(`routes.admin.children.members.add.name`)"/>
    <BackLinkTitle :title="t(`routes.admin.children.members.add.name`)"/>
    <Card>
      <MemberForm :programmesDisponibles="programmesDisponibles" @formSubmit="handleSubmit"/>
    </Card>
  </div>
</template>

<script lang="ts" setup>
import Breadcrumbs from "@/components/layout/Breadcrumbs.vue";
import BackLinkTitle from "@/components/layout/BackLinkTitle.vue";
import MemberForm from "@/components/membres/MemberForm.vue";
import Card from "@/components/layout/Card.vue";
import {useI18n} from "vue3-i18n";
import {Member, Programme} from "@/types/entities";
import {notifyError, notifySuccess} from "@/notify";
import {useMemberService, useProgrammeService} from "@/inversify.config";
import {ICreateMemberRequest} from "@/types/requests/createMemberRequest";
import {useRouter} from "vue-router";
import {ref} from "vue";

const {t} = useI18n();
const router = useRouter();
const memberService = useMemberService();

const programmeService = useProgrammeService();
const programmesDisponibles = ref<Programme[]>(await programmeService.getAllProgrammes());

async function handleSubmit(membre: Member) {
  let req: ICreateMemberRequest = {
    firstName: membre.firstName,
    lastName: membre.lastName,
    email: membre.email,
    password: membre.password,
    role: membre.roles?.toString(),
    programmes: membre.programmes?.map(p => p.toString()),
  };
  console.log({membre, req});
  let succeededOrNotResponse = await memberService.createMember(req);
  if (succeededOrNotResponse && succeededOrNotResponse.succeeded) {
    notifySuccess(t('validation.member.add.success'));
    setTimeout(() => router.back(), 1500);
  } else {
    let errorMessages = succeededOrNotResponse.getErrorMessages('validation.member.add');
    if (errorMessages.length == 0)
      notifyError(t('validation.member.add.errorOccured'));
    else
      notifyError(errorMessages[0])
  }
}
</script>