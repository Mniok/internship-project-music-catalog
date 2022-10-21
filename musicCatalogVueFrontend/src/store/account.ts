import { defineStore } from 'pinia'

export const useAccountStore = defineStore('accountStore', {
  state: () => ({
    accessToken: "1234567",
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
    }
  },
})
