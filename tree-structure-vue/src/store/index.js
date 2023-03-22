import { createStore } from 'vuex';

import nodesModule from './modules/nodes/index.js'

const store = createStore({
    modules:{
        nodes: nodesModule,
    },
});

export default store;