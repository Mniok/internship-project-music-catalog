<template>
  <v-container fluid>
    <v-form class="d-flex">
      <v-toolbar style="background-color:#00000000" elevation="0">    <!-- search criteria bar -->
      
        <!--
        <v-checkbox
          v-model="formSearchByFavourite"
          label="Show favourite only"
          color="indigo lighten-1"
          class="search-inputs"
          hide-details
        ></v-checkbox>
        <v-spacer></v-spacer>
        -->

        <v-checkbox
          v-model="formSearchByUploader"
          label="Show only uploaded by me"
          color="indigo lighten-1"
          class="search-inputs"
          hide-details
        ></v-checkbox>

        <v-spacer class="thin"></v-spacer>

        <v-text-field
          v-model="formSearchByTitle"
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
          v-model="formSearchByArtist"
          label="Search by Artist:"
          class="search-inputs mt-4 wider-field"
          color="indigo lighten-1"
        >
          <v-icon 
            slot="prepend" 
            color="indigo lighten-2"
          >mdi-account-box</v-icon>
        </v-text-field>

        <v-spacer></v-spacer>

        <v-text-field
          v-model="formSearchByGenre"
          label="Search by Genre:"
          class="search-inputs mt-4 wider-field"
          color="indigo lighten-1"
        >
          <v-icon 
            slot="prepend" 
            color="indigo lighten-2"
          >mdi-music-box</v-icon>
        </v-text-field>

        <v-btn text color="indigo lighten-2" class="ml-8" @click="search">
          <v-icon x-large >mdi-magnify</v-icon>
        </v-btn>

      </v-toolbar>
    </v-form>

    <v-divider class="blue-grey darken-1"></v-divider>

    <!-- main content - song widgets page -->
    <v-container
      :v-if="readyToDisplaySongs"
      id="songs-container"
      class="d-flex flex-wrap mb-16"
    >
      <song-widget v-for="s in songsList" :song="s" class="horizontal-tiling"/>
    </v-container>


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
      
      <v-card v-if="readyToDisplaySongs" style="height:40px">
        <v-col cols="12" class="p-0">
          <v-container class="max-width p-0">
            <v-pagination
              v-model="page"
              class="my-0"
              :length="numOfPages"
              @input="handlePagination(page)"
            ></v-pagination>
          </v-container>
        </v-col>    <!-- !!! layout kinda breaks when dislpay is thin, might have to split pagination and upload into 2 differrent footers, or make seperate style for s or xs displays -->
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
import Vue from 'vue';
import { useAccountStore } from '../store/account';
import { useSongStore } from '../store/song';
import { mapState, mapActions } from 'pinia';
import SongWidget from '../components/SongWidget.vue'
import { Link } from '../service/linkHelpers';

var defaultPageSize : number = 12;

interface Song {
  id: number,
  title: string,
  description: string,
  time: number,
  artists: Array<String>,
  genres: Array<String>,
  links: Array<Link>,
  uploadedBy: string,
  uploadedDate: string
}
  
  export default Vue.extend({
    name: 'AppView',

    components: {
      SongWidget,
    },

    data: () => ({
      formSearchByFavourite: false,
      formSearchByUploader: false,
      formSearchByTitle: '',
      formSearchByArtist: '',
      formSearchByGenre: '',

      songsList: Array<Song>(),
      readyToDisplaySongs: false,

      page: 1,
      numOfPages: Number,
      
    }),
  
    /*components: {
      LoginForm,
    },*/

    created() {
      this.loadPage(1);
    },

    methods: {
      loadPage(page: number){
        page -= 1; //because API is 0-based but pagination component is 1-based!!!!

        var searchCriteria = this.searchUploadedByMe + '/' + page + '/' + defaultPageSize + '/'
            + this.searchSongFlags + '/' + this.searchByTitle + '/' + this.searchByArtist + '/' + this.searchByGenre;
      

        this.refreshJWT();
        axios.get('https://localhost:7026/api/Song/searchsongs/' + searchCriteria, {
            headers: { 'Authorization': `bearer ${this.accessToken}` }
          })
        .then(response => { 
          //console.log(response.data); ////

          this.page = response.data.pagination.page +1;   //again, because API is 0-based but pagination component is 1-based!!!! 
          this.numOfPages = response.data.pagination.pagesFromSearch;

          this.songsList = [];
          var s: any;
          for(s of response.data.songs){
            var newSong : Song = {id: 0, title: '', description: '', time: 0, artists: [], genres: [], links: [], uploadedBy: '', uploadedDate: ''};
            //({s.title, s.description, s.time, s.artists, s.genres, s.links, s.uploadedBy} = response.data[0]);
            newSong.id = s.songId;  //!! //to link to song detailed page
            newSong.title = s.title;
            newSong.description = s.description;
            newSong.time = s.time;
            newSong.artists = s.artists;
            newSong.genres = s.genres;
            newSong.links = s.links;
            newSong.uploadedBy = s.uploadedBy;
            newSong.uploadedDate = s.createdAt;  //!!

            this.songsList.push(newSong);
          }

          //console.log(this.songsList);  ////

          this.readyToDisplaySongs = true;

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

      search () {
        console.log("clicked search button");
      },

      handlePagination(page : number) {
        this.loadPage(page);
      },

      ...mapActions(useAccountStore, ['refreshJWT']),
      ...mapActions(useSongStore, ['saveSearchCriteria']),
    },

    computed: {
      ...mapState(useAccountStore, ['accessToken', 'refreshToken']),
      ...mapState(useSongStore, ['searchUploadedByMe', 'searchByTitle', 'searchByArtist', 'searchByGenre', 'searchSongFlags', 'currentPage']),
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

.horizontal-tiling {
  max-width: 520px;
}

</style>