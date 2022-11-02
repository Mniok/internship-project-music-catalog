<template>
  <v-container class="d-flex m-0 p-0">
    <v-icon :small="size == 'small'" :class="site" color="#ffffff00">mdi-link</v-icon>
  </v-container>
</template>


<script lang="ts">
  export default {
    name: "siteIcon",

    props: {
      site: String,
      size: String
    },
  }
</script>

<style scoped>

.youtube {
  background-image: url(../assets/yt_logo_rgb_light_cropped.png);
  background-size: contain;
}

.spotify {
  background-image: url(../assets/Spotify_Logo_RGB_Green_cropped.png);
  background-size: contain;
}

.applemusic {
  background-image: url(../assets/Apple_Music_Icon_RGB_small.svg);
  background-size: contain;
}

.bandcamp {
  background-image: url(../assets/bandcamp-logo-circle-32.png);
  background-size: contain;
}

.soundcloud {
  background-image: url(../assets/soundcloud-logo-orange-white.png);
  background-size: contain;
}

</style>