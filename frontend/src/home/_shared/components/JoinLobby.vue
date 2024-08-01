<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
</script>

<template>
  <JumpIn>
    <div class="flex flex-col gap-4 items-start">
      <TextInput name="name" type="text" label="Name" place-holder="Name" />
      <TextInput name="game-code" type="text" label="Code" place-holder="BCDFG" />
      <Button>Create Game</Button>
    </div>
  </JumpIn>
</template>