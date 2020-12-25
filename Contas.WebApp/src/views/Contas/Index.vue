<template>
<div>
  <nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><router-link tag="a" v-bind:to="{ name: 'Contas', params: { }}">Contas</router-link></li>
    <li class="breadcrumb-item active" aria-current="page">Acesso Rápido</li>
  </ol>
</nav>
  <div class="card-deck mb-3 text-center">
    <div class="card mb-6 shadow-sm">
      <div class="card-header">
        <h4 class="my-0 font-weight-normal">Nova conta</h4>
      </div>
      <div class="card-body">
        <ul class="list-unstyled mt-3 mb-4">
          <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit. </li>
        </ul>
        <router-link tag="a" class="btn btn-lg btn-block btn-outline-info" v-bind:to="{ name: 'ContasAdicionar', params: { }}">Adicionar</router-link>
      </div>
    </div>
    <div class="card mb-6 shadow-sm">
      <div class="card-header">
        <h4 class="my-0 font-weight-normal">Pesquisar contas</h4>
      </div>
      <div class="card-body">
        <ul class="list-unstyled mt-3 mb-4">
          <li>Lorem ipsum dolor sit amet, consectetur adipiscing elit. </li>
        </ul>
        <router-link tag="a" class="btn btn-lg btn-block btn-outline-info" v-bind:to="{ name: 'ContasPesquisar', params: { }}">Pesquisar</router-link>
      </div>
    </div>
  </div>
  <p>Últimas atualizações</p>
      <div class="table-responsive">
        <table class="table table-striped table-sm">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Valor da conta</th>
              <th>Valor da multa</th>
              <th>Valor final</th>
              <th>Vencimento</th>
              <th>Pagamento</th>
              <th>Dias atrasados</th>
              <th>Opções</th>
            </tr>
          </thead>
          <tbody v-if="carregando">
              <tr>
                <td colspan="8" class="text-center"><img src="/static/imgs/loading.gif" /> Carregando...</td>
              </tr>
            </tbody>
            <tbody v-if="!contas.length && !carregando">
              <tr>
                <td colspan="8">Nenhuma registro encontrado.</td>
              </tr>
            </tbody>
            <tbody v-if="contas.length && !carregando">
              <tr v-for="(item, index) in contas" v-bind:key="item.contaId">
                <td>{{item.nome}}</td>
                <td>R$ {{formatMoney(item.valorOriginal)}}</td>
                <td>R$ {{formatMoney(item.valorMulta)}}</td>
                <td>R$ {{formatMoney(item.valorFinal)}}</td>
                <td>{{item.vencimento}}</td>
                <td>{{item.pagamento}}</td>
                <td>{{item.qntDiasAtraso}}</td>
                <td>
                  <div class="btn-group" v-if="!item.loading">
                    <router-link class="btn btn-sm btn-primary mr-2" v-bind:to="{ name: 'ContasVisualizar', params: { contaId: item.contaId } }" data-toggle="tooltip" data-placement="top" title="Visualizar">
                      <font-awesome-icon icon="search" />
                    </router-link>
                    <router-link class="btn btn-sm btn-secondary mr-2" v-bind:to="{ name: 'ContasEditar', params: { contaId: item.contaId } }" data-toggle="tooltip" data-placement="top" title="Editar">
                      <font-awesome-icon icon="pen" />
                    </router-link>
                    <button class="btn btn-sm btn-danger mr-2" data-toggle="tooltip" data-placement="top" title="Remover"  @click.prevent="remover(item, index)">
                      <font-awesome-icon icon="trash-alt" />
                    </button>
                  </div>
                  <div v-else>
                    <img src="/static/imgs/loading.gif" />
                  </div>
                </td>
              </tr>
            </tbody>
        </table>
      </div>
</div>
</template>

<script>
import { mapGetters } from 'vuex';

import ContasService from '@/services/ContasServices';

export default {
  data() {
    return {
      carregando: true,
      contas: [],
    };
  },
  computed: {
    ...mapGetters([
      'usuarioLogado',
      'isAuthLoading',
    ]),
  },
  methods: {
    remover(obj, index) {
      const self = this;

      const onConfirm = () => {
        this.contas[index].loading = true;
        ContasService.remover(obj.contaId).then(() => {
          self.obterUltimosRegistros();
          self.$notice({
            type: 'success', // alert, success, warning, error, info/information
            text: 'Conta removida com sucesso.',
          });
        }, () => {
          this.contas[index].loading = false;
          self.$notice({
            type: 'error', // alert, success, warning, error, info/information
            text: 'Não foi possível remover a conta.',
          });
        });
      };

      self.modalConfirm('Deseja realmente remover a conta?', onConfirm);
    },
    obterUltimosRegistros() {
      const self = this;
      self.carregando = true;
      ContasService.obterUltimasAtualizacoes().then((dados) => {
        self.contas = dados.contas;
        self.totalRegistros = dados.totalRegistros;
        self.carregando = false;
      }, () => {
        self.$notice({
          type: 'error', // alert, success, warning, error, info/information
          text: 'Não foi possível carregar as contas.',
        });
        self.carregando = false;
      });
    },
  },
  created() {
    if (!this.usuarioLogado) return;
    const self = this;
    self.obterUltimosRegistros();
  },
};
</script>
