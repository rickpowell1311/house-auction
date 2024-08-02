<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import Form from '@/_shared/components/forms/Form.vue';
import FormField from '@/_shared/components/forms/FormField.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
import { Key as SignalRKey, type SignalRClient } from '@/_shared/providers/signalRClient';
import { useForm } from 'vee-validate';
import { inject } from 'vue';
import { useRouter } from 'vue-router';
import { object, string } from 'yup';

const router = useRouter();
const signalRClient = inject<SignalRClient>(SignalRKey);

const { meta, handleSubmit } = useForm({
  validationSchema: object({
    name: string()
      .required("Name is required")
      .min(2, "Name must have more than 2 characters")
      .max(20, "Name must be less than 20 characters"),
  })
})

const onSubmit = handleSubmit(async (values, ctx) => {
  try {
    const gameId = await signalRClient?.hub.createLobby(values.name);
    router.push(`/lobby/${gameId}`);
  } catch (err) {
    console.log(`Unable to create game: ${err}`);
    ctx.evt?.preventDefault();
    return false;
  }
})

</script>

<template>
  <Form @submit="onSubmit">
    <FormField>
      <TextInput name="name" type="text" label="Name" placeholder="Name" />
    </FormField>
    <Button :disabled="!meta.valid">Create Game</Button>
  </Form>
</template>