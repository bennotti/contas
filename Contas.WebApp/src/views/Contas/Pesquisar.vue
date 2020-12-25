<template>
<div>
  <nav aria-label="breadcrumb">
  <ol class="breadcrumb">
    <li class="breadcrumb-item"><router-link tag="a" v-bind:to="{ name: 'Contas', params: { }}">Contas</router-link></li>
    <li class="breadcrumb-item active" aria-current="page">Pesquisar</li>
  </ol>
</nav>
  <div class="card-deck mb-3 text-left">
    <div class="card mb-12 shadow-sm">
      <div class="card-header">
        <div class="row">
          <div class="col-6">
            <h4 class="my-0 font-weight-normal">Filtro</h4>
          </div>
          <div class="col-6">
            <button class="btn float-right btn-outline-secondary" v-on:click="mostrarEsconder"><span v-if="filtroVisivel">Esconder</span><span v-else>Mostrar</span></button>
          </div>
        </div>
      </div>
      <div class="card-body" v-if="filtroVisivel">
<form id="formConta" name="formConta" ref="formConta" @submit.prevent="obter" @reset.prevent="limparObter">
    <div class="form-group row">
    <div class="col-sm-12 text-right">
      <button type="reset" class="btn btn-outline-secondary mr-2" :disabled="carregando">Limpar filtro</button>
      <button type="submit" class="btn btn-outline-primary pl-5 pr-5" :disabled="carregando">Filtrar</button>
    </div>
  </div>

  <div class="form-group row">
    <label for="nome" class="col-sm-2 col-form-label">Nome</label>
    <div class="col-sm-10">
      <input type="text" class="form-control" id="nome" name="nome" v-model="filtro.nome"/>
    </div>
  </div>
  <div class="form-group row">
    <label for="valor" class="col-sm-2 col-form-label">Valor</label>
    <div class="col-sm-10">
      <div class="row">
        <div class="col-6">
          <money name="valor" v-model="filtro.valor" placeholder="R$ 0,00" class="form-control"></money>
        </div>
        <div class="col-6">
          <select v-model="filtro.tipoComparacaoValor" class="c-select form-control boxed">
              <option value="0">Nenhum</option>
              <option value="1">Igual</option>
              <option value="2">Maior Igual</option>
              <option value="3">Menor Igual</option>
            </select>
        </div>
      </div>
    </div>
  </div>

  <div class="form-group row">
    <div class="col-sm-12 text-right">
      <button type="reset" class="btn btn-outline-secondary mr-2" :disabled="carregando">Limpar filtro</button>
      <button type="submit" class="btn btn-outline-primary pl-5 pr-5" :disabled="carregando">Filtrar</button>
    </div>
  </div>
</form>
      </div>
    </div>
  </div>
  <p v-if="!carregando">{{totalRegistros}} Registros</p>
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
      totalRegistros: 0,
      filtroVisivel: false,
      carregando: true,
      contas: [],
      filtro: {
        nome: '',
        valor: 0,
        tipoComparacaoValor: 0,
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
    remover(obj, index) {
      const self = this;

      const onConfirm = () => {
        self.contas[index].loading = true;
        ContasService.remover(obj.contaId).then(() => {
          self.obter();
          self.$notice({
            type: 'success', // alert, success, warning, error, info/information
            text: 'Conta removida com sucesso.',
          });
        }, () => {
          self.contas[index].loading = false;
          self.$notice({
            type: 'error', // alert, success, warning, error, info/information
            text: 'Não foi possível remover a conta.',
          });
        });
      };

      self.modalConfirm('Deseja realmente remover a conta?', onConfirm);
    },
    mostrarEsconder() {
      this.filtroVisivel = !this.filtroVisivel;
    },
    limparObter() {
      this.filtro = {
        nome: '',
        valor: 0,
        tipoComparacaoValor: 0,
      };
      this.obter();
    },
    obter() {
      const self = this;
      self.carregando = true;
      ContasService.obter(self.filtro).then((dados) => {
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
    self.obter();
  },
};
</script>
