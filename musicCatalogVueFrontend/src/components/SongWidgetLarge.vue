<template>
  <v-container>
    
  </v-container>
</template>


<script>
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';
  import { Link } from '../service/linkHelper';

  export default {
    name: "songWidgetBig",

    props: {
      songTitle: String,
      songTime: Number,
      songDescription: String,
      songArtists: Array<String>,
      songGenres: Array<String>,
      songLinks: Array<Link>,
      songUploader: String,
      songUploadedDate: String,
    },
  }
</script>