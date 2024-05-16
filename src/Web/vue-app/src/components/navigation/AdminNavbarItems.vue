<template>
  <ul class="navbar__nav">
    <li>
      <RouterLink :to="t('routes.programme.path')" class="navbar__navlink">{{ t('routes.programme.name') }}</RouterLink>
    </li>
    <li v-if="memberStore.hasRole(Role.Admin)">
      <Subnav :routeKey="'admin'"/>
    </li>
    <li v-if="memberStore.hasRole(Role.Admin)">
      <RouterLink :to="t('routes.import.path')" class="navbar__navlink">{{ t('routes.import.name') }}</RouterLink>
    </li>


    <li>
      <button class="navbar__navlink" @click="logout">{{ t('global.logout') }}</button>
    </li>
  </ul>
</template>

<script lang="ts" setup>
import {useMemberStore} from "@/stores/memberStore";
import {useI18n} from "vue3-i18n";
import Subnav from "./Subnav.vue";
import {Role} from "@/types";

const {t} = useI18n();
const memberStore = useMemberStore();

function logout() {
  localStorage.clear();
  sessionStorage.clear();
  window.location.replace("/Authentication/Logout/")
}
</script>