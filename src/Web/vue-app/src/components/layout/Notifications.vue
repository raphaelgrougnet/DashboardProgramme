<template>
  <div class="notifications">
    <notifications position="top right">
      <template #body="props">
        <div :class="`notification notification--${props.item.type}`">
          <div class="notification__content">
            <div class="notification__icon">
              <IconSuccess v-if="props.item.type === 'success'" class="icon icon--green"/>
              <IconWarning v-if="props.item.type === 'error'" class="icon"/>
            </div>
            <div class="notification__text" v-html="props.item.text"></div>
          </div>
        </div>
      </template>
    </notifications>
  </div>
</template>

<script lang="ts" setup>
import IconSuccess from "@/assets/icons/icon__check.svg";
import IconWarning from "@/assets/icons/icon__warning.svg";
</script>
