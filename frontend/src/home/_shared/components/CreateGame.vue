<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import Form from '@/_shared/components/forms/Form.vue';
import FormField from '@/_shared/components/forms/FormField.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
import Loader from '@/_shared/components/Loader.vue';
import { Key, type SignalRClient } from '@/_shared/providers/signalRClient';
import { useForm } from 'vee-validate';
import { inject, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import { object, string } from 'yup';

const router = useRouter();
const signalRClient = inject<SignalRClient>(Key);
const gameId = ref<string | undefined>();
const submitted = ref(false);

const { meta, handleSubmit } = useForm({
  validationSchema: object({
    name: string()
      .required("Name is required")
      .min(2, "Name must have more than 2 characters")
      .max(20, "Name must be less than 20 characters"),
  })
})

const onSubmit = handleSubmit(values => {
  submitted.value = true;
  signalRClient?.handleLobbyCreated(id => {
    gameId.value = id;
  })
  signalRClient?.createLobby(values.name);
})

watch(gameId, () => {
  router.push(`/lobby/${gameId.value}`);
})

</script>

<template>
  <Loader v-if="submitted" />
  <Form v-else @submit="onSubmit">
    <FormField>
      <TextInput name="name" type="text" label="Name" placeholder="Name" />
    </FormField>
    <Button :disabled="!meta.valid">Create Game</Button>
  </Form>
</template>