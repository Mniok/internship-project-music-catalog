<template>
  <v-container class="m-0 p-0">
    <v-card class="d-flex" dark>
      <v-container id="left-column" class="mr-5 pb-0" style="width:90px"> <!-- bez tego style width v-sheet jest zciśnięty -->
        <v-sheet id="image-preview" height="88px" width="88px" rounded outlined elevation="8" color="blue-grey darken-3" class="d-flex">
          <div class="float-right float-end"><span v-if="song.time>0"> {{song.time | timeFormat}}</span></div>
        </v-sheet>
      </v-container>

      <v-container id="right-column">
        <h5>
          <strong><u>{{song.title}}</u></strong>
        </h5>

        <strong>By</strong> <v-chip 
          v-for="artist in song.artists"
          link
          small pill
          color="indigo lighten-1"
          text-color="blue lighten-5"
          class="mr-2"
        >{{artist.artistName}}
        </v-chip>

        <h6 v-if="song.genres?.length == 0">No genres found.</h6>
        <div v-else>
            <v-chip
              v-for="genre in song.genres"
              link
              small pill
              color="deep-purple lighten-2"
              text-color="blue lighten-5"
              class="mr-2"
            >{{genre.genreName}}
            </v-chip>
        </div>

        <div id="description">
          <p>{{song.description}}</p>
        </div>

      </v-container>

      <div id="links" class="mt-5 mb-5">
        <v-chip
          v-if="!!youtubeLink"
          small pill
          link :href="youtubeLink"
          dark color="red lighten-1"
          class="no-decorators mr-3"
          >view on <site-icon site="youtube" size="small" class="ml-1"/>
        </v-chip>
        <v-chip
          v-if="!!spotifyLink"
          small pill
          link :href="spotifyLink"
          dark color="green darken-3"
          class="no-decorators mr-3"
          >view on <site-icon site="spotify" size="small" class="ml-1"/>
        </v-chip>
        <v-chip
          v-if="!!applemusicLink"
          small pill
          link :href="applemusicLink"
          dark color="pink darken-2"
          class="no-decorators mr-3"
          >view on <site-icon site="applemusic" size="small" class="ml-1"/>
        </v-chip>
        <v-chip
          v-if="!!bandcampLink"
          small pill
          link :href="bandcampLink"
          dark color="cyan lighten-2"
          class="no-decorators mr-3"
          >view on <site-icon site="bandcamp" size="small" class="ml-1"/>
        </v-chip>
        <v-chip
          v-if="!!soundcloudLink"
          small pill
          link :href="soundcloudLink"
          dark color="orange lighten-2"
          class="no-decorators mr-3"
          >view on <site-icon site="soundcloud" size="small" class="ml-1"/>
        </v-chip>
      </div>
          
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

    /*data: () => ({
      title: String,
      description: String,
      time: 0,
      artists: Array<String>(),
      genres: Array<String>(),
      links: Array<Link>(),
      uploadedBy: String,
      uploadedDate: String
    }),

    created() {
      // moves values from prop song to option data, to prevent typeerrors when value is read before prop is fully processed

      this.title = this.song?.title;
      this.description = this.song?.description;
      this.time = this.song?.time;

      var artistsList : Array<String> = [];
      for (var artist of this.song?.artists){
        artistsList.push(artist.artistName);
      }
      this.artists = artistsList;
      console.log(this.artists) 

      var genresList : Array<String> = [];
      for (var genre of this.song?.genres){
        genresList.push(genre.genreName);
      }
      this.genres = genresList;

      var linksList : Array<List> = Array<List>();
      for (var link of this.song?.links){
        linksList.push({toSite: link.toSite, linkBody: link.linkBody});
      }
      this.links = linksList;

      console.log(this.links) 
      this.uploadedBy = this.song?.uploadedBy.userName;
      this.uploadedDate = this.song?.uploadedDate;
    },*/

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

.v-card>*>strong, h5, h6 {
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