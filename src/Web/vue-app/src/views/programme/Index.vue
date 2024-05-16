<template>
  <div class="content-grid">
    <h1>{{ t('programme.mes-programmes') }}</h1>
    <div v-if="lstProgrammes.length > 0" class="row row-cols-2 gap-3">
      <LinkCard v-for="programme in lstProgrammes" :key="programme.numero"
                :link-path="{name: 'programme:id', params: {programme: programme.numero}}"
                :link-text="t('programme.index.afficherProgramme')"
                :title="programme.numero" class="col fs-4 bg-slate-400 carte-programme">
        {{ programme.nom }}
      </LinkCard>
      <div v-if="estAdmin" class="col">
        <div class="bg-light text-bg-info p-4 rounded border border-muted text-muted">
          <p><i class="bi bi-shield-check me-1"></i> <em>{{ t('programme.index.estAdmin') }}</em></p>
        </div>
      </div>
    </div>
    <div v-else class="row row-cols-2 gap-3">
      <div class="bg-light text-bg-info p-4 rounded border border-info">
        <h2 class="fs-4 mb-3">{{ t('programme.index.aucunProgramme.titre') }}</h2>
        <p>{{ t('programme.index.aucunProgramme.message') }}</p>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import {useI18n} from "vue3-i18n";
import {computed, onMounted} from "vue";
import {useProgrammeStore} from "@/stores/programmeStore";
import LinkCard from "@/components/layout/LinkCard.vue";
import {useMemberStore} from "@/stores/memberStore";
import {Role} from "@/types";

const {t} = useI18n();
const memberStore = useMemberStore();
const programmeStore = useProgrammeStore();
const lstProgrammes = computed(() => Object.values(programmeStore._programmes));
const estAdmin = memberStore.hasRole(Role.Admin);

onMounted(programmeStore.rafraichirDepuisApi);
</script>

<style lang="scss" scoped>
</style>