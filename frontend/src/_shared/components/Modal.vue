<template>
  <div class="z-20 w-screen h-screen fixed top-0 left-0 translate-x-0">
    <div class="relative w-full h-full">
      <div class="absolute top-0 left-0 w-full h-full bg-gray-900 opacity-70" />
      <div class="absolute w-full h-full flex items-center justify-center z-10">
        <slot />
      </div>
    </div>
  </div>
</template>