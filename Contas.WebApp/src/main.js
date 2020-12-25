import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

import Vue from 'vue';
import vueLodash from 'vue-lodash';
import lodash from 'lodash';
import vueMoment from 'vue-moment';
import moment from 'moment-timezone';
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';
import vueTheMask from 'vue-the-mask';
import upperFirst from 'lodash/upperFirst';
import camelCase from 'lodash/camelCase';
import vmodal from 'vue-js-modal';
import money from 'v-money';
import idleVue from 'idle-vue';
import { extend } from 'vee-validate';
import * as rules from 'vee-validate/dist/rules';
import vSelect from '@alfsnd/vue-bootstrap-select';
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faChess,
  faLandmark,
  faUser,
  faSearch,
  faPen,
  faTimes,
  faPlus,
  faRobot,
  faWallet,
  faClipboardList,
  faChartLine,
  faSignOutAlt,
  faPoll,
  faChartPie,
  faTrashAlt,
  faSyncAlt,
  faDownload,
  faFileDownload,
  faPlay,
  faStop,
} from '@fortawesome/free-solid-svg-icons';
import { faListAlt } from '@fortawesome/free-regular-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

import vueNotice from './vue-notice';
import coreMixin from './mixins/core';

import App from './App.vue';
import router from './router';
import store from './store';

library.add(faPlay);
library.add(faStop);
library.add(faDownload);
library.add(faFileDownload);
library.add(faSyncAlt);
library.add(faTrashAlt);
library.add(faChartPie);
library.add(faPoll);
library.add(faChess);
library.add(faLandmark);
library.add(faUser);
library.add(faListAlt);
library.add(faSearch);
library.add(faPen);
library.add(faTimes);
library.add(faPlus);
library.add(faRobot);
library.add(faWallet);
library.add(faClipboardList);
library.add(faChartLine);
library.add(faSignOutAlt);

Vue.component('font-awesome-icon', FontAwesomeIcon);

Vue.config.productionTip = false;

Vue.use(vueTheMask);
Vue.use(vueLodash, { name: 'custom', lodash });
Vue.use(vueNotice);
Vue.use(vueMoment, {
  moment,
});
Vue.use(BootstrapVue);
Vue.use(IconsPlugin);
Vue.use(vmodal, { dialog: true, title: 'Contas WebApp' });
Vue.use(money, {
  decimal: ',',
  thousands: '.',
  prefix: 'R$ ',
  suffix: '',
  precision: 2,
  masked: false,
});

const eventsHub = new Vue();

Vue.use(idleVue, {
  eventEmitter: eventsHub,
  idleTime: 300000,
});

const requireComponent = require.context(
  './components',
  // Whether or not to look in subfolders
  false,
  // The regular expression used to match base component filenames
  /Base[A-Z]\w+\.(vue|js)$/,
);

requireComponent.keys().forEach((fileName) => {
  // Get component config
  const componentConfig = requireComponent(fileName);

  // Get PascalCase name of component
  const componentName = upperFirst(
    camelCase(
    // Strip the leading `'./` and extension from the filename
      fileName.replace(/^\.\/(.*)\.\w+$/, '$1'),
    ),
  );

  // Register component globally)
  Vue.component(componentName, componentConfig.default || componentConfig);
});

Vue.mixin(coreMixin);

Vue.component('v-select', vSelect);

Object.keys(rules).forEach((rule) => {
  extend(rule, rules[rule]);
});

new Vue({
  mixins: [coreMixin],
  components: {
    App,
  },
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');
