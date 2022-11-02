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

      localStorage.setItem('Auth', this.accessToken);
      localStorage.setItem('Refresh', this.refreshToken);
    },

    updateCurrentUser(accessToken: string){
      axios.get(`https://localhost:7026/api/Account/user`, {
        headers: { 'Authorization': `bearer ${accessToken}` }
      })
      .then(response => {
        this.currentUser = response.data.username;
        //console.log(`logged in as ${this.currentUser}`);
      })
      .catch(function (error) {
        console.log(error);
      });
    },

    logout(){
      //console.log("logging out...");  ////
      //console.log(`bearer ${this.accessToken}`) ////

      var token : string = this.accessToken;  //to pass to api, because pinia store gets cleared

      this.accessToken = "";    //moved here, because You couldn't log out when token expired and got locked in
      this.refreshToken = "";
      this.currentUser = "";

      localStorage.removeItem('Auth');
      localStorage.removeItem('Refresh');

      vueApp.$router.push("/");

      axios.post(`https://localhost:7026/api/Account/logout`, {/*no params*/}, {
        headers: { 'Authorization': `bearer ${token}` }
      })
      .then(() => {
        /*this.accessToken = "";    //moved outside of .then because otherwise, You got locked in when the token expired
        this.refreshToken = "";
        this.currentUser = "";

        localStorage.removeItem('Auth');
        localStorage.removeItem('Refresh');

        //router.push({ name: 'home' })
        //this.router.push("/");

        vueApp.$router.push("/");*/
      })
      .catch(function (error :any) {
        console.log(error);
      });

    },

    refreshJWT(){
      let logoutfunc = this.logout; //handle bo w catch error jest inny scope

      axios.post(`https://localhost:7026/api/Account/refresh-token`, 
      {
        refreshToken: this.refreshToken
      }, 
      {
        headers: { 'Authorization': `bearer ${this.accessToken}` }
      })
      .then(response => {
        this.newJWT(response.data.accessToken, response.data.refreshToken)
      })
      .catch(function (error) {
        console.log(error);
        if(error.response.status == 401){
          alert("Token expired. Log in again and retry.");
          logoutfunc();
        }
      });
    },

  },


})
