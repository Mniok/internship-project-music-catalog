<template>
  <v-container>
    <v-card class="pa-8 d-flex align-center" elevation="4" outlined>
      <v-col>

        <v-row justify="center">
          <v-card-title>
            Log into re:mail
          </v-card-title>
        </v-row>

        <v-divider />
      
        <v-form
          ref="form"
          lazy-validation
        >

          <v-text-field
            v-model="username"
            :rules="usernameRules"
            label="Username:"
            required
          ></v-text-field>

          <v-text-field
            v-model="password"
            :rules="passwordRules"
            label="Password:"
            required
          ></v-text-field>

          <v-spacer />

          <v-row class="mt-4 ml-0 mr-0">
            <v-btn
              :disabled="!valid"
              color="success"
              @click="login"
            >
              Log in
            </v-btn>
            <v-spacer />
            <v-btn
              to="/register"
              color="primary lighten-1"
            >
              Create a new account
            </v-btn>
          </v-row>
        
        </v-form>
    
      </v-col>
    </v-card>
  </v-container>
</template>


<script>
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';

  export default {

    data: () => ({
      username: '',
      usernameRules: [
        v => !!v || 'Please enter Your Username',
      ],
      password: '',
      passwordRules: [
        v => !!v || 'Please enter Your Password',
      ],
    }),

    methods: {
      updateCurrentUserFromAPI(accessToken) {
        //console.log("got here! try GET loggedInAs (with authorization)"); ////
        //console.log(`bearer ${accessToken}`) ////
        axios.get(`https://localhost:7026/api/Account/user`, {
          headers: { 'Authorization': `bearer ${accessToken}` }
        })
        .then(response => {
          this.updateCurrentUser(response.data.username);
          console.log(`logged in as ${this.currentUser}`);
        })
        .catch(function (error) {
          console.log(error);
        });
      },

      /*test() {
        console.log("here");
      },*/
      
      login () {
        this.$refs.form.validate()

        //var getTest = axios('https://localhost:7026/api/Account/testController');
        //console.log(getTest);

        var loginRequest = axios.post('https://localhost:7026/api/Account/login', {
          username: this.username,
          password: this.password
        })
        .then(response => {
          console.log(response);
          //this.test(); ////
          this.newJWT(response.data.accessToken, response.data.refreshToken);
          //console.log("access token: " + this.accessToken);
          this.updateCurrentUserFromAPI(this.accessToken);
        })
        .catch(function (error) {
          console.log(error);
        });
      },

  
      ...mapActions(useAccountStore, ['updateCurrentUser', 'newJWT']),
      
    },

    computed: {
      valid() {
        return !!this.username && !!this.password;
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken', 'currentUser']),
    }
  }
</script>