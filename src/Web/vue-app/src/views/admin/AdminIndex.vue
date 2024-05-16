<template>
  <div>
    <h1 class="util--screen-reader">{{ t("routes.admin.name") }}</h1>
    <div class="content-grid content-grid--double">
      <Card :title="t('activities.activities')">
        <BtnLink :name="t('routes.admin.children.activities.add.name')"
                 :path="$router.currentRoute.value.path + '/' + t('routes.admin.childrens.activities.add.path')"/>
      </Card>
      <Card :title="t('activityOccurrences.activityOccurrences')">
        <BtnLink :name="t('routes.admin.children.activities.occurrences.add.name')"
                 :path="$router.currentRoute.value.path + '/' + t('routes.admin.childrens.activities.occurrences.add.path')"/>
      </Card>
      <Card :title="t('activities.activities')">
        <BtnLink :name="t('routes.admin.children.activities.add.name')"
                 :path="$router.currentRoute.value.path + '/' + t('routes.admin.childrens.activities.add.path')"/>
      </Card>
      <Card :title="t('routes.admin.children.memberships.name')">
        <BtnLink :name="t('routes.admin.children.memberships.name')"
                 :path="$router.currentRoute.value.path + '/' + t('routes.admin.childrens.memberships.path')"/>
      </Card>
    </div>
  </div>
</template>

<script lang="ts" setup>
import Card from '../components/Card.vue';
import BtnLink from '../components/BtnLink.vue';
import {useI18n} from "vue3-i18n";

const {t} = useI18n()
</script>
