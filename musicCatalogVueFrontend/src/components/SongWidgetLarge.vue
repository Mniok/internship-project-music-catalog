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
          <strong>{{songTitle}}</strong>
          <span v-if="songTime>0"> {{songTime | timeFormat}}</span><strong>, by</strong>
          <v-chip 
            v-for="artist in songArtists"
            link
            pill
            color="indigo lighten-1"
            text-color="blue lighten-5"
            class="ml-3"
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
              class="mr-2"
            >{{genre}}
            </v-chip>
          </h5>
        </div>

        <div id="links" class="m2-5 mb-5">
          <v-chip
            v-if="!!youtubeLink"
            pill
            link :href="youtubeLink"
            dark color="red lighten-1"
            class="no-decorators mr-3 mt-3"
            ><site-icon site="youtube" class="mr-2"/>
            view on YouTube
            <v-icon>mdi-play</v-icon>
          </v-chip>
          <v-chip
            v-if="!!spotifyLink"
            pill
            link :href="spotifyLink"
            dark color="green darken-3"
            class="no-decorators mr-3 mt-3"
            ><site-icon site="spotify" class="mr-2"/>
            view on Spotify
            <v-icon>mdi-play</v-icon>
          </v-chip>
          <v-chip
            v-if="!!applemusicLink"
            pill
            link :href="applemusicLink"
            dark color="pink darken-2"
            class="no-decorators mr-3 mt-3"
            ><site-icon site="applemusic" class="mr-2"/>
            view on Apple Music
            <v-icon>mdi-play</v-icon>
          </v-chip>
          <v-chip
            v-if="!!bandcampLink"
            pill
            link :href="bandcampLink"
            dark color="cyan lighten-2"
            class="no-decorators mr-3 mt-3"
            ><site-icon site="bandcamp" class="mr-2"/>
            view on Bandcamp
            <v-icon>mdi-play</v-icon>
          </v-chip>
          <v-chip
            v-if="!!soundcloudLink"
            pill
            link :href="soundcloudLink"
            dark color="orange lighten-2"
            class="no-decorators mr-3 mt-3"
            ><site-icon site="soundcloud" class="mr-2"/>
            view on Soundcloud
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
  import { Link, restoreYoutubeLink, restoreSpotifyLink, restoreApplemusicLink, restoreBandcampLink, restoreSoundcloudLink } from '../service/linkHelpers';
  import SiteIcon from './SiteIcon.vue';

  export default {
    name: "songWidgetLarge",

    components: {
      SiteIcon,
    },

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
            return restoreYoutubeLink(link.linkBody);
          }
        };
        return "";
      },

      spotifyLink() : string {
        var link : any;
        for (link of this.songLinks!){
          if (link.toSite == "spotify"){
            return restoreSpotifyLink(link.linkBody);
          }
        };
        return "";
      },

      applemusicLink() : string {
        var link : any;
        for (link of this.songLinks!){
          if (link.toSite == "applemusic"){
            //console.log("youtube: " + link.linkBody); ////
            return restoreApplemusicLink(link.linkBody);
          }
        };
        return "";
      },

      bandcampLink() : string {
        var link : any;
        for (link of this.songLinks!){
          if (link.toSite == "bandcamp"){
            return restoreBandcampLink(link.linkBody);
          }
        };
        return "";
      },

      soundcloudLink() : string {
        var link : any;
        for (link of this.songLinks!){
          if (link.toSite == "soundcloud"){
            return restoreSoundcloudLink(link.linkBody);
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

h3>strong, h3>span>strong, h5, h6 {
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