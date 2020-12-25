<template>
  <div>Splash</div>
</template>

<script>
// @ is an alias to /src
// import HelloWorld from '@/components/HelloWorld.vue';

/* verificar se esta logado */

export default {
  name: 'SplashScreen',
  created() {
    // verificar se o usuario esta logado
    this.$router.push({ name: 'App' });
  },
};
</script>
