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
          v-model="valid"
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
      login () {
        this.$refs.form.validate()

        var getTest = axios('https://localhost:7026/api/Account/testController');
        
        axios.post('https://localhost:7026/api/Account/login', {
          username: this.username,
          password: this.password
        })
        .then(function (response) {
          console.log(response);
        })
        .catch(function (error) {
          console.log(error);
        });

      },
    },

    computed: {
      valid() {
        return !!this.username && !!this.password;
      }
    }
  }
</script>