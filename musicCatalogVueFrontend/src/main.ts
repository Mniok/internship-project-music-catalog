import '@babel/polyfill'
import 'mutationobserver-shim'
import Vue from 'vue'
import './plugins/bootstrap-vue'
import './plugins/axios'
import App from './App.vue'
import './registerServiceWorker'
import router from './router'
import vuetify from './plugins/vuetify'
import { createPinia, PiniaVuePlugin } from 'pinia'
import VueCompositionAPI from '@vue/composition-api'
import { markRaw } from 'vue'

Vue.use(PiniaVuePlugin);
const pinia = createPinia();
pinia.use(({ store }) => { store.$router = markRaw(router) });
Vue.use(VueCompositionAPI);

Vue.prototype.$appName = 'note-a-logue';

Vue.config.productionTip = false;

var vueApp = new Vue({
  router,
  vuetify,
  pinia,
  render: h => h(App)
}).$mount('#app')

export {vueApp};