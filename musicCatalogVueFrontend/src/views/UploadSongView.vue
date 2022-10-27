<template>
  <div>
    <h1>upload song</h1>
    <p>song id {{$route.params.id}}: {{songTitle}} by {{artistsString}}</p>
    <p>{{songDescription}}</p>
    <p>length: {{songTime | timeFormat}}</p>
    <p>genres: {{genresString}}</p>
    <p>uploaded by {{songUploader}} on {{songUploadedOn}}</p>
  </div>
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
      songArtists: [],
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
p {
  color: bisque;
}

</style>