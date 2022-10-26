<template>
  <v-container fluid>
    <v-toolbar style="background-color:#00000000" elevation="0">

      <!---->
      <v-checkbox
        v-model="searchByFavourite"
        label="Show favourite only"
        color="indigo lighten-1"
        class="search-inputs"
        hide-details
      ></v-checkbox>
      <v-spacer></v-spacer>
      <!---->

      <v-checkbox
        v-model="searchByUploader"
        label="Show only uploaded by me"
        color="indigo lighten-1"
        class="search-inputs"
        hide-details
      ></v-checkbox>

      <v-spacer class="thin"></v-spacer>

      <v-text-field
        v-model="searchByTitle"
        label="Search by Title:"
        class="search-inputs mt-4 wider-field"
      >
        <v-icon 
          slot="prepend" 
          color="indigo lighten-2"
        >mdi-tag-text-outline</v-icon>
      </v-text-field>

      <v-spacer></v-spacer>

      <v-text-field
        v-model="searchByArtist"
        label="Search by Artists:"
        class="search-inputs mt-4 wider-field"
      >
        <v-icon 
          slot="prepend" 
          color="indigo lighten-2"
        >mdi-account-box-multiple</v-icon>
      </v-text-field>

      <v-spacer></v-spacer>

      <v-text-field
        v-model="searchByGenre"
        label="Search by Genres:"
        class="search-inputs mt-4 wider-field"
      >
        <v-icon 
          slot="prepend" 
          color="indigo lighten-2"
        >mdi-music-box-multiple</v-icon>
      </v-text-field>

      <v-btn text color="indigo lighten-2" class="ml-8" @click="search">
        <v-icon x-large >mdi-magnify</v-icon>
      </v-btn>

    </v-toolbar>

    <v-divider class="blue-grey darken-1"></v-divider>

  </v-container>
</template>
  
<script lang="ts">
 import Vue from 'vue'
  //import LoginForm from '../components/LoginForm.vue'
  
  export default Vue.extend({
    name: 'AppView',

    data: () => ({
      searchByFavourite: false,
      searchByUploader: false,
      searchByTitle: '',
      searchByArtist: '',
      searchByGenre: ''
    }),
  
    /*components: {
      LoginForm,
    },*/

    methods: {
      search () {
        console.log("clicked search button");
      },
    },


  })
</script>
  
<style>
/* nie może być scoped bo komponenty vuetify są z innego komponentu - poza scopem! */
        /* zamiast scoped -> klasa searchInputs! */

hr.widen {  /* do v-divider */
  border-width: 1px;
}

/*.searchInputs > .v-input > .v-input__control > .v-input__slot > .v-input--selection-controls__input > .v-icon,*/ /*nie działają te 2 */
/*.theme--light.v-checkbox > .v-input__control > .v-input__slot > .v-input--selection-controls__input > .v-icon,*/
.search-inputs>* .v-label,
.search-inputs>* .v-icon { /* do wyłączonych checkboxów, bo color="" wspiera tylko checked a unchecked są zawsze czarne */
  color: #7986CB !important;
}  /* indigo lighten-2 */

.search-inputs>* .v-label {
  margin-bottom: 0px !important;
}

.search-inputs>* input{ 
  /*color: #C5CAE9 !important;  /* indigo lighten-4 */
  color: #5C6BC0 !important;  /* indigo lighten-1 */
}

/*.theme--light.v-text-field > .v-input__control > .v-input__slot:before*/ /*ze zbadaj'a - działa dobrze ale nie jest "scoped" do klasy searchInputs*/
.search-inputs>* .v-input__slot:before {
    border-color: #7986CB !important;
}

.wider-field {
  min-width: 20vw;
}

</style>