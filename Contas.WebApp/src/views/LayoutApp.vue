<template>
  <div class="">
    <nav class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap shadow">
      <router-link tag="a" class="navbar-brand col-md-3 col-lg-2 mr-0 px-3" v-bind:to="{ name: 'Contas', params: { }}">Contas</router-link>
      <button class="navbar-toggler position-absolute d-md-none collapsed" type="button" data-toggle="collapse" data-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <b-navbar-nav class="ml-auto">
            <b-nav-item-dropdown text='Meu perfil' right>
              <b-dropdown-item @click="logout()">Sair</b-dropdown-item>
            </b-nav-item-dropdown>
          </b-navbar-nav>
    </nav>
      <!-- The sidebar -->
      <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
      <div class="sidebar-sticky pt-3">
        <ul class="nav flex-column">
          <router-link tag="li" class="nav-item" v-bind:to="{ name: 'Contas', params: { }}">
            <a class="nav-link" href="javascript:void(0)">
              <font-awesome-icon icon="chart-line" />
              Contas
            </a>
          </router-link>
        </ul>
      </div>
    </nav>
    <main role="main" class="col-md-9 ml-sm-auto col-lg-10 px-md-4 pt-3">
      <router-view :key="$route.fullPath" v-if="!saindo"></router-view>
    </main>
  </div>
</template>

<script>
import AuthService from '@/services/AutenticacaoService';

export default {
  onActive() {
    const self = this;
    self.openModalAlertNoButtons('<p class="text-center"><img src="/static/imgs/loading.gif"/><br/>Validando usuário</p>');
    AuthService.check({ showLoadingPage: false }).then(() => {
      self.closeModalAlert();
    }, () => {
      self.closeModalAlert();
      self.$router.push({ name: 'Login' });
    });
  },
  data() {
    return {
      saindo: false,
      notificacoes: {
        lista: [],
        totalRegistros: 0,
      },
    };
  },
  computed: {
    usuarioLogado() {
      return this.$store.getters.usuarioLogado;
    },
    isAuth() {
      return this.$store.getters.isAuth;
    },
    isLocalLoading() {
      return this.$store.getters.isLocalLoading;
    },
  },
  methods: {
    logout() {
      const self = this;
      self.saindo = true;
      AuthService.logout().then(() => {
        self.$router.push({ name: 'Login' });
      }, () => {
        self.$router.push({ name: 'Login' });
      });
    },
  },
  created() {
    this.saindo = false;
  },
};
</script>
