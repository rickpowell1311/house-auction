<script setup lang="ts">
import { PhHouse } from '@phosphor-icons/vue';

interface Props {
  type: "face-up" | "face-down" | "place-holder";
}

const props = defineProps<Props>();
</script>

<template>
  <div v-if="props.type === 'place-holder'" class="min-w-48 w-48 h-64" />
  <div v-else class="min-w-48 w-48 h-64 border-white bg-white border-4 rounded-2xl shadow-lg shadow-black">
    <slot v-if="props.type === 'face-up'" />
    <div v-else-if="props.type === 'face-down'" class="w-full h-full rounded-2xl bg-primary-500 relative">
      <div class="absolute top-0 left-0 w-full h-full z-10">
        <div class="p-2 h-full overflow-clip">
          <div class="w-96 -rotate-45 -ml-28 -mt-14">
            <p class="text-primary-600 font-bold text-2xl break-words opacity-30 flex flex-wrap gap-2">
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
              <PhHouse />
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>