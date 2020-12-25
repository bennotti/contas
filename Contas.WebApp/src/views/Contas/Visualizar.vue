<template>
<div>
  <nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><router-link tag="a" v-bind:to="{ name: 'Contas', params: { }}">Contas</router-link></li>
    <li class="breadcrumb-item active" aria-current="page">Visualizar</li>
  </ol>
</nav>
<div class="text-center" v-if="carregandoDados"><img src="/static/imgs/loading.gif" /></div>
  <form id="formConta" name="formConta" ref="formConta" v-if="!carregandoDados">
    <div class="form-group row">
    <div class="col-sm-10">
      <router-link tag="a" class="btn btn-outline-info ml-2" v-bind:to="{ name: 'ContasEditar', params: { contaId: contaId }}">Editar</router-link>
      <router-link tag="a" class="btn btn-outline-danger ml-2" v-bind:to="{ name: 'Contas', params: { }}">Voltar</router-link>
    </div>
  </div>

  <div class="form-group row">
    <label for="nome" class="col-sm-2 col-form-label">Nome</label>
    <div class="col-sm-10">
      <input type="text" id="nome" name="nome" v-model="dados.nome" placeholder="Nome da conta" class="form-control-plaintext" readonly/>
    </div>
  </div>
  <div class="form-group row">
    <label for="valorOriginal" class="col-sm-2 col-form-label">Valor original</label>
    <div class="col-sm-10">
      <money name="valorOriginal" v-model="dados.valorOriginal" placeholder="R$ 0,00" class="form-control-plaintext" readonly></money>
    </div>
  </div>
  <div class="form-group row">
    <label for="vencimento" class="col-sm-2 col-form-label">Vencimento</label>
    <div class="col-sm-10">
      <the-mask :mask="'##/##/####'" :masked="true" name="vencimento" v-model="dados.vencimento" class="form-control-plaintext" readonly/>
    </div>
  </div>
  <div class="form-group row">
    <label for="pagamento" class="col-sm-2 col-form-label">Pagamento</label>
    <div class="col-sm-10">
      <the-mask :mask="'##/##/####'" :masked="true" name="pagamento" v-model="dados.pagamento" class="form-control-plaintext" readonly/>
    </div>
  </div>
  <div class="form-group row">
    <label for="diasAtrasado" class="col-sm-2 col-form-label">Dias em atraso</label>
    <div class="col-sm-10">
      <input type="text" :disabled="true" name="diasAtrasado" v-model="diasAtrasado" placeholder="0 dia(s)" class="form-control-plaintext" readonly/>
    </div>
  </div>
  <div class="form-group row">
    <label for="valorMulta" class="col-sm-2 col-form-label">Valor da multa</label>
    <div class="col-sm-10">
      <money :disabled="true" name="valorMulta" v-model="valorMulta" placeholder="R$ 0,00" class="form-control-plaintext" readonly></money>
    </div>
  </div>
  <div class="form-group row">
    <label for="valorFinal" class="col-sm-2 col-form-label">Valor final</label>
    <div class="col-sm-10">
      <money :disabled="true" name="valorFinal" v-model="valorFinal" placeholder="R$ 0,00" class="form-control-plaintext" readonly></money>
    </div>
  </div>

  <div class="form-group row">
    <div class="col-sm-10">
      <router-link tag="a" class="btn btn-outline-info ml-2" v-bind:to="{ name: 'ContasEditar', params: { contaId: contaId }}">Editar</router-link>
      <router-link tag="a" class="btn btn-outline-danger ml-2" v-bind:to="{ name: 'Contas', params: { }}">Voltar</router-link>
    </div>
  </div>
</form>
</div>
</template>

<script>
import { mapGetters } from 'vuex';

import ContasService from '@/services/ContasServices';

export default {
  data() {
    return {
      contaId: 0,
      carregandoDados: false,
      diasAtrasado: 0,
      valorMulta: 0,
      valorFinal: 0,
      dados: {
        nome: '',
        valorOriginal: 0,
        vencimento: '',
        pagamento: '',
      },
    };
  },
  computed: {
    ...mapGetters([
      'usuarioLogado',
      'isAuthLoading',
    ]),
  },
  methods: {
    carregarDados() {
      const self = this;
      const onError = () => {
        self.modalAlert('Não foi possivel carregar os dados');
        self.carregandoDados = false;
      };

      const onSuccess = (res) => {
        self.carregandoDados = false;
        self.dados = res;
        self.diasAtrasado = res.qntDiasAtraso;
        self.valorMulta = res.valorMulta;
        self.valorFinal = res.valorFinal;
      };
      self.carregandoDados = true;
      ContasService.obterPorId(self.contaId).then(onSuccess, onError);
    },
  },
  created() {
    // if (!this.usuarioLogado) return;
    if (this.$route.params.contaId != null) {
      this.contaId = this.$route.params.contaId;
      this.carregarDados();
    } else {
      this.$router.push({ name: 'Contas' });
    }
  },
};
</script>
