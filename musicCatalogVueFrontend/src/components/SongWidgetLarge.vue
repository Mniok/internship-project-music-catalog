<template>
  <v-container class="d-flex">
    <v-container id="left-column" class="mr-5 pb-0" style="width:220px"> <!-- bez tego style width v-sheet jest zciśnięty -->
        <v-sheet id="image-preview" height="220px" width="220px" rounded outlined elevation="8" color="blue-grey darken-3">
          <span>[image]</span>
        </v-sheet>
        <p class="uploader-info mt-2 ml-1">Uploaded by <strong>{{songUploader}}</strong><br/>
        on {{songUploadedDate | dateFormat}}</p>
      </v-container>

      <v-container id="right-column" class="ml-12" >
        <h3 class="mb-4">
          <strong><u>{{songTitle}}</u></strong> {{songTime | timeFormat}}<strong>, by </strong>
          <v-chip 
            v-for="artist in songArtists"
            link
            pill
            color="indigo lighten-1"
            text-color="blue lighten-5"
            class="mr-3"
          >{{artist}}
          </v-chip>
        </h3>
        <h6 v-if="songGenres?.length == 0">No genres found.</h6>
        <div v-else>
          <h5>Genres:
            <v-chip
              v-for="genre in songGenres"
              link
              small pill
              color="indigo lighten-1"
              text-color="blue lighten-5"
              class="mr-3"
            >{{genre}}
            </v-chip>
          </h5>
        </div>

        <div id="links" class="mt-5">
          <v-chip v-if="!!youtubeLink">
            YouTube link
            <v-icon>mdi-play</v-icon>
          </v-chip>
        </div>

        <div id="description">
          <p>{{songDescription}}</p>
        </div>

      </v-container>
  </v-container>
</template>


<script lang="ts">
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';
  import { Link } from '../service/linkHelpers';

  export default {
    name: "songWidgetLarge",

    props: {
      songTitle: String,
      songTime: Number,
      songDescription: String,
      songArtists: Array,
      songGenres: Array,
      songLinks: Array,
      songUploader: String,
      songUploadedDate: String,
    },

    filters: {
      timeFormat(value : number) {
        if (!value) return '';

        var seconds : number = value % 60;
        var minutes : number = ((value-seconds) % (60*60))/60;
        var hours   : number = ((value -minutes*60 -seconds)) / (60*60);

        if(hours > 0)
          return `[${hours}:${String(minutes).padStart(2,'0')}:${String(seconds).padStart(2,'0')}]`

        return `[${minutes}:${String(seconds).padStart(2,'0')}]`;

      },

      dateFormat(value: string) {
        var date : string; var time : string;
        [date, time] = value.split("T");
        time = time?.split(".").at(0)!;

        //time = time?.split(":").at(0) + ':' + time?.split(":").at(1);   ///strip seconds
        time = time?.substring(0, 5);   ///strip seconds    //both of these keep throwing type errors in console no matter what I try but work flawlessly

        return date + " " + time;
      },

      multiline(value: string) {
        return value.replace("\n", "\n\r")
      }
    },

    computed: {
      youtubeLink() : string {
        var link : any;
        for (link of this.songLinks!){
          if (link.toSite == "youtube"){
            //console.log("youtube: " + link.linkBody); ////
            return link.linkBody;
          }
        };

        return "";
      },
    },
  }
</script>

<style scoped>
h3 {
  color: #BBDEFB; /* blue lighten-4 */
}

h3 > strong, h5, h6 {
  color: #9FA8DA; /* indigo lighten-3 */
}

.uploader-info {
  color: #B0BEC5; /* blue-grey lighten-3 */
  font-size: 0.8em;
}

#description > p {
  white-space: pre;
  color: #B0BEC5; /* blue-grey lighten-3 */
}

</style>