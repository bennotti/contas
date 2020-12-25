const APP_INICIANDO = 'APP_INICIANDO';
const APP_STARTED = 'APP_STARTED';

const MODAL_LOADING = 'MODAL_LOADING';
const MODAL_LOADED = 'MODAL_LOADED';
const LOADING = 'LOADING';
const LOADED = 'LOADED';
const AUTH_LOADING = 'AUTH_LOADING';
const AUTH_LOADED = 'AUTH_LOADED';

const states = {
  isModalLoading: false,
  isLoading: true,
  isAuthLoading: true,
  isAppStarted: false,
};

const mutations = {
  [MODAL_LOADED](state) {
    state.isModalLoading = false;
  },
  [MODAL_LOADING](state) {
    state.isModalLoading = true;
  },
  [LOADING](state) {
    state.isLoading = true;
  },
  [LOADED](state) {
    state.isLoading = false;
  },
  [AUTH_LOADING](state) {
    state.isAuthLoading = true;
  },
  [AUTH_LOADED](state) {
    state.isAuthLoading = false;
  },
  [APP_INICIANDO]() {
  },
  [APP_STARTED](state) {
    state.isAppStarted = true;
  },
};

const actions = {
  [APP_INICIANDO]({ commit }) {
    commit('LOADING');
  },
  [MODAL_LOADING]({ state, commit }) {
    if (!state.isModalLoading) commit(MODAL_LOADING);
  },
  [MODAL_LOADED]({ state, commit }) {
    if (state.isModalLoading) commit(MODAL_LOADED);
  },
  [LOADING]({ state, commit }) {
    if (!state.isLoading) commit(LOADING);
  },
  [APP_STARTED]({ commit }) {
    commit(APP_STARTED);
  },
  [LOADED]({ commit }) {
    commit(LOADED);
  },
  [AUTH_LOADING]({ state, commit }) {
    if (!state.isAuthLoading) commit(AUTH_LOADING);
  },
  [AUTH_LOADED]({ state, commit }) {
    if (state.isAuthLoading) commit(AUTH_LOADED);
  },
};

const getters = {
  isAppStarted(st) {
    return st.isAppStarted;
  },
  isModalLoading(st) {
    return st.isModalLoading;
  },
  isAuthLoading(st) {
    return st.isAuthLoading;
  },
  isLoading(st) {
    return st.isLoading || st.isAuthLoading;
  },
};

export default {
  state: states,
  actions,
  mutations,
  getters,
};
