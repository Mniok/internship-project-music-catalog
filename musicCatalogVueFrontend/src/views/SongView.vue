<template>
  <div>
    <p>song id {{$route.params.id}}: {{songTitle}} by {{artistsString}}</p>
    <p>{{songDescription}}</p>
    <p>length: {{songTime | timeFormat}}</p>
    <p>genres: {{genresString}}</p>
    <p>links: {{linksString}}</p>
    <p>uploaded by {{songUploader}} on {{songUploadedOn | dateFormat}}</p>
  </div>
</template>

<script lang="ts">
  import Vue from 'vue'
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';
  import { Link } from '../service/linkHelpers';

  export default Vue.extend({
    name: 'Song',

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
          console.log(response.data); ////
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
      artistsString(){
        return this.songArtists.join(", ");
      },

      genresString(){
        return this.songGenres.join(", ");
      },

      linksString(){
        var links : Array<String> = ["(TEMPORARY before adding links is implemented): "];
        this.songLinks.forEach(l => {
          links.push( l.toSite + ".com/" + l.linkBody);
        });
        return links.join(", ");
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

      dateFormat(value: string) {
        var date : string; var time : string;
        [date, time] = value!.split("T")!;   //! is supposed to tell tsc this value will newer be null, but doesn't fix undefined error in console
        time = time!.split(".").at(0)!;

        //time = time?.split(":").at(0) + ':' + time?.split(":").at(1);   ///strip seconds
        time = time!.substring(0, 5);   ///strip seconds    //both of these keep throwing type errors in console no matter what I try but work flawlessly

        return date + " " + time;
      }
    },

  })
</script>


<style scoped>
p {
  color: bisque;
}
</style>