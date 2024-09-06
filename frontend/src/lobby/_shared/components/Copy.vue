<script setup lang="ts">
import { PhCopy } from '@phosphor-icons/vue';
import { ref } from 'vue';

const props = defineProps<{
  value: string;
}>();

const copies = ref(0);

const handleCopy = () => {
  navigator.clipboard.writeText(props.value);
  copies.value++;
};

</script>

<template>
  <div class="flex gap-2 items-center">
    <PhCopy size="24" class="hover:cursor-pointer" @click="handleCopy" />
    <div class="relative">
      <div v-if="copies > 0" :key="copies"
        class="absolute top-[50%] left-[50%] -translate-y-[50%] text-primary-white text-sm bg-black rounded-lg py-2 px-3 animate-fade animate-reverse animate-delay-1000">
        <p class="whitespace-nowrap">Copied to clipboard</p>
      </div>
    </div>
  </div>
</template>