<script setup lang="ts">
interface Props {
  isFaceUp: boolean;
}

const props = defineProps<Props>();
</script>

<template>
  <div class="min-w-48 w-48 h-64 border-white bg-white border-4 rounded-2xl shadow-lg shadow-black">
    <slot v-if="props.isFaceUp" />
    <div v-else class="w-full h-full rounded-2xl bg-primary-500 relative">
      <div class="absolute top-0 left-0 w-full h-full z-10">
        <div class="p-2 h-full overflow-clip">
          <div class="w-80 -rotate-45 -ml-20 -mt-10">
            <p class="text-primary-600 font-bold text-2xl break-words opacity-30 flex flex-wrap gap-2">
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
              <span class="material-symbols-rounded">home</span>
            </p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>