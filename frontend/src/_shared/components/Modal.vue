<template>
  <div class="z-20 w-screen h-screen fixed">
    <div class="relative w-full h-full">
      <div class="absolute top-0 left-0 w-full h-full bg-gray-900 opacity-70" />
      <div class="w-full h-full flex items-center justify-center">
        <slot />
      </div>
    </div>
  </div>
</template>