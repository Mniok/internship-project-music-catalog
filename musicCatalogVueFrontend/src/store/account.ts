import { defineStore } from 'pinia'
import axios from 'axios';
import router from '@/router';

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
      console.log("logging out...");

      axios.post('https://localhost:7026/api/Account/logout', {
        headers: { 'Authorization': `bearer ${this.accessToken}` }
      })
      .then(response => {
        this.accessToken = "";
        this.refreshToken = "";
        this.currentUser = "";

        //router.push({ name: 'home' })
        //this.$router
      })
      .catch(function (error :any) {
        console.log(error);
      });

      
    },
  },
})
