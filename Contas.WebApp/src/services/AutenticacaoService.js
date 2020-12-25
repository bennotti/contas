import store from '@/store';
import _ from 'lodash';

export default {
  logout() {
    store.dispatch('AUTH_LOADING');
    return new Promise((resolve) => {
      const callback = () => {
        store.dispatch('AUTH_LOADED');
        resolve();
      };
      store.dispatch('LOGOUT').then(callback, callback);
    });
  },
  login(dadosUsuario) {
    store.dispatch('AUTH_LOADING');
    return new Promise((resolve, reject) => {
      const invalidLogin = () => {
        store.dispatch('AUTH_LOADED');
        reject(new Error('Login Inválido'));
      };

      store.dispatch('LOGIN', dadosUsuario).then((response) => {
        store.dispatch('AUTH_LOADED');
        resolve(response);
      }, invalidLogin);
    });
  },
  authLoaded() {
    return new Promise((resolve) => {
      store.dispatch('AUTH_LOADED');
      resolve();
    });
  },
  check(p) {
    const obj = _.defaults({}, { showLoadingPage: true }, p);
    if (obj.showLoadingPage) store.dispatch('AUTH_LOADING');
    return new Promise((resolve, reject) => {
      store.dispatch('AUTH_TOKEN').then((token) => {
        if (_.isNull(token) || _.isEmpty(token)) {
          reject();
          return;
        }

        store.dispatch('LOGIN_CHECK', token).then((response) => {
          store.dispatch('AUTH_LOADED');
          resolve(response);
        }, () => {
          const callback = () => {
            store.dispatch('AUTH_LOADED');
            reject(new Error('Login Inválido'));
          };
          store.dispatch('LOGOUT').then(callback, callback);
        });
      });
    });
  },
  obterUsuarioLogado() {
    return store.getters.obterUsuarioLogado;
  },
  isAuth() {
    return store.getters.isAuth;
  },
};
