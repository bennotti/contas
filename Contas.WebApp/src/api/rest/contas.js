import axios from 'axios';
// import _ from 'lodash';

const API_URL = process.env.VUE_APP_API_URL;

const restApi = {
  calcularMulta(dados) {
    return new Promise((resolve, reject) => {
      axios.post(`${API_URL}/contas/calcularMulta`, dados).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  ultimasAtualizacoes() {
    return new Promise((resolve, reject) => {
      axios.get(`${API_URL}/contas`).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  listar(filtro) {
    return new Promise((resolve, reject) => {
      axios.get(`${API_URL}/contas/pesquisar`, { params: filtro }).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  obterPorId(contaId) {
    return new Promise((resolve, reject) => {
      axios.get(`${API_URL}/contas/${contaId}`).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  cadastrar(dados) {
    return new Promise((resolve, reject) => {
      axios.post(`${API_URL}/contas`, dados).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  atualizar(contaId, dados) {
    return new Promise((resolve, reject) => {
      axios.put(`${API_URL}/contas/${contaId}`, dados).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
  remover(contaId) {
    return new Promise((resolve, reject) => {
      axios.delete(`${API_URL}/contas/${contaId}`).then((res) => {
        if (res.data.result) {
          resolve(res.data);
        } else {
          reject(new Error(res.data.msg));
        }
      }, () => {
        reject(new Error('error'));
      });
    });
  },
};

export default restApi;
