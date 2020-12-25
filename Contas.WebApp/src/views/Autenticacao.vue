<template>
  <div class="container-fluid h-100">
        <div class="row no-gutter h-100">
            <div class="d-none d-md-flex col-md-4 col-lg-6 bg-image"></div>
            <div class="col-md-8 col-lg-6">
              <div class="login d-flex">
                <div class="container">
                  <div class="row">
                      <div class="col-md-9 col-lg-8 mx-auto">
                        <div class="text-center"><img src="/static/imgs/logo.png" class="ml-auto mr-auto mb-5 mt-5"/></div>
                        <h3 class="login-heading mb-4 text-center">Bem-vindo de volta!</h3>
                        <ValidationObserver v-slot="{ invalid }">
                        <form id="formLogin" name="formLogin" ref="formLogin" @submit.prevent="login">

                          <ValidationProvider name="usuario" rules="required" v-slot="{ valid }">
                          <div class="form-label-group">
                            <label for="usuario">Usuario</label>
                            <input type="text" v-model="usuario.usuario" id="usuario" name="usuario" class="form-control" v-bind:class="{ 'border border-danger': !valid, 'border-success': valid }" placeholder="Usuário de acesso" required autofocus/>
                          </div>
                          </ValidationProvider>

                          <ValidationProvider name="senha" rules="required" v-slot="{ valid }">
                          <div class="form-label-group mt-2">
                            <label for="senha">Senha</label>
                            <input type="password" v-model="usuario.senha" id="senha" name="senha" class="form-control" v-bind:class="{ 'border border-danger': !valid, 'border-success': valid }" placeholder="Senha de acesso" required>
                          </div>
                          </ValidationProvider>

                          <button type="submit" class="mt-3 btn btn-lg btn-primary btn-block btn-login text-uppercase font-weight-bold mb-2"  :disabled="invalid">Entrar</button>
                        </form>
                        </ValidationObserver>
                      </div>
                  </div>
                </div>
              </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ValidationObserver, ValidationProvider } from 'vee-validate';
import _autenticacaoService from '@/services/AutenticacaoService';

export default {
  components: { ValidationObserver, ValidationProvider },
  data() {
    return {
      usuario: {
        email: '',
        senha: '',
      },
    };
  },
  methods: {
    login() {
      const self = this;
      _autenticacaoService.login(self.usuario).then(() => {
        self.$router.push({ name: 'Contas' });
      }, () => {
        self.usuario.senha = '';
        self.modalAlert('Login Inválido');
      });
    },
  },
};
</script>
