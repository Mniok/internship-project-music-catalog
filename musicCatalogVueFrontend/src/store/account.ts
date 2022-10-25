import { defineStore } from 'pinia'
import axios from 'axios';
//import router from '.././router'; //`router: router` w options wywala bledy
//import router from '@/router';
import { vueApp } from '../main';   //// dla routera


export const useAccountStore = defineStore('accountStore', {

  state: () => ({
    accessToken: "",
    refreshToken: "",
    currentUser: "",
  }),

  /*getters: {
    double: state => state.count * 2,
  },*/

  actions: {
    newJWT(accessToken: string, refreshToken: string){
      this.accessToken = accessToken;
      this.refreshToken = refreshToken;
    },

    updateCurrentUser(username: string){
      this.currentUser = username;
    },

    logout(){
      //console.log("logging out...");  ////
      //console.log(`bearer ${this.accessToken}`) ////

      axios.post(`https://localhost:7026/api/Account/logout`, {/*no params*/}, {
        headers: { 'Authorization': `bearer ${this.accessToken}` }
      })
      .then(response => {
        this.accessToken = "";
        this.refreshToken = "";
        this.currentUser = "";

        //router.push({ name: 'home' })
        //this.router.push("/");

        vueApp.$router.push("/");
      })
      .catch(function (error :any) {
        console.log(error);
      });

    },

  },


})
