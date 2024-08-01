<script setup lang="ts">
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

onBeforeMount(() => {
  router.push("/home");
})

</script>
<template>
  <div />
</template>