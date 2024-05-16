<template>
  <div class="content-grid content-grid--subpage content-grid--subpage-table">
    <Breadcrumbs :title="t(`routes.admin.children.programmes.name`)"/>
    <div class="content-grid__header">
      <h1 class="back-link-title">{{ t(`routes.admin.children.programmes.name`) }}</h1>
      <div class="content-grid__filters">
        <SearchInput v-model="searchValue"/>
      </div>
    </div>
    <Card :title="t('global.quickLinks')" class="card--row-content">
      <BtnLink
          :name="t('routes.admin.children.programmes.add.name')"
          :path="{ name: 'admin.children.programmes.add' }"
      />
    </Card>
    <Card>
      <DataTable
          :headers="programmeHeader"
          :is-loading="programmesAreLoading"
          :items="tableProgrammes"
          :search-value="searchValue"
          @delete="deleteProgramme"
      />
    </Card>
  </div>
</template>

<script lang="ts" setup>
import i18n from "@/i18n";
import {useI18n} from "vue3-i18n";
import {computed, onMounted, Ref, ref} from "vue";
import {useProgrammeService} from "@/inversify.config";
import {notifyError, notifySuccess} from "@/notify";
import Card from "@/components/layout/Card.vue";
import Breadcrumbs from "@/components/layout/Breadcrumbs.vue";
import SearchInput from "@/components/layout/SearchInput.vue";
import DataTable from "@/components/layout/DataTable.vue";
import BtnLink from "@/components/layout/BtnLink.vue";
import {Programme} from "@/types/entities/programme";
import {Guid} from "@/types";

const {t} = useI18n();
const programmeService = useProgrammeService();

const allProgrammes = ref<Programme[]>([] as Programme[]) as Ref<Programme[]>;
const searchValue = ref("");
const programmesAreLoading = ref(false);

const tableProgrammes = computed(() => {
  let lang = i18n.getLocale();
  return allProgrammes.value.map((x: Programme) => ({
    id: x.id,
    nom: x.nom,
    numero: x.numero,
    actions: {
      update: {name: `admin.children.programmes.edit`, params: {id: x.id}},
      delete: true
    }
  })).sort((a: any, b: any) => {
    if (a.name < b.name) {
      return -1;
    }
    if (a.name > b.name) {
      return 1;
    }
    return 0;
  }) as Programme[];
});

onMounted(async () => await loadAllProgrammes());

async function deleteProgramme(programme: any) {
  if (programme?.id == null || !Guid.isValid(programme.id ?? ""))
    return;

  let confirmDelete = confirm(t("validation.programme.delete.confirmation"));
  if (!confirmDelete)
    return;

  let response = await programmeService.deleteProgramme(programme.id);
  if (response.succeeded) {
    allProgrammes.value = allProgrammes.value.filter(x => x.id !== programme.id);
    notifySuccess(t('validation.programme.delete.success'))
  } else {
    let errorMessages = response.getErrorMessages('validation.book.delete',
        'validation.programme.delete.errorOccured');
    notifyError(errorMessages[0])
  }
}


async function loadAllProgrammes() {
  programmesAreLoading.value = true;

  let programmes = await programmeService.getAllProgrammes();
  if (programmes && programmes.length > 0) {
    allProgrammes.value = programmes;
  }
  programmesAreLoading.value = false;
}

const programmeHeader = [
  {
    text: t("programme.name"),
    value: 'nom',
    sortable: true,
    width: 300,
  },
  {
    text: t("programme.number"),
    value: "numero",
    sortable: true,
    width: 150,
  },
  {
    text: t("global.table.actions"),
    value: "actions",
    width: 75
  },
];

</script>
