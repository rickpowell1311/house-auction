<script setup lang="ts">
import Back from "@/_shared/components/Back.vue";
import { ref } from "vue";
import { useRouter } from "vue-router";
import JumpIn from "../_shared/components/animations/JumpIn.vue";
import Button from "../_shared/components/Button.vue";
import Main from "../_shared/components/layout/Main.vue";
import CreateLobby from "./_shared/components/CreateLobby.vue";
import JoinLobby from "./_shared/components/JoinLobby.vue";

const router = useRouter();

const createGame = () => {
  router.push("/game/123");
};

const workflow = ref("landing" as "landing" | "creating" | "joining");

</script>
<template>
  <div class="h-1/6" />
  <JumpIn>
    <div class="container mx-auto">
      <Main>
        <div class="flex justify-center">
          <div class="flex flex-col items-center justify-center gap-16">
            <h1 class="text-center text-shadow-sm shadow-gray-300">House Auction</h1>
            <div class="flex flex-col gap-8 items-start">
              <Back v-if="workflow !== 'landing'" @click="() => workflow = 'landing'" />
              <template v-if="workflow === 'landing'">
                <div class="flex flex-col gap-4 items-start">
                  <Button @click="() => workflow = 'creating'">New Game</Button>
                  <Button @click="() => workflow = 'joining'">Join Game</Button>
                </div>
              </template>
              <template v-else-if="workflow === 'creating'">
                <CreateLobby />
              </template>
              <template v-else-if="workflow === 'joining'">
                <JoinLobby />
              </template>
            </div>
          </div>
        </div>
      </Main>
    </div>
  </JumpIn>
</template>