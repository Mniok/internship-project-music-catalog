<template>
  <login-form class="position-form" />
</template>

<script lang="ts">
  import Vue from 'vue'
  import LoginForm from '../components/LoginForm.vue'

  export default Vue.extend({
    name: 'Home',

    components: {
      LoginForm,
    },
  })
</script>

<style scoped>
  div {
    width: 60%;
    margin-top:15vh;
  }
</style>