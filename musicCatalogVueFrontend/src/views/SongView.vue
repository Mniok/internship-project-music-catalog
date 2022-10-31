<template>
  <song-widget-large
    :song-title="songTitle"
    :song-description="songDescription"
    :song-time="songTime"
    :song-artists="songArtists"
    :song-genres="songGenres"
    :song-links="songLinks"
    :song-uploader="songUploader"
    :song-uploaded-date="songUploadedOn"
  />
</template>

<script lang="ts">
  import Vue from 'vue'
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';
  import { Link } from '../service/linkHelpers';
  import SongWidgetLarge from '../components/SongWidgetLarge.vue';

  export default Vue.extend({
    name: 'Song',

    components: {
      SongWidgetLarge,
    },

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

    created() {
        axios.get('https://localhost:7026/api/Song/song/' + this.$route.params.id, {
          //id: this.$route.params.id
        },
        {
          headers: { 'Authorization': `bearer ${this.accessToken}` }
        })
        .then(response => { 
          //console.log(response.data); ////
          this.song = response.data;

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
          }


          //console.log(this.songArtists);  ////


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

    computed: {
      ...mapState(useAccountStore, ['accessToken', 'refreshToken']),
    },

    

  })
</script>
