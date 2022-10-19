<template>
  <v-container>
    <v-card class="pa-8 d-flex align-center" elevation="4" outlined>
      <v-col>

        <v-row justify="center">
          <v-card-title>
            Create a new re:mail account
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

          <v-text-field
            v-model="confirmPassword"
            :rules="confirmPasswordRules.concat(passwordsMatch)"
            label="Confirm password:"
            required
          ></v-text-field>

          <v-spacer />

          <v-row class="mt-4 ml-0 mr-0">
            <v-spacer />
            <v-btn
              :disabled="!valid"
              color="success"
              @click="register"
            >
              Create account
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
  //import { LoginForm } from './LoginForm';

  export default {
    name: "registerForm",

    data: () => ({
      username: '',
      usernameRules: [
        v => !!v || 'Please enter Your Username',
      ],
      password: '',
      passwordRules: [
        v => !!v || 'Please enter Your Password',
        v => (v && v.length >= 8) || 'Password must be at least 8 characters',
      ],
      confirmPassword: '',
      confirmPasswordRules: [
        v => !!v || 'Please enter Your Password again for confirmation',
        //v => (v && true && console.log(v + ", ")) || 'Debug',
        //v => (v && v==this.password) || 'Password must match', //didn't have access to password, placed in computed passwordsMatch instead and concated to rules so it's one array
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
      
      login (uname, pass) {
        var loginRequest = axios.post('https://localhost:7026/api/Account/login', {
          username: uname,
          password: pass
        })
        .then(response => {
          this.newJWT(response.data.accessToken, response.data.refreshToken);
          this.updateCurrentUserFromAPI(this.accessToken);
        })
        .catch(function (error) {
          console.log(error);
        });
      },


      register () {
        this.$refs.form.validate();

        axios.post('https://localhost:7026/api/Account/registration', {
          username: this.username,
          password: this.password
        })
        .then(response => {
          this.login(this.username, this.password);
        })
        .catch(function (error) {
          console.log(error);
        });
      },



  
      ...mapActions(useAccountStore, ['updateCurrentUser', 'newJWT']),
      
    },

    computed: {
      valid() {
        console.log(this.passwordsMatch);
        return !!this.username && !!this.password && !!this.confirmPassword && this.passwordsMatch === true; /// === true because string returned on false was truthy
      },

      passwordsMatch() {
        return (this.password == this.confirmPassword) || 'Password must match';
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken', 'currentUser']),
    }
  }
</script>