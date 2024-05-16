<template>
  <div class="content-grid content-grid--subpage content-grid--subpage-table">
    <Breadcrumbs :title="t(`routes.admin.children.members.name`)"/>
    <div class="content-grid__header">
      <h1 class="back-link-title">{{ t(`routes.admin.children.members.name`) }}</h1>
      <div class="content-grid__filters">
        <SearchInput v-model="searchValue"/>
      </div>
    </div>
    <Card :title="t('global.quickLinks')" class="card--row-content">
      <BtnLink
          :name="t('routes.admin.children.members.add.name')"
          :path="{ name: 'admin.children.members.add' }"
      />
    </Card>
    <Card>
      <DataTable
          :headers="memberHeader"
          :is-loading="membersAreLoading"
          :items="tableMembers"
          :search-value="searchValue"
          @delete="deleteMember"
      />
    </Card>
  </div>
</template>

<script lang="ts" setup>
import i18n from "@/i18n";
import {useI18n} from "vue3-i18n";
import {computed, onMounted, ref} from "vue";
import {useMemberService} from "@/inversify.config";
import Card from "@/components/layout/Card.vue";
import Breadcrumbs from "@/components/layout/Breadcrumbs.vue";
import SearchInput from "@/components/layout/SearchInput.vue";
import DataTable from "@/components/layout/DataTable.vue";
import BtnLink from "@/components/layout/BtnLink.vue";
import {IAuthenticatedMember, Member} from "@/types/entities";
import {Guid} from "@/types";
import {notifyError, notifySuccess} from "@/notify";

const {t} = useI18n();
const memberService = useMemberService();
const connectedMember = await memberService.getCurrentMember();

const allMembers = ref<Member[]>([] as Member[]);
const searchValue = ref("");
const membersAreLoading = ref(false);

const isNotConnectedMember = (memberEmail: string) => {
  if (connectedMember?.email?.toString()) {
    return connectedMember?.email?.toString() !== memberEmail.toString();
  } else {
    // Handle case when connectedMember is not yet fetched
    return false;
  }
};

const tableMembers = computed(() => {
  let lang = i18n.getLocale();
  return allMembers.value.map((x: any) => ({
    id: x.id as Guid,
    firstName: x.firstName,
    lastName: x.lastName,
    roles: x.roles,
    active: x.active ? t("global.yes") : t("global.no"),
    email: x.email,
    actions: {
      update: {name: `admin.children.members.edit`, params: {id: x.id}},
      delete: isNotConnectedMember(x.email)
    },
  } as Member)).sort((a: any, b: any) => {
    if (a.firstName + " " + a.lastName < b.firstName + " " + b.lastName) {
      return -1;
    }
    if (a.firstName + " " + a.lastName > b.firstName + " " + b.lastName) {
      return 1;
    }
    return 0;
  }) as Member[];
});

onMounted(async () => await loadAllMembers());

async function loadAllMembers() {
  membersAreLoading.value = true;

  let members = await memberService.getAllMembers();
  if (members && members.length > 0) {
    allMembers.value = members;
  }
  allMembers.value.forEach(member => {
    member.roles = member.roles?.length ? member.roles.map(r => t(`user.roles.${r}`)) : [t("user.roles.user")];
  });

  membersAreLoading.value = false;
}

async function deleteMember(member: any) {
  // Implement the logic for deleting a user
  if(member.id == null || !Guid.isValid(member.id ?? ""))
    return;
  
  
  if((connectedMember as IAuthenticatedMember).id == member.id){
    notifyError(t('validation.member.delete.connectedMember'));
    return;
  }

  let confirmDelete = confirm(t("validation.member.delete.confirmation"));

  if(!confirmDelete)
    return;

  let response = await memberService.deleteMember(member.id);
  console.log('Response JSON:', JSON.stringify(response));

  if(response.succeeded){
    allMembers.value = allMembers.value.filter(x => x.id !== member.id);
    notifySuccess(t('validation.member.delete.success'))
  }else{
    let errorMessages = response.getErrorMessages('validation.member.delete',
        'validation.member.delete.errorOccured');
    notifyError(t('validation.member.delete.errorOccured'))
  }
}


const memberHeader = [
  {
    text: t("global.firstName"),
    value: 'firstName',
    sortable: true,
    width: 200,
  },
  {
    text: t("global.lastName"),
    value: 'lastName',
    sortable: true,
    width: 200,
  },
  {
    text: t("global.email"),
    value: 'email',
    sortable: true,
    width: 300,
  },
  {
    text: t("global.active"),
    value: 'active',
    sortable: true,
    width: 50,
  },
  {
    text: t("global.roles"),
    value: 'roles',
    sortable: true,
    width: 100,
  },
  {
    text: t("global.table.actions"),
    value: "actions",
    width: 100
  },
];

</script>