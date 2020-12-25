import store from '@/store';

export default {
  iniciando() {
    return new Promise((resolve) => {
      if (store.getters.isAppStarted) {
        resolve({
          isAppStarted: store.getters.isAppStarted,
          isAuth: store.getters.isAuth,
          token: store.getters.tokenAuth,
          usuarioLogado: store.getters.usuarioLogado,
        });
        return;
      }

      store.dispatch('APP_INICIANDO');

      store.dispatch('AUTH_LOAD_LOCALSTORE').then((response) => {
        store.dispatch('AUTH_LOADED');
        resolve({
          isAppStarted: store.getters.isAppStarted,
          isAuth: store.getters.isAuth,
          token: response.token,
          usuarioLogado: response.usuarioLogado,
        });
      }, () => {
        store.dispatch('LOGOUT').then(() => {
          resolve(null);
        });
      });
    });
  },
  carregado() {
    return Promise.resolve().then(() => {
      store.dispatch('APP_STARTED');
      store.dispatch('LOADED');
    });
  },
};
