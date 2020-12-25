import Vue from 'vue';
import Vuex from 'vuex';
import camelCase from 'lodash/camelCase';

Vue.use(Vuex);

const requireModule = require.context('.', false, /\.js$/);
const modules = {};

requireModule.keys().forEach((fileName) => {
  if (fileName === './index.js') return;
  const moduleName = camelCase(fileName.replace(/(\.\/|\.js)/g, ''));
  modules[moduleName] = {
    ...requireModule(fileName).default,
  };
});

export default new Vuex.Store({
  modules,
});
