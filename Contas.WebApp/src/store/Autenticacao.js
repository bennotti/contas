import axios from 'axios';
import _ from 'lodash';
import autenticacaoApi from '@/api/autenticacao';

const LOGIN = 'LOGIN';
const LOGIN_CHECK = 'LOGIN_CHECK';
const LOGIN_SUCCESS = 'LOGIN_SUCCESS';
const LOGOUT = 'LOGOUT';
const LOAD_USER_PROFILE_SUCCESS = 'LOAD_USER_PROFILE_SUCCESS';
const AUTH_LOAD_LOCALSTORE = 'AUTH_LOAD_LOCALSTORE';
const AUTH_TOKEN = 'AUTH_TOKEN';
const ESQUECEU_SENHA = 'ESQUECEU_SENHA';

const asyncLocalStorage = {
  setItem(key, value) {
    return Promise.resolve().then(() => {
      localStorage.setItem(key, window.btoa(value));
    });
  },
  getItem(key) {
    return Promise.resolve().then(() => {
      const data = localStorage.getItem(key);
      if (_.isEmpty(data) || _.isNull(data)) return '';
      return window.atob(data);
    });
  },
  remove(key) {
    return Promise.resolve().then(() => {
      localStorage.removeItem(key);
    });
  },
};

const states = {
  usuarioLogado: null,
  isLoginCheck: false,
  tokenAuth: null,
  lastCheck: null,
};
const mutations = {
  [LOGIN_SUCCESS](state, payload) {
    state.tokenAuth = payload.token;
  },
  [LOGIN_CHECK](state, status) {
    state.isLoginCheck = status;
  },
  [LOAD_USER_PROFILE_SUCCESS](state, payload) {
    state.usuarioLogado = payload.usuarioLogado;
  },
  [LOGOUT](state) {
    state.lastCheck = null;
    state.usuarioLogado = null;
    state.tokenAuth = null;
  },
};
const actions = {
  [AUTH_LOAD_LOCALSTORE]({ commit }) {
    return new Promise((resolve, reject) => {
      asyncLocalStorage.getItem('token').then((token) => {
        if (_.isNull(token) || _.isEmpty(token)) {
          reject();
          return;
        }
        commit(LOGIN_SUCCESS, { token });
        axios.defaults.headers.common.Authorization = token;
        commit(LOGIN_CHECK, true);
        autenticacaoApi.meusDados(token).then((usuarioLogado) => {
          commit(LOAD_USER_PROFILE_SUCCESS, { usuarioLogado });
          commit(LOGIN_CHECK, false);
          resolve({ token, usuarioLogado });
        }, (error) => {
          reject(error);
        });
      });
    });
  },
  [LOGOUT]({ state, commit }) {
    return new Promise((resolve) => {
      if (_.isNull(state.tokenAuth) || _.isEmpty(state.tokenAuth)) {
        resolve();
        return;
      }

      const logoutCallback = () => {
        asyncLocalStorage.remove('token').then(() => {
          axios.defaults.headers.common.Authorization = null;
          commit(LOGOUT);
          resolve();
        });
      };

      autenticacaoApi.logout().then(logoutCallback, logoutCallback);
    });
  },
  [ESQUECEU_SENHA]({ }, user) {
    return new Promise((resolve, reject) => {
      autenticacaoApi.esqueceuSenha(user).then(() => {
        resolve();
      }, (error) => {
        reject(error);
      });
    });
  },
  [LOGIN]({ commit }, user) {
    return new Promise((resolve, reject) => {
      const loadUserProfile = (token) => {
        autenticacaoApi.meusDados(token).then((usuarioLogado) => {
          commit(LOAD_USER_PROFILE_SUCCESS, { usuarioLogado });
          commit(LOGIN_CHECK, false);
          resolve({ token, usuarioLogado });
        }, (error) => {
          commit(LOGOUT);
          reject(error);
        });
      };

      autenticacaoApi.login(user).then((token) => {
        commit(LOGIN_SUCCESS, { token });
        axios.defaults.headers.common.Authorization = token;
        commit(LOGIN_CHECK, true);
        asyncLocalStorage.setItem('token', token).then(() => {
          loadUserProfile(token);
        });
      }, (error) => {
        commit(LOGOUT);
        reject(error);
      });
    });
  },
  [LOGIN_CHECK]({ state, commit }, token) {
    return new Promise((resolve, reject) => {
      let nToken = token;

      if (_.isEmpty(nToken) || _.isNull(nToken)) {
        nToken = state.tokenAuth;
      }

      if (_.isEmpty(nToken) || _.isNull(nToken)) {
        reject();
        return;
      }

      autenticacaoApi.meusDados(nToken).then((usuarioLogado) => {
        commit(LOAD_USER_PROFILE_SUCCESS, { usuarioLogado });
        resolve({ token: nToken, usuarioLogado });
      }, (error) => {
        reject(error);
      });
    });
  },
  [AUTH_TOKEN]({ }) {
    return new Promise((resolve) => {
      asyncLocalStorage.getItem('token').then((token) => {
        resolve(token);
      });
    });
  },
};

const getters = {
  isAuth(st) {
    return !_.isNull(st.usuarioLogado) && !_.isEmpty(st.usuarioLogado);
  },
  usuarioLogado(st) {
    return st.usuarioLogado;
  },
  tokenAuth(st) {
    return st.tokenAuth;
  },
};

export default {
  state: states,
  mutations,
  getters,
  actions,
};
