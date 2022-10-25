<template>
  <div><p>song id {{$route.params.id}}: {{songTitle}} by {{artistsString}}</p></div>
</template>

<script lang="ts">
  import Vue from 'vue'
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';

  export default Vue.extend({
    name: 'Song',

    data: () => ({
      song: '',
      songTitle: '',
      songArtists: Array<String>(),
    }),

    created() {
      var loginRequest = axios.get('https://localhost:7026/api/Song/song/' + this.$route.params.id, {
          //id: this.$route.params.id
        },
        {
          headers: { 'Authorization': `bearer ${this.accessToken}` }
        })
        .then(response => { 
          console.log(response.data); ////
          this.song = response.data;

          this.songTitle = response.data.title; 


          for(var i=0; i<response.data.artists.$values.length; i++){
            this.songArtists.push(response.data.artists.$values.at(i).artistName);
          }

          //this.songArtists.push("abcd");
          console.log(this.songArtists);  ////


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
      artistsString(){
        return this.songArtists.join(", ");
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken']),
    }

  })
</script>


<style scoped>
p {
  color: bisque;
}
</style>