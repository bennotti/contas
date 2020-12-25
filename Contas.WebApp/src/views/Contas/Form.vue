<template>
<div>
  <nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><router-link tag="a" v-bind:to="{ name: 'Contas', params: { }}">Contas</router-link></li>
    <li class="breadcrumb-item active" aria-current="page"><span v-if="editar">Alterar conta</span><span v-else>Nova Conta</span></li>
  </ol>
</nav>
<div class="text-center" v-if="carregandoDados"><img src="/static/imgs/loading.gif" /></div>
<ValidationObserver v-slot="{ invalid }">
  <form id="formConta" name="formConta" ref="formConta" @submit.prevent="salvar" v-if="!carregandoDados">
    <div class="form-group row">
    <div class="col-sm-10">
      <button type="button" class="btn btn-success pl-5 pr-5" :disabled="invalid || calculando || !calculado" v-on:click="continuar">Salvar e continuar</button>
      <button type="submit" class="btn btn-primary ml-2 pl-5 pr-5" :disabled="invalid || calculando || !calculado">Salvar e sair</button>
      <button type="reset" class="btn btn-outline-secondary ml-2">Limpar</button>
      <router-link tag="a" class="btn btn-outline-danger ml-2" v-bind:to="{ name: 'Contas', params: { }}">Voltar</router-link>
    </div>
  </div>

  <ValidationProvider name="nome" rules="required" v-slot="{ valid }">
  <div class="form-group row">
    <label for="nome" class="col-sm-2 col-form-label">Nome</label>
    <div class="col-sm-10">
      <input type="text" id="nome" name="nome" v-model="dados.nome" placeholder="Nome da conta" class="form-control" v-bind:class="{ 'border border-danger': !valid, 'border-success': valid }" required/>
    </div>
  </div>
  </ValidationProvider>
  <ValidationProvider name="valorOriginal" rules="required" v-slot="{ valid }">
  <div class="form-group row">
    <label for="valorOriginal" class="col-sm-2 col-form-label">Valor original</label>
    <div class="col-sm-10">
      <money name="valorOriginal" v-model="dados.valorOriginal" @blur.native="calcularMulta" placeholder="R$ 0,00" v-bind:class="{ 'border border-danger': !valid, 'border-success': valid }" class="form-control" required></money>
    </div>
  </div>
  </ValidationProvider>
  <ValidationProvider name="vencimento" rules="required" v-slot="{ valid }">
  <div class="form-group row">
    <label for="vencimento" class="col-sm-2 col-form-label">Vencimento</label>
    <div class="col-sm-10">
      <the-mask :mask="'##/##/####'" :masked="true" @blur.native="calcularMulta" name="vencimento" v-model="dados.vencimento" v-bind:class="{ 'border border-danger': !valid, 'border-success': valid }" class="form-control" required/>
    </div>
  </div>
  </ValidationProvider>
  <ValidationProvider name="pagamento" rules="required" v-slot="{ valid }">
  <div class="form-group row">
    <label for="pagamento" class="col-sm-2 col-form-label">Pagamento</label>
    <div class="col-sm-10">
      <the-mask :mask="'##/##/####'" :masked="true" @blur.native="calcularMulta" name="pagamento" v-model="dados.pagamento" v-bind:class="{ 'border border-danger': !valid, 'border-success': valid }" class="form-control"/>
    </div>
  </div>
  </ValidationProvider>
  <div class="form-group row" v-if="calculando">
    <label for="calculando" class="col-sm-2 col-form-label">Calculando</label>
    <div class="col-sm-10">
      <img src="/static/imgs/loading.gif" />
    </div>
  </div>
  <div class="form-group row">
    <label for="diasAtrasado" class="col-sm-2 col-form-label">Dias em atraso</label>
    <div class="col-sm-10">
      <input type="text" :disabled="true" name="diasAtrasado" v-model="diasAtrasado" placeholder="0 dia(s)" class="form-control"/>
    </div>
  </div>
  <div class="form-group row">
    <label for="valorMulta" class="col-sm-2 col-form-label">Valor da multa</label>
    <div class="col-sm-10">
      <money :disabled="true" name="valorMulta" v-model="valorMulta" placeholder="R$ 0,00" class="form-control"></money>
    </div>
  </div>
  <div class="form-group row">
    <label for="valorFinal" class="col-sm-2 col-form-label">Valor final</label>
    <div class="col-sm-10">
      <money :disabled="true" name="valorFinal" v-model="valorFinal" placeholder="R$ 0,00" class="form-control"></money>
    </div>
  </div>

  <div class="form-group row">
    <div class="col-sm-10">
      <button type="button" class="btn btn-success pl-5 pr-5" :disabled="invalid || calculando || !calculado" v-on:click="continuar">Salvar e continuar</button>
      <button type="submit" class="btn btn-primary ml-2 pl-5 pr-5" :disabled="invalid || calculando || !calculado">Salvar e sair</button>
      <button type="reset" class="btn btn-outline-secondary ml-2">Limpar</button>
      <router-link tag="a" class="btn btn-outline-danger ml-2" v-bind:to="{ name: 'Contas', params: { }}">Voltar</router-link>
    </div>
  </div>
