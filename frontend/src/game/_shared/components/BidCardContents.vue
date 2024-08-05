<script setup lang="ts">

const props = defineProps<{ amount: number }>();
</script>

<template>
  <div class="w-full h-full overflow-hidden relative rounded-2xl bg-primary-300">
    <div class="absolute top-0 left-0 w-full h-full z-10">
      <div class="p-2 h-full overflow-clip">
        <div class="w-80 rotate-45 -ml-20 -mt-10">
          <p class="text-primary-600 font-bold text-2xl break-words opacity-25">
            $$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
          </p>
        </div>
      </div>
    </div>
    <div class="absolute flex justify-center items-center w-full h-full z-20">
      <div class="p-2 rounded-lg bg-secondary flex justify-center items-center border-white border-solid border-2">
        <p class="text-white font-bold text-[50px]">${{ props.amount / 1000 }}<span
            class="text-white font-bold text-2xl">, 000</span></p>
      </div>
    </div>
  </div>
</template>