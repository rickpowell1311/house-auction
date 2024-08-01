<script setup lang="ts">
import JumpIn from '@/_shared/components/animations/JumpIn.vue';
import Button from '@/_shared/components/Button.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
</script>

<template>
  <JumpIn>
    <div class="flex flex-col gap-4 items-start">
      <TextInput name="name" type="text" label="Name" place-holder="Name" />
      <Button>Create Game</Button>
    </div>
  </JumpIn>
</template>