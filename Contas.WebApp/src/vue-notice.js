import Noty from 'noty';

const options = {
  layout: 'topRight',
  theme: 'bootstrap-v4',
  timeout: 5000,
};

export default {
  install: (Vue, opts) => {
    // eslint-disable-next-line no-param-reassign
    Vue.prototype.$notice = (data) => new Noty(Object.assign(options, opts, data)).show();
  },
};
