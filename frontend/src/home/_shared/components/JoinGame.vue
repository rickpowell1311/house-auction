<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import Form from '@/_shared/components/forms/Form.vue';
import FormField from '@/_shared/components/forms/FormField.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
import ValidationError from '@/_shared/components/forms/ValidationError.vue';
import type { IHouseAuctionReceiver } from '@/_shared/providers/generated/TypedSignalR.Client/HouseAuction';
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
    name: string()
      .required("Name is required")
      .min(2, "Name must have more than 2 characters")
      .max(20, "Name must be less than 20 characters"),
    gameId: string()
      .required("Game ID is required")
      .length(5, "Game ID must be 5 characters in length")
  })
})

const onSubmit = handleSubmit(async (values, ctx) => {
  try {
    signalRClient?.subscribe({
      notifyError(message) {
        joinGameError.value = message;
      }
    } as IHouseAuctionReceiver)
    await signalRClient?.hub.joinLobby(values.gameId, values.name);
    router.push(`/lobby/${values.gameId}`);
  } catch (err) {
    console.log(`Unable to join game: ${err}`);
    ctx.evt?.preventDefault();
    return false;
  }
});

</script>

<template>
  <Form @submit="onSubmit">
    <FormField>
      <TextInput name="name" type="text" label="Name" placeholder="Name" />
    </FormField>
    <FormField>
      <TextInput name="gameId" type="text" label="Game ID" placeholder="BCDFG" />
    </FormField>
    <Button :disabled="!meta.valid">Join Game</Button>
    <ValidationError>{{ joinGameError }}</ValidationError>
  </Form>
</template>