</form>
</ValidationObserver>
</div>
</template>

<script>
import { ValidationObserver, ValidationProvider } from 'vee-validate';
import { mapGetters } from 'vuex';

import ContasService from '@/services/ContasServices';

export default {
  components: { ValidationObserver, ValidationProvider },
  data() {
    return {
      contaId: 0,
      editar: false,
      carregandoDados: false,
      continuarNoForm: false,
      atrasado: false,
      carregando: true,
      calculando: false,
      calculado: false,
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
    calcularMulta() {
      const self = this;
      if (self.dados.valorOriginal > 0 && self.dados.vencimento !== '' && self.dados.pagamento !== '') {
        self.calculando = true;
        self.calculado = false;
        ContasService.calcularMulta(self.dados).then((res) => {
          self.diasAtrasado = res.qntDiasAtraso;
          self.valorMulta = res.valorMulta;
          self.valorFinal = res.valorFinal;
          self.calculando = false;
          self.calculado = true;
        }, () => {
          self.$notice({
            type: 'error', // alert, success, warning, error, info/information
            text: 'Não foi possível calcular o valor da multa.',
          });
          self.calculando = false;
          self.calculado = false;
        });
      } else {
        self.calculado = false;
        self.calculando = false;
        self.diasAtrasado = 0;
        self.valorMulta = 0;
        self.valorFinal = self.dados.valorOriginal;
      }
    },
    continuar() {
      this.continuarNoForm = true;
      this.salvar();
    },
    salvar() {
      const self = this;

      const onError = () => {
        self.modalAlert('Não foi possivel salvar os dados');
        self.carregandoDados = false;
        self.continuarNoForm = false;
      };

      const onSuccess = () => {
        if (!self.continuarNoForm) {
          self.$router.push({ name: 'Contas' });
          return;
        }

        self.modalAlert('Conta cadastrada com sucesso!');

        self.editar = false;
        self.dados = {
          nome: '',
          valorOriginal: 0,
          vencimento: '',
          pagamento: '',
        };
        self.calculado = false;
        self.calculando = false;
        self.diasAtrasado = 0;
        self.valorMulta = 0;
        self.valorFinal = self.dados.valorOriginal;
        self.carregandoDados = false;
        self.continuarNoForm = false;
      };
      self.carregandoDados = true;
      if (!self.editar) ContasService.cadastrar(self.dados).then(onSuccess, onError);
      else ContasService.alterar(self.contaId, self.dados).then(onSuccess, onError);
    },
    carregarDados() {
      const self = this;
      const onError = () => {
        self.modalAlert('Não foi possivel carregar os dados');
        self.carregandoDados = false;
      };

      const onSuccess = (res) => {
        self.carregandoDados = false;
        self.dados = res;
        self.calculado = true;
        self.calculando = false;
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
      this.editar = true;
      this.contaId = this.$route.params.contaId;
      this.carregarDados();
    } else {
      this.editar = false;
      this.contaId = 0;
    }
  },
};
</script>
