<script setup lang="ts">
import type { CardStoryblok } from '@/_shared/components/storyblok/schema/component-types';

const props = defineProps<{ blok: CardStoryblok }>();
</script>

<template>
  <div class="w-full h-full overflow-hidden relative rounded-2xl">
    <div class="absolute top-0 left-0 w-full h-full z-10">
      <div class="p-2">
        <div class="flex justify-between">
          <div
            class="p-1 rounded-lg bg-secondary w-12 h-12 flex justify-center items-center border-white border-solid border-2">
            <p class="text-white font-bold text-3xl">{{ props.blok.value }}</p>
          </div>
        </div>
      </div>
    </div>
    <img class="object-cover w-full h-full" :src="props.blok.picture.filename" :alt=props.blok.picture.alt>
  </div>
</template>