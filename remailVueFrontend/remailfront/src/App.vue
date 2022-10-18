<template>
  <v-app>
    <v-app-bar
      app
      color="indigo lighten-1"
      dark
    >
      <div class="d-flex align-center">
        <v-icon x-large>mdi-email-sync</v-icon>

        <h4 class="mt-2 ml-2">re:mail</h4>
      </div>

      <v-spacer></v-spacer>

      <v-btn text disabled>
        <span class="mr-2">Not logged in</span>
        <v-icon>mdi-account-circle</v-icon>
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

export default Vue.extend({

  name: 'App',

  data: () => ({
    //
  }),
});
</script>