<template>
  <register-form class="position-form" />
</template>

<script lang="ts">
  import Vue from 'vue'
  import RegisterForm from '../components/RegisterForm.vue'

  export default Vue.extend({
    name: 'Register',

    components: {
      RegisterForm,
    },
  })
</script>

<style scoped>
  div {
    width: 60%;
    margin-top:15vh;
    margin-bottom:15vh;
  }
</style>