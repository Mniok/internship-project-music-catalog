<template>
  <v-container fluid>
    <v-form class="d-flex">
      <v-toolbar style="background-color:#00000000" elevation="0">    <!-- search criteria bar -->
      
        <!--
        <v-checkbox
          v-model="searchByFavourite"
          label="Show favourite only"
          color="indigo lighten-1"
          class="search-inputs"
          hide-details
        ></v-checkbox>
        <v-spacer></v-spacer>
        -->

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
          color="indigo lighten-1"
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
          color="indigo lighten-1"
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
          color="indigo lighten-1"
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
    </v-form>

    <v-divider class="blue-grey darken-1"></v-divider>

    <!-- main content - song widgets page -->
    <!--<h1 style="height:2137px">test footer if there's content</h1>-->


    <!-- bar for bottom pagination and add song button -->
    <v-footer  
      absolute 
      padless 
      style="background-color:#00000000" elevation="0"
      class="mb-13 d-flex justify-space-between"
    >  <!-- bars seem to always be the same height so positioning with mb-13 shouldn't be a problem on differrent size displays -->
      <v-card class="hidden">
          <!-- invisible card to center pagination component -->
          <v-btn class="pl-8">
          <span class="mr-2">Upload new song</span> <!-- not really, just same width -->
          <v-icon>mdi-tray-arrow-up</v-icon>
        </v-btn> 
      </v-card>
      
      <v-card >
            <span>&nbsp;&nbsp;&lt; pagination goes here &gt;&nbsp;&nbsp;</span>    <!-- !!! layout kinda breaks when dislpay is thin, might have to split pagination and upload into 2 differrent footers, or make seperate style for s or xs displays -->
      </v-card>

      <v-card class="corner-curved" color="success">
        <v-btn to="/upload" class="corner-curved no-decorators pl-8 pt-1" text color="grey lighten-3">
          <span class="mr-2">Upload new song</span>
          <v-icon>mdi-tray-arrow-up</v-icon>
        </v-btn>
      </v-card>
    </v-footer>

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

    created() {
        axios.get('https://localhost:7026/api/Song/songs/', {
          //id: this.$route.params.id
        },
        {
          headers: { 'Authorization': `bearer ${this.accessToken}` }
        })
        .then(response => { 
          console.log(response.data); ////
          console.log(response.data.$values); ////
          /*console.log(response.data.$values.at(16).valueOf().$ref); ////
          console.log(JSON.decycle(response.data.$values));
          var newJson : any;
          var njson = JSON.decycle(response.data.$values, newJson);
          console.log(newJson);
          console.log(njson);
          var elem = response.data.$values[0];
          elem.$ref = 2;
          elem.$id = 2;
          //elem.uploadedBy.uploadedSongs.$values[0].$ref = elem;
          console.log(JSON.decycle(elem))
          console.log(JSON.retrocycle(elem))
          console.log(JSON.retrocycle(response.data.$values));
          console.log("---------");
          console.log(JSON.retrocycle(response.data));
          //console.log(JSON.hbasfsdhga(response.data)); properly crashes, meaning cycle.js IS used
          console.log(JSON.retrocycle(response.data));
          console.log(JSON.retrocycle(JSON.stringify(response.data.$values)));
          console.log(JSON.parse(JSON.retrocycle(JSON.stringify(response.data.$values))));
          console.log(JSON.parse(JSON.decycle(JSON.stringify(response.data.$values))));*/

          /*this.song = response.data;

          this.songTitle = response.data.title;
          this.songDescription = response.data.description;
          this.songTime = response.data.time;
          this.songUploader = response.data.uploadedBy.userName;
          this.songUploadedOn = response.data.createdAt;


          for(var i=0; i<response.data.artists.$values.length; i++){
            this.songArtists.push(response.data.artists.$values.at(i).artistName);
          }

          for(var i=0; i<response.data.genres.$values.length; i++){
            this.songGenres.push(response.data.genres.$values.at(i).genreName);
          }

          for(var i=0; i<response.data.links.$values.length; i++){
            var l : Link = {toSite: "", linkBody: ""};
            l.toSite = response.data.links.$values.at(i).toSite;
            l.linkBody = response.data.links.$values.at(i).linkBody;
            this.songLinks.push(l);
          }*/

        })
        .catch(function (error) {
          console.log(error);
          if(error.response == undefined){
            alert("Couldn't connect to the server. Try again later.");
          }
          if(error.response.status == 401){
            alert("401: unauthorized");
          }
        });
    },

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

.hidden {
  visibility: hidden;
}

.corner-curved {
  border-radius: 48px 0px 0px 0px !important;
}

</style>