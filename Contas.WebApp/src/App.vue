<template>
<div id="app">
  <v-dialog :clickToClose="false"/>
  <div id="app-loading" class="loading" v-if="isLoading">Loading&#8230;</div>
  <div id="app-content" v-if="!isLoading">
    <router-view></router-view>
  </div>
</div>
</template>

<script>
import { mapGetters } from 'vuex';

export default {
  data() {
    return {
    };
  },
  computed: {
    ...mapGetters([
      'isLoading',
    ]),
  },
  created() {
  },
  ready() {
  },
};
</script>

<style>
  @import './assets/css/app.css';
  @import './assets/css/modal.css';
  @import './assets/css/loading.css';
  @import './assets/css/noty.css';
  @import './assets/css/viewCard.css';
  @import './assets/css/v-select.css';

html, body {
    height: 100%;
}

#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: #2c3e50;
  height: 100%;
  min-height: 100%;
  /*margin-top: 60px;*/
}

#app-content{
  height: 100%;
  min-height: 100%;
}
</style>
