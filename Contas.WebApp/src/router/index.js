import Vue from 'vue';
import VueRouter from 'vue-router';
import _ from 'lodash';
import LayoutApp from '@/views/LayoutApp.vue';

import _appService from '@/services/AppService';
import _autenticacaoService from '@/services/AutenticacaoService';

Vue.use(VueRouter);

const appLauch = (to, from, next) => {
  const callbackCarregado = () => {
    _appService.carregado();
    _autenticacaoService.authLoaded().then(() => {
      next();
    });
  };
  _appService.iniciando().then(callbackCarregado, callbackCarregado);
};

const appLauchWithCheckAuth = (to, from, next) => {
  const callbackCarregado = (response) => {
    if (_.isNull(response)) {
      _appService.carregado();
      next({ name: 'Login' });
      return;
    }
    if (!response.isAppStarted) {
      _appService.carregado();
      if (!response.isAuth) next({ name: 'Login' });
      else if (response.usuarioLogado.alterarSenha) {
        if (to.name === 'AlterarSenha') next();
        else next({ name: 'AlterarSenha' });
      } else next();
      return;
    }

    _autenticacaoService.check().then(() => {
      if (response.usuarioLogado.alterarSenha) {
        if (to.name === 'AlterarSenha') next();
        else next({ name: 'AlterarSenha' });
      } else next();
    }, () => {
      next({ name: 'Login' });
    });
  };
  _appService.iniciando().then(callbackCarregado, callbackCarregado);
};

const routes = [
  {
    path: '/login',
    name: 'Login',
    beforeEnter: appLauch,
    component: () => import(/* webpackChunkName: "login" */ '@/views/Autenticacao.vue'),
  },
  {
    path: '/',
    redirect: '/contas',
    name: 'App',
    beforeEnter: appLauchWithCheckAuth,
    component: LayoutApp,
    children: [
      {
        path: 'contas',
        name: 'Contas',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Contas/Index.vue'),
      },
      {
        path: 'contas/adicionar',
        name: 'ContasAdicionar',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Contas/Form.vue'),
      },
      {
        path: 'contas/pesquisar',
        name: 'ContasPesquisar',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Contas/Pesquisar.vue'),
      },
      {
        path: 'contas/:contaId/editar',
        name: 'ContasEditar',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Contas/Form.vue'),
      },
      {
        path: 'contas/:contaId',
        name: 'ContasVisualizar',
        component: () => import(/* webpackChunkName: "dashboard" */ '@/views/Contas/Visualizar.vue'),
      },
    ],
  },
];

const router = new VueRouter({
  routes,
  linkActiveClass: 'active', // active class for non-exact links.
  linkExactActiveClass: 'active', // active class for *exact* links.
});

export default router;
