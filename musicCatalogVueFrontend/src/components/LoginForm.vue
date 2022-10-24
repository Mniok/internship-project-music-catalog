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
            :append-icon="showPass ? 'mdi-eye' : 'mdi-eye-off'"
            :rules="passwordRules"
            :type="showPass ? 'text' : 'password'"
            label="Password:"
            counter
            required
            @click:append="showPass = !showPass"
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
  import mixinLoggedInAs from '../mixins/loggedInAs';

  export default {
    name: "loginForm",

    mixins: [mixinLoggedInAs],

    data: () => ({
      username: '',
      usernameRules: [
        v => !!v || 'Please enter Your Username',
      ],
      password: '',
      passwordRules: [
        v => !!v || 'Please enter Your Password',
      ],
      showPass: false,
    }),

    methods: {

      /*updateCurrentUserFromAPI(accessToken) {
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
      },*/

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
        .then(response => { //logged in into valid account, otherwise error is thrown
          //console.log(response);
          //this.test(); ////
          this.newJWT(response.data.accessToken, response.data.refreshToken);
          //console.log("access token: " + this.accessToken);
          this.updateCurrentUserFromAPI(this.accessToken);

          this.$router.push({ name: 'appview' });
          // ! nie to samo co name: 'App' ! App to cała instancja, AppView to sam ten wycinek gdzie coś się dzieje.
          //}

        })
        .catch(function (error) {
          console.log(error);
          if(error.response == undefined){
            alert("Couldn't connect to the server. Try again later.");
          }
          if(error.response.status == 401){
            alert("Wrong username or password entered. Try again.");
          }
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