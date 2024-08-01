<script setup lang="ts">
import Button from '@/_shared/components/Button.vue';
import Form from '@/_shared/components/forms/Form.vue';
import FormField from '@/_shared/components/forms/FormField.vue';
import TextInput from '@/_shared/components/forms/TextInput.vue';
import { useForm } from 'vee-validate';
import { object, string } from 'yup';

const onSubmit = (values: unknown) => {
  console.log(values);
}

const { meta } = useForm({
  validationSchema: object({
    name: string()
      .required("Name is required")
      .min(2, "Name must have more than 2 characters")
      .max(20, "Name must be less than 20 characters"),
    roomCode: string()
      .required("Room code is required")
      .length(5, "Room code must be 5 characters in length")
  })
})

</script>

<template>
  <Form @submit="onSubmit">
    <FormField>
      <TextInput name="name" type="text" label="Name" placeholder="Name" />
    </FormField>
    <FormField>
      <TextInput name="roomCode" type="text" label="Room Code" placeholder="BCDFG" />
    </FormField>
    <Button :disabled="!meta.valid">Join Game</Button>
  </Form>
</template>