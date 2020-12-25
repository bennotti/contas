import _ from 'lodash';

import apiRest from '@/api/rest/contas';
import apiMock from '@/api/mock/contas';

const API_URL = process.env.VUE_APP_API_URL;

export default _.isNull(API_URL) || _.isEmpty(API_URL) ? apiMock : apiRest;
