<template>
  <v-app class="blue-grey-darken-4-bg" >
    <v-app-bar
      app
      color="indigo lighten-1"
      dark
    >
      
      <div class="d-flex align-center">
        <v-icon x-large>mdi-folder-music</v-icon>

        <router-link :to="isLoggedIn ? '/app' : '/login'" class="no-decorators">
          <h4 class="mt-2 ml-2 white--text">{{this.$appName}}</h4>
        </router-link>
      </div>

      <v-spacer></v-spacer>

      <v-btn to="/app" text :disabled="!isLoggedIn" class="no-decorators">
        <span v-if="!isLoggedIn" class="mr-2">Not logged in</span>
        <span v-else class="mr-2">Logged in as {{currentUser}}</span>
        <v-icon>mdi-account-circle</v-icon>
      </v-btn>
      <v-btn to="/settings" v-if="isLoggedIn" text color="blue lighten-2" class="no-decorators">
        <v-icon color="grey lighten-2">mdi-cog</v-icon>
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
        <v-card-text class="white--text pt-2 pb-2">
          <v-btn to="/about" color="blue lighten-5" class="no-decorators" text rounded>
            <v-icon>mdi-card-account-details-outline</v-icon>
            <span class="ml-2">contact/about</span>
          </v-btn>
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
      
      ...mapActions(useAccountStore, ['newJWT', 'updateCurrentUser', 'logout', 'refreshJWT'])
  },

  created() {
    var accessToken : string | null = localStorage.getItem('Auth');
    var refreshToken : string | null = localStorage.getItem('Refresh');

    if (!!accessToken && !!refreshToken){
      this.newJWT(accessToken, refreshToken);
      this.refreshJWT();
      this.updateCurrentUser(accessToken);
    }
  },

  computed: {
      isLoggedIn() {
        return !!this.currentUser;
      },

      logoLink() {  //where clicking logo redirects you
        return false;
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken', 'currentUser']),
  }
});
</script>


<style>
.indigo-darken-3-bg {
  background-color: #283593 !important;   /* nie może być scoped! */
}

.blue-grey-darken-4-bg {
  background-color: #263238 !important;
}

.no-decorators {
  text-decoration: none !important;
}
</style>