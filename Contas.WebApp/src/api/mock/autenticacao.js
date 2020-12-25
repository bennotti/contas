// MOCK API

const mock = {
  setAuthorization(/* token */) {
    // if (token) console.log(token)
  },
  removeAuthorization() {
    // console.log('removeAuthorization')
  },
  logout() {
    return new Promise((resolve) => {
      // console.log('logout')
      resolve();
    });
  },
  login(/* user */) {
    return new Promise((resolve) => {
      resolve('Bearer tokenAutenticacao');
    });
  },
  userProfile(token) {
    return new Promise((resolve) => {
      resolve({
        id: 1,
        nome: 'Usuário MOCK',
        email: 'usuario@mock.com.br',
        dataCadastro: '01/01/2018 00:00:00',
        dataUltimaAlteracao: '01/01/2018 00:00:00',
        alterarSenha: false,
        token,
      });
    });
  },
};

export default mock;
