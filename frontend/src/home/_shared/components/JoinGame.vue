<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import Form from '@/_shared/components/forms/Form.vue';
import FormField from '@/_shared/components/forms/FormField.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
import ValidationError from '@/_shared/components/forms/ValidationError.vue';
import { Key as SignalRKey, type SignalRClient } from '@/_shared/providers/signalRClient';
import { useForm } from 'vee-validate';
import { inject, ref } from 'vue';
import { useRouter } from 'vue-router';
import { object, string } from 'yup';

const signalRClient = inject<SignalRClient>(SignalRKey);
const router = useRouter();
const joinGameError = ref<string | undefined>();

const { meta, handleSubmit } = useForm({
  validationSchema: object({
    gameId: string()
      .required("Game ID is required")
      .length(5, "Game ID must be 5 characters in length")
  })
})

const onSubmit = handleSubmit(async (values, _) => {
  router.push(`/lobby/${values.gameId}`);
});

</script>

<template>
  <Form @submit="onSubmit">
    <FormField>
      <TextInput name="gameId" type="text" label="Game ID" placeholder="BCDFG" />
    </FormField>
    <Button :disabled="!meta.valid">Join Game</Button>
    <ValidationError>{{ joinGameError }}</ValidationError>
  </Form>
</template>