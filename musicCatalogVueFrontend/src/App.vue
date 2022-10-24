<template>
  <v-app>
    <v-app-bar
      app
      color="indigo lighten-1"
      dark
    >
      
      <div class="d-flex align-center">
        <v-icon x-large>mdi-email-sync</v-icon>

        <router-link to="/login" style="text-decoration: none;">
          <h4 class="mt-2 ml-2 white--text">re:mail</h4>
        </router-link>
      </div>

      <v-spacer></v-spacer>

      <v-btn text :disabled="!isLoggedIn">
        <span v-if="!isLoggedIn" class="mr-2">Not logged in</span>
        <span v-else class="mr-2">Logged in as {{currentUser}}</span>
        <v-icon>mdi-account-circle</v-icon>
      </v-btn>
      <v-btn v-if="isLoggedIn" text color="red lighten-2" @click="logout">
        <span class="grey--text text--lighten-2">Log out</span>
      </v-btn>
    </v-app-bar>

    <v-main>
      <router-view/>
    </v-main>

    <v-footer absolute padless>
      <v-card
        flat
        tile
        width="100%"
        class="indigo lighten-1 text-center"
      >
        <v-card-text class="white--text">
          <v-btn to="/about" color="blue lighten-5" text rounded>contact/about</v-btn>
        </v-card-text>
      </v-card>
    </v-footer>


  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import { useAccountStore } from './store/account';
import { mapState, mapActions } from 'pinia';

export default Vue.extend({

  name: 'App',

  data: () => ({
    //
  }),

  methods: {
      
      ...mapActions(useAccountStore, ['logout'])
    },

  computed: {
      isLoggedIn() {
        return !!this.currentUser;
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken', 'currentUser']),
    }
});
</script>


<style>
/*body {
  background-color: #283593; / indigo darken-3 /
}*/
</style>