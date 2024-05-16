<template>
  <div class="dashboard">
    <LogoutPopup/>

    <div class="dashboard__navbar">
      <Navbar :member-is-loading="memberIsLoading"/>
    </div>

    <div class="dashboard__content-container">
      <Notifications/>

      <div class="dashboard__content">
        <RouterView v-slot="{Component}">
          <template v-if="Component">
            <Suspense>
              <component :is="Component"/>
              <template #fallback>
                <Loader/>
              </template>
            </Suspense>
          </template>
        </RouterView>
      </div>
    </div>
  </div>
</template>

<script lang="ts" setup>
import Notifications from "@/components/layout/Notifications.vue";
import Navbar from "@/components/navigation/Navbar.vue";
import {onMounted, ref} from "vue";
import {useMemberStore} from "@/stores/memberStore";
import {useMemberService} from "@/inversify.config";
import LogoutPopup from "@/components/layout/LogoutPopup.vue";
import Loader from "@/components/layout/Loader.vue";

const memberStore = useMemberStore();
const memberService = useMemberService();

const memberIsLoading = ref(true);

onMounted(async () => {
  memberIsLoading.value = true;
  let member = await memberService.getCurrentMember();
  if (member)
    memberStore.setMember(member);
  memberIsLoading.value = false
});

</script>

<style lang="scss">
@import "./sass/index.scss";
</style>
