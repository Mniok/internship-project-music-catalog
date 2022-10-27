<template>
  <v-container class="d-flex">
    <v-container id="left-column" style="width:40%">
      <v-sheet id="image-preview" height="220px" width="220px" rounded outlined elevation="8" color="blue-grey darken-3">
        <span>[choose an image]</span>
      </v-sheet>
    </v-container>

    <v-divider vertical class="blue-grey darken-1"/>

    <v-container id="right-column" class="float-right">
      <v-text-field
        dark color="indigo lighten-2"
        v-model="songTitle"
        outlined
        label="Song Title:"
        class="search-inputs mt-4 wider-field"
      >
        <v-icon 
          large
          slot="prepend" 
          color="blue-grey"
          style="margin-top:-8px"
        >mdi-tag-text-outline <!-- negative margin centers icon to text field -->
        </v-icon>
      </v-text-field>


      <p class="category-title">
        <v-icon small color="blue-grey">mdi-account-box-multiple</v-icon>
        ARTIST(S):
      </p>
      <v-text-field v-for="(artist, index) in songArtists"
        dark color="indigo lighten-2"
        v-model="songArtists[index]"
        :label="index | artistLabel"
        class="search-inputs ml-5"
      >
      </v-text-field>

    </v-container>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue'
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';

  interface Link {
    toSite: string,
    linkBody: string
  }

  export default Vue.extend({
    name: 'UploadSong',

    data: () => ({
      song: '',
      songTitle: '',
      songDescription: '',
      songTime: 0,
      //songArtists: Array<String>(),
      //songGenres: Array<String>(),
      //songLinks: Array<Link>(),
      songArtists: Array<String>("", "", ""),
      songGenres: [],
      songLinks: [],
      songUploader: '',
      songUploadedOn: '',
    }),


    computed: {
      artistsString(){
        return this.songArtists.join(", ");
      },

      genresString(){
        return this.songGenres.join(", ");
      },

      /*linksString(){
        var links : Array<String> = ["(TEMPORARY before adding links is implemented): "];
        this.songLinks.forEach(l => {
          links.push( l.toSite + ".com/" + l.linkBody);
        });
        return links.join(", ");
      },*/

      artistsCount(){
        return 5;
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken']),
    },

    filters: {
      timeFormat(value : number) {
        //if (!value) return '';

        var seconds : number = value % 60;
        var minutes : number = ((value-seconds) % (60*60))/60;
        var hours   : number = ((value -minutes*60 -seconds)) / (60*60);

        if(hours > 0)
          return `${hours}:${String(minutes).padStart(2,'0')}:${String(seconds).padStart(2,'0')}`

        return `${minutes}:${String(seconds).padStart(2,'0')}`;

      },

      artistLabel(index : number){
        if (index < 1)
          return "Artist"
        return "Artist " + index.toString();
        //z jakiegoś powodu podświetla jako błąd? ale działa i wg. dokumentacji tak był powinno
        // :label="'Artist ' + ( index==0 ? '' : (index+1).toString() )"    nie podkreśla jako błąd, ale jest niezbyt czyste
      }

      /*dateFormat(value: string) {
        var date : string; var time : string;
        [date, time] = value!.split("T")!;   //! is supposed to tell tsc this value will newer be null, but doesn't fix undefined error in console
        time = time!.split(".").at(0)!;

        //time = time?.split(":").at(0) + ':' + time?.split(":").at(1);   ///strip seconds
        time = time!.substring(0, 5);   ///strip seconds    //both of these keep throwing type errors in console no matter what I try but work flawlessly

        return date + " " + time;
      }*/
    },

  })
</script>


<style scoped>
.category-title {
  color: #B0BEC5; /* blue-grey lighten-3 */
  font-size: 0.8em;
}

</style>