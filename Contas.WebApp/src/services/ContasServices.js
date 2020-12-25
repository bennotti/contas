import store from '@/store';

export default {
  calcularMulta(dados) {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_CALCULAR_MULTA', dados).then((result) => {
        resolve(result);
      }, () => {
        reject(new Error('Erro ao calcular a multa'));
      });
    });
  },
  obterUltimasAtualizacoes() {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_OBTERULTIMAS').then((result) => {
        resolve(result);
      }, () => {
        reject(new Error('Erro ao retornar as contas'));
      });
    });
  },
  obter(filtro) {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_OBTER', filtro).then((result) => {
        resolve(result);
      }, () => {
        reject(new Error('Erro ao retornar as contas'));
      });
    });
  },
  obterPorId(id) {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_OBTER_POR_ID', id).then((result) => {
        resolve(result);
      }, () => {
        reject(new Error('Erro ao retornar os dados da conta'));
      });
    });
  },
  alterar(contaId, dados) {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_ATUALIZAR', { contaId, dados }).then((result) => {
        resolve(result);
      }, () => {
        reject(new Error('Erro ao salvar os dados da conta'));
      });
    });
  },
  cadastrar(dados) {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_CADASTRAR', dados).then((result) => {
        resolve(result);
      }, () => {
        reject(new Error('Erro ao salvar os dados da conta'));
      });
    });
  },
  remover(id) {
    return new Promise((resolve, reject) => {
      store.dispatch('CONTAS_REMOVER', id).then(() => {
        resolve();
      }, () => {
        reject(new Error('Erro ao remover a conta'));
      });
    });
  },
};
