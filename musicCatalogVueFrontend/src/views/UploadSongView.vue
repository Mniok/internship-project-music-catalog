<template>
  <v-container class="d-flex mb-16 pb-16">
    <v-form class="d-flex" style="width:100%">
      <v-container id="left-column" style="width:40%">
        <v-sheet id="image-preview" height="220px" width="220px" rounded outlined elevation="8" color="blue-grey darken-3">
          <span>[choose an image]</span>
        </v-sheet>
      </v-container>

      <v-divider vertical class="blue-grey darken-1 mt-4"/>

      <v-container id="right-column" class="float-right pb-0">
        <!-- song title input -->
        <v-text-field
          dark color="indigo lighten-2"
          v-model="songTitle"
          outlined
          label="Song Title:"
          class="mt-4 wider-field"
        >
          <v-icon 
            large
            slot="prepend" 
            color="blue-grey"
            style="margin-top:-8px"
          >mdi-tag-text-outline <!-- negative margin centers icon to text field -->
          </v-icon>
        </v-text-field>


        <p class="category-title">                                            <!-- artists inputs list -->
          <v-icon small color="blue-grey">mdi-account-box-multiple</v-icon>
          ARTIST(S):
        </p>
        <v-text-field v-for="index in artistsCount" :key="index"
          dark color="indigo lighten-2"
          v-model="songArtists[index-1]"
          :label="index | labelNumbers('Artist')"
          class="ml-5"
        >
        </v-text-field>

        <p class="category-title mt-8">                                       <!-- genres inputs list -->
          <v-icon small color="blue-grey">mdi-music-box-multiple</v-icon>
          GENRE(S):
        </p>
        <v-text-field v-for="index in genresCount" :key="index"
          dark color="indigo lighten-2"
          v-model="songGenres[index-1]"
          :label="index | labelNumbers('Genre')"
          class="ml-5"
        >
        </v-text-field>

        <p class="category-title mt-8">                                       <!-- genres inputs list -->
          <v-icon small color="blue-grey">mdi-music-box-multiple</v-icon>
          DESCRIPTION:
        </p>
        <v-textarea
          dark color="indigo lighten-2" 
          v-model="songDescription"
          outlined
          label="Song Description:"
          class="wider-field ml-5 pb-0"
        >
        </v-textarea>

      </v-container>
    </v-form>
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
      songArtists: Array<String>(),
      songGenres: Array<String>(),
      songLinks: Array<Link>(),
      songUploader: '',
      songUploadedOn: '',
    }),


    computed: {
      artistsCount(){   //ile wyświetlić inputów - o 1 więcej niż najwyższy niepusty
        //console.log(this.songArtists);  ////

        if(!!this.songArtists.at(this.songArtists.length-1)){   //przy dodawaniu
          return this.songArtists.length + 1;
        }

        for(var i : number = this.songArtists.length - 1; i>=0; i--)   //przy zmniejszaniu
          if(!!this.songArtists.at(i))
            return i+2;

        return 1;
      },

      genresCount(){   //ile wyświetlić inputów - o 1 więcej niż najwyższy niepusty
        //console.log(this.songArtists);  ////

        if(!!this.songGenres.at(this.songGenres.length-1)){   //przy dodawaniu
          return this.songGenres.length + 1;
        }

        for(var i : number = this.songGenres.length - 1; i>=0; i--)   //przy zmniejszaniu
          if(!!this.songGenres.at(i))
            return i+2;

        return 1;
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

      labelNumbers(index : number, labelFor: string){
        if (index < 2)
          return labelFor
        return labelFor + ' ' + index.toString();
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