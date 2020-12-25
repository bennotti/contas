import _ from 'lodash';

const mixin = {
  methods: {
    defaultToolbar() {
      return [
        [{ header: [false, 1, 2, 3, 4, 5, 6] }],
        ['bold', 'italic', 'underline', 'strike'], // toggled buttons
        [
          { align: '' },
          { align: 'center' },
          { align: 'right' },
          { align: 'justify' },
        ],
        ['blockquote', 'code-block'],
        [{ list: 'ordered' }, { list: 'bullet' }, { list: 'check' }],
        [{ indent: '-1' }, { indent: '+1' }], // outdent/indent
        [{ color: [] }, { background: [] }], // dropdown with defaults from theme
        ['clean'], // remove formatting button
      ];
    },
    paginacao(array, page, porPagina) {
      const pg = page || 1;
      const pgSize = porPagina || 100;
      const offset = (pg - 1) * pgSize;
      const pagedItems = _.drop(array, offset).slice(0, pgSize);
      return pagedItems;
    },
    removeHtml(text) {
      if (!text) return '';
      const regex = /(<([^>]+)>)/ig;
      return text.replace(regex, '');
    },
    // isMobile: function() {
    //     var md = new MobileDetect(window.navigator.userAgent)
    //     return md.mobile()
    // },
    makeOptionForVSelect(lista, label = 'text', value = 'value') {
      const result = [];
      for (let i = 0; i < lista.length; i += 1) {
        const item = lista[i];
        result.push({ label: item[label], value: item[value] });
      }
      return result;
    },
    makeOptions(primeiraOpcao, opcoes) {
      if (primeiraOpcao) opcoes.unshift(primeiraOpcao);
      return opcoes;
    },
    randomString(max = 8) {
      let random = '';
      do {
        random += this.guid();

        const re = new RegExp('-', 'g');
        random = random.replace(re, '');
      } while (max > random.length);

      return random.substring(0, max);
    },
    montaHtmlValidacoes(validacoes) {
      let html = '';
      if (!validacoes) return html;
      if (!validacoes.length) return html;

      html += '<ul>';
      for (let i = 0; i < validacoes.length; i += 1) {
        const item = validacoes[i];
        html += `<li>${item.campo}`;
        if (item.erros.length) {
          html += '<ul>';
          for (let ii = 0; ii < item.erros.length; ii += 1) {
            const itemError = item.erros[ii];
            html += `<li>${itemError}</li>`;
          }
          html += '</ul>';
        }
        html += '</li>';
      }
      html += '</ul>';
      return html;
    },
    closeModalAlert() {
      this.$modal.hide('dialog');
    },
    openModalAlertNoButtons(text) {
      this.$modal.show('dialog', {
        text,
        buttons: [],
      });
    },
    modalAlert(text, onConfirm) {
      const self = this;
      self.$modal.show('dialog', {
        text,
        buttons: [
          {
            title: 'Ok',
            default: true,
            handler: () => {
              self.$modal.hide('dialog');
              if (onConfirm) onConfirm();
            },
          },
        ],
      });
    },
    modalConfirm(text, onConfirm, onCancel) {
      const self = this;
      self.$modal.show('dialog', {
        text,
        buttons: [
          {
            title: 'Não',
            handler: () => {
              self.$modal.hide('dialog');
              if (onCancel) onCancel();
            },
          },
          {
            title: 'Sim',
            default: true,
            handler: () => {
              self.$modal.hide('dialog');
              if (onConfirm) onConfirm();
            },
          },
        ],
      });
    },
    strTimeToMomentFormat(value) {
      if (value === '') return { hour: 0, minute: 0, second: 0 };
      const dados = value.split(':');
      if (dados.length === 2) dados.push(0);
      return {
        hour: dados[0],
        minute: dados[1],
        second: dados[2],
      };
    },
    formatMoney(value, places = 2, thousand = '.', decimal = ',') {
      let number = value;
      const negative = number < 0 ? '-' : '';
      const i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10).toString();
      let j = i.length;
      j = j > 3 ? j % 3 : 0;
      return negative + (j ? i.substr(0, j) + thousand : '') + i.substr(j).replace(/(\d{3})(?=\d)/g, `$1${thousand}`) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : '');
    },
    guid(split = '-') {
      const s4 = () => Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
      return s4() + s4() + split + s4() + split + s4() + split + s4() + split + s4() + s4() + s4();
    },
  },
};

export default mixin;
