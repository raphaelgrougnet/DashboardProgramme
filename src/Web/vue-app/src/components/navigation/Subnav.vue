<template>
  <div v-if="routeKey" :class="{ 'subnav--expand': isExpanded }" class="subnav">
    <button class="subnav__title" @click="toggleExpansion">
      {{ t(`routes.${routeKey}.name`) }}
      <IconChevron :class="{'icon--rotate-180 icon--black' : !isExpanded}" class="icon icon--white"/>
    </button>

    <Transition name="expand">
      <ul
          v-if="directChildRoutes != null && directChildRoutes.length > 0"
          v-show="isExpanded"
          ref="content"
          class="subnav__content"
      >
        <li v-for="child in directChildRoutes" :key="child.name">
          <RouterLink :to="getChildPath(routeKey, child.name?.toString())" class="subnav__navlink">
            {{ t(`routes.${child.name?.toString()}.name`) }}
          </RouterLink>
        </li>
      </ul>
    </Transition>
  </div>
</template>

<script lang="ts" setup>
import {computed, onMounted, ref} from "vue";
import IconChevron from "@/assets/icons/icon__chevron.svg";
import {useI18n} from "vue3-i18n";
import {useRouter} from "vue-router";
import {getChildPath} from "@/router/helpers";

// eslint-disable-next-line
const props = defineProps<{
  routeKey: string
}>();

const {t} = useI18n();
const router = useRouter();

const directChildRoutes = computed(() => {
  const routes = router.getRoutes();

  return routes.filter(r => r.path == t(`routes.${props.routeKey}.path`))[0].children;
});

let isExpanded = ref<boolean>(false);
let content = ref<HTMLElement>();
let height = ref<string>();

onMounted(() => {
  height.value = `${
      content.value != undefined ? content.value.scrollHeight : 0
  }px`;
});

function toggleExpansion() {
  isExpanded.value = !isExpanded.value;
}
</script>

<style lang="scss" scoped>
.expand-leave-active,
.expand-enter-active {
  transition: max-height 0.2s cubic-bezier(0.69, 0.33, 0.16, 0.97);
  overflow: hidden;
}

.expand-enter-to,
.expand-leave-from {
  max-height: v-bind(height);
}

.expand-enter-from,
.expand-leave-to {
  max-height: 0;
}
</style>
