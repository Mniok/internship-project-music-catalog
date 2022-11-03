<template>
  <v-container class="p-0 mb-6">
    <v-card class="mb-2 p-2 pb-0" dark>
      <v-row class="d-flex">
        <v-col id="left-column" class="d-flex p-0 mr-2 mb-3
        ">
          <v-container class="mr-5 pb-0"> <!-- bez tego style width v-sheet jest zciśnięty -->
            <v-sheet id="image-preview" height="88px" width="88px" rounded outlined elevation="8" color="blue-grey darken-3" class="d-flex justify-end align-end pb-0 pr-1">
              <div class="align-right float-end"><span v-if="song.time>0"> {{song.time | timeFormat}}</span></div>
            </v-sheet>
          </v-container>
        </v-col>

        <v-col id="right-column" class="d-flex p-0">
          <v-container>
            <h5 class="text-truncate">
              <strong><u>
                <router-link :to="/song/ + song.id">{{song.title}}</router-link>
              </u></strong>
            </h5>

            <div id="artists">
              <strong>By</strong> <v-chip 
                v-for="artist in song.artists"
                link
                small pill
                color="indigo lighten-1"
                text-color="blue lighten-5"
                class="mr-2 mb-2"
              >{{artist.artistName}}
              </v-chip>
            </div>

            <h6 v-if="song.genres?.length == 0">No genres found.</h6>
            <div v-else>
                <v-chip
                  v-for="genre in song.genres"
                  link
                  small pill
                  color="deep-purple lighten-2"
                  text-color="blue lighten-5"
                  class="mr-2 mb-2 text-truncate"
                >{{genre.genreName}}
                </v-chip>
            </div>

          </v-container>
        </v-col>
      </v-row>

      <v-row class="m-1 mt-1">
        <div id="description">
          <p class="text-truncate">{{song.description}}</p>
        </div>
      </v-row>

      <v-row class="m-0 p-0">
        <div id="links">
          <v-chip
            v-if="!!youtubeLink"
            small pill
            link :href="youtubeLink"
            dark color="red lighten-1"
            class="no-decorators mr-3 mb-2"
            >view on <site-icon site="youtube" size="small" class="ml-1"/>
          </v-chip>
          <v-chip
            v-if="!!spotifyLink"
            small pill
            link :href="spotifyLink"
            dark color="green darken-3"
            class="no-decorators mr-3 mb-2"
            >view on <site-icon site="spotify" size="small" class="ml-1"/>
          </v-chip>
          <v-chip
            v-if="!!applemusicLink"
            small pill
            link :href="applemusicLink"
            dark color="pink darken-2"
            class="no-decorators mr-3 mb-2"
            >view on <site-icon site="applemusic" size="small" class="ml-1"/>
          </v-chip>
          <v-chip
            v-if="!!bandcampLink"
            small pill
            link :href="bandcampLink"
            dark color="cyan lighten-2"
            class="no-decorators mr-3 mb-2"
            >view on <site-icon site="bandcamp" size="small" class="ml-1"/>
          </v-chip>
          <v-chip
            v-if="!!soundcloudLink"
            small pill
            link :href="soundcloudLink"
            dark color="orange lighten-2"
            class="no-decorators mr-3 mb-2"
            >view on <site-icon site="soundcloud" size="small" class="ml-1"/>
          </v-chip>
        </div>
      </v-row>
      
    </v-card>
    <p class="uploader-info mt-2 ml-1">Uploaded by <strong>{{song.uploadedBy.userName}}</strong><br/>
    on {{song.uploadedDate | dateFormat}}</p>
  </v-container>
</template>


<script lang="ts">
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';
  import { Link, restoreYoutubeLink, restoreSpotifyLink, restoreApplemusicLink, restoreBandcampLink, restoreSoundcloudLink } from '../service/linkHelpers';
  import SiteIcon from './SiteIcon.vue';

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

  export default {
    name: "songWidget",

    components: {
      SiteIcon,
    },

    props: {
      song: {
        type: Object,
        default(rawProps : any) {
          return {
            id: Number,
            title: String,
            description: String,
            time: Number,
            artists: Array<String>(),
            genres: Array<String>(),
            links: Array<Link>(),
            uploadedBy: String,
            uploadedDate: String
          }
        }
      }
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
        if (value.split == undefined) ///prevents value.split is not a function error
          return "";

        var date : string; var time : string;
        [date, time] = value.split("T");
        time = time?.split(".").at(0)!;

        //time = time?.split(":").at(0) + ':' + time?.split(":").at(1);   ///strip seconds
        time = time?.substring(0, 5);   ///strip seconds    //both of these keep throwing type errors in console no matter what I try but work flawlessly

        return date + " " + time;
      },

    },

    computed: {
      youtubeLink() : string {
        var link : any;
        for (link of this.song.links){
          if (link.toSite == "youtube"){
            //console.log("youtube: " + link.linkBody); ////
            return restoreYoutubeLink(link.linkBody);
          }
        };
        return "";
      },

      spotifyLink() : string {
        var link : any;
        for (link of this.song.links){
          if (link.toSite == "spotify"){
            return restoreSpotifyLink(link.linkBody);
          }
        };
        return "";
      },

      applemusicLink() : string {
        var link : any;
        for (link of this.song.links){
          if (link.toSite == "applemusic"){
            //console.log("youtube: " + link.linkBody); ////
            return restoreApplemusicLink(link.linkBody);
          }
        };
        return "";
      },

      bandcampLink() : string {
        var link : any;
        for (link of this.song.links){
          if (link.toSite == "bandcamp"){
            return restoreBandcampLink(link.linkBody);
          }
        };
        return "";
      },

      soundcloudLink() : string {
        var link : any;
        for (link of this.song.links){
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

#artists>strong, h5, h6 {
  color: #9FA8DA; /* indigo lighten-3 */
}

#left-column {
  min-width:92px;
  max-width:92px;
}

.uploader-info {
  color: #B0BEC5; /* blue-grey lighten-3 */
  font-size: 0.8em;
}

#description {
  max-width: 100%;
}

#description > p {
  white-space: pre;
  color: #B0BEC5; /* blue-grey lighten-3 */
  font-size: 0.9em;
}

a { /* to remove styling from router-link */
  text-decoration: inherit !important;
  color: inherit;
}

a:hover {
  color: #E8EAF6; /* indigo lighten-5 */
}

</style>