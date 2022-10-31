<template>
  <v-container class="d-flex mb-16 pb-16">
    <v-form class="d-flex" style="width:100%">
      <v-container id="left-column" class="mr-5 pb-0" style="width:40%">
        <v-sheet id="image-preview" height="220px" width="220px" rounded outlined elevation="8" color="blue-grey darken-3">
          <span>[choose an image]</span>
        </v-sheet>

        <p class="category-title mt-8">                                            <!-- song length input -->
          <v-icon small color="blue-grey">mdi-timer-outline</v-icon>
          TIME:
        </p>
        <v-text-field
          dark color="indigo lighten-2"
          v-model="songTime"
          label="How long is this song?"
          class="ml-5"
          type="time"
          step="1"
          messages="hh:mm:ss"
        ></v-text-field>

        <p class="category-title mt-8">                                            <!-- list of link inputs -->
          <v-icon small color="blue-grey">mdi-link-box-variant</v-icon>
          LINK(S)**:
        </p>
        <v-text-field
          dark color="red"
          v-model="youtubeLink"
          :rules="youtubeLinkRules"
          label="YouTube link:"
        >
          <v-icon 
            slot="prepend" 
            color="red darken-4"
          >mdi-youtube</v-icon>
        </v-text-field>

        <v-text-field
          dark color="green"
          v-model="spotifyLink"
          :rules="spotifyLinkRules"
          label="Spotify link:"
        >
          <v-icon 
            slot="prepend" 
            color="green darken-4"
          >mdi-spotify</v-icon>
        </v-text-field>

        <v-text-field
          dark color="purple"
          v-model="itunesLink"
          :rules="itunesLinkRules"
          label="Itunes link:"
        >
          <v-icon 
            slot="prepend" 
            color="purple darken-4"
          >mdi-soundcloud</v-icon>
        </v-text-field>

        <v-text-field
          dark color="blue"
          v-model="bandcampLink"
          :rules="bandcampLinkRules"
          label="Bandcamp link:"
        >
          <v-icon 
            slot="prepend" 
            color="blue darken-4"
          >mdi-soundcloud</v-icon>
        </v-text-field>

        <v-text-field
          dark color="orange"
          v-model="soundcloudLink"
          :rules="soundcloudLinkRules"
          label="Soundcloud link:"
        >
          <v-icon 
            slot="prepend" 
            color="orange darken-4"
          >mdi-soundcloud</v-icon>
        </v-text-field>

      </v-container>

      <v-divider vertical class="blue-grey darken-1 mt-4"/>

      <v-container id="right-column" class="float-right pb-0">
        <!-- song title input -->
        <v-text-field
          dark color="indigo lighten-2"
          v-model="songTitle"
          outlined
          label="Song Title*:"
          class="mt-4 mb-4 wider-field"
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
          ARTIST(S)**:
        </p>
        <v-text-field v-for="index in artistsCount" :key="'artist'+index"
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
        <v-text-field v-for="index in genresCount" :key="'genre'+index"
          dark color="indigo lighten-2"
          v-model="songGenres[index-1]"
          :label="index | labelNumbers('Genre')"
          class="ml-5"
        >
        </v-text-field>

        <p class="category-title mt-8">                                       <!-- genres inputs list -->
          <v-icon small color="blue-grey">mdi-comment-text</v-icon>
          DESCRIPTION:
        </p>
        <v-textarea
          dark color="indigo lighten-2" 
          v-model="songDescription"
          outlined
          label="Song Description:"
          class="wider-field ml-5"
        >
        </v-textarea>

        <v-row class="mr-0">
          <v-card style="background-color:#00000000" elevation="0" class="v-messages theme--dark v-messages__message mt-4 ml-8">
              <span >* required <br/>** at least 1 required</span>
          </v-card>
          <v-spacer/>
          <v-btn
            :disabled="!valid"
            dark color="success"
            class="mt-4 ml-0 mr-0 float-right"
            @click="submitSong"
          >
            Submit song
          </v-btn>
        </v-row>

      </v-container>
    </v-form>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue'
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';

  function cleanupLink(linkEnd:string) : string | undefined {
    //console.log(linkEnd);   ////
    //console.log(linkEnd?.split('/').at(0)?.split('?').at(0)?.split('&').at(0));   ////
    return linkEnd?.split('/').at(0)?.split('?').at(0)?.split('&').at(0);
  }

  var youtubeLinkBase : string = "www.youtube.com/watch?v=";      //ex. https://www.youtube.com/watch?v=ORnvO1VyYMk   //Man On The Silver Mountain btw
  var youtubeLinkShare : string = "youtu.be/";                    //ex. https://youtu.be/ORnvO1VyYMk
  var youtubeLinkShorts : string = "www.youtube.com/shorts/";     //ex. https://www.youtube.com/shorts/BZ_276VGGv4
  function trimYoutubeLink(link : string) : string | boolean | undefined {
    if (!link)
      return false;

    if (link.startsWith("https://"))
      link = link.substring("https://".length);

    if (link.startsWith("http://"))
      link = link.substring("http://".length);

    if (link.substring(0, youtubeLinkBase.length) == youtubeLinkBase)
      return cleanupLink( link.substring(youtubeLinkBase.length) );

    if (link.substring(0, youtubeLinkShare.length) == youtubeLinkShare)
      return cleanupLink( link.substring(youtubeLinkShare.length) );

    if (link.substring(0, youtubeLinkShorts.length) == youtubeLinkShorts)
      return cleanupLink( link.substring(youtubeLinkShorts.length) );

    return false;
  }

  function trimSpotifyLink(link : string) : string | boolean | undefined {
    return false;
  }

  function trimItunesLink(link : string) : string | boolean | undefined {
    return false;
  }

  function trimBandcampLink(link : string) : string | boolean | undefined {
    return false;
  }

  var soundcloudLinkBase : string = "soundcloud.com/";           //ex. https://soundcloud.com/jimihendrix/3b-wav , https://soundcloud.com/creedence-clearwater-revival/fortunate-son-at-the-royal //ten 2gi not available in poland
  function trimSoundcloudLink(link : string) : string | boolean | undefined {
    if (!link)
      return false;

    if (link.startsWith("https://"))
      link = link.substring("https://".length);

    if (link.startsWith("http://"))
      link = link.substring("http://".length);

    if (link.substring(0, soundcloudLinkBase.length) == soundcloudLinkBase){
      link = link.substring(soundcloudLinkBase.length);
      var linkArtist : string | undefined, linkSong : string | undefined;
      [linkArtist, linkSong] = link.split("/");
      linkArtist = cleanupLink(linkArtist);
      linkSong = cleanupLink(linkSong);
      //console.log(link + " ==> " + linkArtist + " / " + linkSong); ////
      if (!!linkArtist && !!linkSong)
        return linkArtist + '/' + linkSong;
    }

    return false;
  }

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
      songTime: "00:05:00",
      songArtists: Array<String>(),
      songGenres: Array<String>(),
      //songLinks: Array<Link>(),

      youtubeLink: '',
      youtubeLinkRules: [
        /// v => v.substring(0, youtubeLinkBase.length) == youtubeLinkBase || "No valid YouTube link detected",
        v => (!!trimYoutubeLink(v) || v=="") || "No valid YouTube link detected",
      ],
      spotifyLink: '',
      spotifyLinkRules: [
        v => (!!trimSpotifyLink(v) || v=="") || "No valid Spotify link detected",
      ],
      itunesLink: '',
      itunesLinkRules: [
        v => (!!trimItunesLink(v) || v=="") || "No valid Itunes link detected",
      ],
      bandcampLink: '',
      bandcampLinkRules: [
        v => (!!trimBandcampLink(v) || v=="") || "No valid Bandcamp link detected",
      ],
      soundcloudLink: '',
      soundcloudLinkRules: [
        v => (!!trimSoundcloudLink(v) || v=="") || "No valid Soundcloud link detected",
      ],
    }),


    methods: {
      submitSong () {
        axios.post('https://localhost:7026/api/Song/upload', 
        {
          title: this.songTitle,
          time: this.songTimeInt,
          description: this.songDescription,
          artists: this.songArtistsNonEmpty,
          genres: this.songGenresNonEmpty,
          links: this.songLinks
        }, 
        {
          headers: { 'Authorization': `bearer ${this.accessToken}` }
        })
        .then(response => {
          //console.log(response); ////

          this.$router.push({
            name: 'song',
            params: { id: response.data.songId }
          });

        })
        .catch(function (error) {
          console.log(error);
          if(error.response == undefined){
            alert("Couldn't connect to the server. Try again later.");
          }
          if(error.response.status == 401){
            alert("Token expired. Log in again and retry.");
          }
        });
      },
    },


    computed: {
      artistsCount() : number {   //ile wyświetlić inputów - o 1 więcej niż najwyższy niepusty
        //console.log(this.songArtists);  ////
        //console.log(this.songTime + " ==> " + this.songTimeInt) ////
        //console.log(this.songLinks); ////

        if(!!this.songArtists.at(this.songArtists.length-1)){   //przy dodawaniu
          return this.songArtists.length + 1;
        }

        for(var i : number = this.songArtists.length - 1; i>=0; i--)   //przy zmniejszaniu
          if(!!this.songArtists.at(i))
            return i+2;

        return 1;
      },

      genresCount() : number{   //ile wyświetlić inputów - o 1 więcej niż najwyższy niepusty
        //console.log(this.songGenres);  ////

        if(!!this.songGenres.at(this.songGenres.length-1)){   //przy dodawaniu
          return this.songGenres.length + 1;
        }

        for(var i : number = this.songGenres.length - 1; i>=0; i--)   //przy zmniejszaniu
          if(!!this.songGenres.at(i))
            return i+2;

        return 1;
      },

      songTimeInt() : number{
        var hours:string, minutes:string, seconds:string;
        [hours, minutes, seconds] = this.songTime.split(":");
        return parseInt(hours)*60*60 + parseInt(minutes)*60 + parseInt(seconds);
      },

      songLinks() : Array<Link>{
        var linksList : Array<Link> = Array<Link>();

        if (!!trimYoutubeLink(this.youtubeLink))    //!!"" -> false btw
          linksList.push({ toSite: 'youtube', linkBody: trimYoutubeLink(this.youtubeLink)?.toString()! });    //toString żeby się typescript uspokoił

        if (!!trimSpotifyLink(this.spotifyLink))
          linksList.push({ toSite: 'spotify', linkBody: trimSpotifyLink(this.spotifyLink)?.toString()! });

        if (!!trimItunesLink(this.itunesLink))
          linksList.push({ toSite: 'itunes', linkBody: trimItunesLink(this.itunesLink)?.toString()! });

        if (!!trimBandcampLink(this.bandcampLink))
          linksList.push({ toSite: 'bandcamp', linkBody: trimBandcampLink(this.bandcampLink)?.toString()! });

        if (!!trimSoundcloudLink(this.soundcloudLink))
          linksList.push({ toSite: 'soundcloud', linkBody: trimSoundcloudLink(this.soundcloudLink)?.toString()! });

        //console.log(this.songLinks); ////
        return linksList;
      },

      songArtistsNonEmpty() : Array<String>{
        return this.songArtists.filter(artist => !!artist);
      },

      songGenresNonEmpty() : Array<String>{
        return this.songGenres.filter(genre => !!genre);
      },

      valid() : boolean{
        //console.log(this.songArtistsNonEmpty); ////
        var noInvalidLinks : boolean = (!!trimYoutubeLink(this.youtubeLink) || this.youtubeLink == "");
        noInvalidLinks &&= (!!trimSpotifyLink(this.spotifyLink) || this.spotifyLink == "");
        noInvalidLinks &&= (!!trimItunesLink(this.itunesLink) || this.itunesLink == "");
        noInvalidLinks &&= (!!trimBandcampLink(this.bandcampLink) || this.bandcampLink == "");
        noInvalidLinks &&= (!!trimSoundcloudLink(this.soundcloudLink) || this.soundcloudLink == "");

        return !!this.songTitle && this.songArtistsNonEmpty.length > 0 && this.songLinks.length > 0 && noInvalidLinks;
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

    },

  })
</script>


<style scoped>
.category-title {
  color: #B0BEC5; /* blue-grey lighten-3 */
  font-size: 0.8em;
}
</style>

<style> /* to nie może być scoped, bo wpływa na element z innego komponentu (v-text-field type="time") */
::-webkit-calendar-picker-indicator {
    filter: invert(1);  /* normalnie input type="time" ma hardcoded czarny background-image */
}
</style>