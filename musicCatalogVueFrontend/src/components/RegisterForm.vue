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
            counter
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

          <v-text-field
            v-model="confirmPassword"
            :append-icon="showConfirmPass ? 'mdi-eye' : 'mdi-eye-off'"
            :rules="confirmPasswordRules.concat(passwordsMatch)"
            :type="showConfirmPass ? 'text' : 'password'"
            label="Confirm password:"
            counter
            required
            @click:append="showConfirmPass = !showConfirmPass"
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


<script lang="ts">
  import { useAccountStore } from '../store/account';
  import { mapState, mapActions } from 'pinia';
  //import { LoginForm } from './LoginForm';
  import mixinLoggedInAs from '../mixins/loggedInAs';


  export default {
    name: "registerForm",

    mixins: [mixinLoggedInAs],

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
      showPass: false,
      showConfirmPass: false,
    }),

    methods: {
      
      login (uname: string, pass: string) {
        var loginRequest = axios.post('https://localhost:7026/api/Account/login', {
          username: uname,
          password: pass
        })
        .then(response => {
          this.newJWT(response.data.accessToken, response.data.refreshToken);
          this.updateCurrentUserFromAPI(this.accessToken);

          this.$router.push({ name: 'appview' });
        })
        .catch(function (error :any) {
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
        .catch(function (error :any) {
          console.log(error);
          if(error.response == undefined){
            alert("Couldn't connect to the server. Try again later.");
          }
          if(error.response.status == 400){
            alert("User already exists! Try another username.");
          }
        });
      },



  
      ...mapActions(useAccountStore, ['updateCurrentUser', 'newJWT']),
      
    },

    computed: {
      valid() {
        //console.log(this.passwordsMatch); ////
        return !!this.username && !!this.password && this.password.length >= 8 && !!this.confirmPassword && this.passwordsMatch === true; /// === true because string returned on false was truthy
      },

      passwordsMatch() {
        return (this.password == this.confirmPassword) || 'Password must match';
      },

      ...mapState(useAccountStore, ['accessToken', 'refreshToken', 'currentUser']),
    }
  }
</script>