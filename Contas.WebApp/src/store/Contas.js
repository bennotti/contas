import contaApi from '@/api/contas';

const CONTAS_OBTER = 'CONTAS_OBTER';
const CONTAS_CALCULAR_MULTA = 'CONTAS_CALCULAR_MULTA';
const CONTAS_OBTERULTIMAS = 'CONTAS_OBTERULTIMAS';
const CONTAS_OBTIDAS = 'CONTAS_OBTIDAS';
const CONTAS_CADASTRAR = 'CONTAS_CADASTRAR';
const CONTAS_ATUALIZAR = 'CONTAS_ATUALIZAR';
const CONTAS_OBTER_POR_ID = 'CONTAS_OBTER_POR_ID';
const CONTAS_REMOVER = 'CONTAS_REMOVER';

const state = { };

const mutations = {
  [CONTAS_OBTIDAS]() { },
};

const actions = {
  [CONTAS_OBTER_POR_ID]({ }, id) {
    return new Promise((resolve, reject) => {
      contaApi.obterPorId(id).then((data) => {
        if (data.result) {
          resolve(data);
        } else {
          reject(new Error(data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  [CONTAS_OBTERULTIMAS]({ }) {
    return new Promise((resolve, reject) => {
      contaApi.ultimasAtualizacoes().then((data) => {
        if (data.result) {
          resolve(data);
        } else {
          reject(new Error(data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  [CONTAS_OBTER]({ }, filtro) {
    return new Promise((resolve, reject) => {
      contaApi.listar(filtro).then((data) => {
        if (data.result) {
          resolve(data);
        } else {
          reject(new Error(data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  [CONTAS_CADASTRAR]({ }, dados) {
    return new Promise((resolve, reject) => {
      contaApi.cadastrar(dados).then(() => {
        resolve();
      }, () => {
        reject(new Error('Não foi possivel cadastrar a conta'));
      });
    });
  },
  [CONTAS_CALCULAR_MULTA]({ }, dados) {
    return new Promise((resolve, reject) => {
      contaApi.calcularMulta(dados).then((data) => {
        if (data.result) {
          resolve(data);
        } else {
          reject(new Error(data.msg));
        }
      }, () => {
        reject(new Error('Não foi calcular a multa'));
      });
    });
  },
  [CONTAS_ATUALIZAR]({ }, model) {
    return new Promise((resolve, reject) => {
      contaApi.atualizar(model.contaId, model.dados).then(() => {
        resolve();
      }, () => {
        reject(new Error('Não foi possivel atualizar a conta'));
      });
    });
  },
  [CONTAS_REMOVER]({ }, id) {
    return new Promise((resolve, reject) => {
      contaApi.remover(id).then(() => {
        resolve();
      }, () => {
        reject(new Error('Não foi possivel remover a conta'));
      });
    });
  },
};

const getters = { };

export default {
  state,
  mutations,
  getters,
  actions,
};
