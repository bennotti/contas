import axios from 'axios';
// import _ from 'lodash';

const API_URL = process.env.VUE_APP_API_URL;

const restApi = {
  setAuthorization(token) {
    if (token) axios.defaults.headers.common.Authorization = token;
  },
  removeAuthorization() {
    axios.defaults.headers.common.Authorization = null;
  },
  logout() {
    return new Promise((resolve, reject) => {
      axios.delete(`${API_URL}/auth`).then((res) => {
        if (res.data.result) {
          resolve();
        } else {
          reject(new Error(res.data.msg));
        }
      }, (error) => {
        reject(error);
      });
    });
  },
  esqueceuSenha(user) {
    return new Promise((resolve, reject) => {
      axios.post(`${API_URL}/autenticacao/esqueceu-senha`, user).then((res) => {
        if (res.data.result) {
          resolve();
        } else {
          reject(new Error(res.data.msg));
        }
      }, (error) => {
        reject(error);
      });
    });
  },
  login(user) {
    return new Promise((resolve, reject) => {
      axios.post(`${API_URL}/autenticacao`, user).then((res) => {
        if (res.data.result) {
          const token = `Bearer ${res.data.token}`;
          resolve(token);
        } else {
          reject(new Error(res.data.msg));
        }
      }, (error) => {
        reject(error);
      });
    });
  },
  meusDados(token) {
    return new Promise((resolve, reject) => {
      axios.get(`${API_URL}/meusDados`, { headers: { Authorization: token } }).then((res) => {
        if (res.data.result) {
          resolve(res.data.dados);
        } else {
          reject(new Error('Login Inválido'));
        }
      }, (error) => {
        reject(error);
      });
    });
  },
};

export default restApi;
