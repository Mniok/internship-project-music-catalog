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

Vue.use(PiniaVuePlugin)
const pinia = createPinia()
pinia.use(({ store }) => { store.router = markRaw(router) });   //to allow using router inside pinia actions
Vue.use(VueCompositionAPI)


Vue.config.productionTip = false

new Vue({
  router,
  vuetify,
  pinia,
  render: h => h(App)
}).$mount('#app')
