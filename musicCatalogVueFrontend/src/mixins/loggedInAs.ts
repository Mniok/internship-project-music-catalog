/*import { useAccountStore } from '../store/account';
//import { mapState, mapActions } from 'pinia';
import { mapActions } from 'pinia';
import axios from 'axios';

var mixinLoggedInAs = {
    methods: {
        updateCurrentUserFromAPI(accessToken : String) {
            //console.log("log from mixin"); ////
        
            //console.log(`bearer ${accessToken}`) ////
            axios.get(`https://localhost:7026/api/Account/user`, {
              headers: { 'Authorization': `bearer ${accessToken}` }
            })
            .then(response => {
              this.updateCurrentUser(response.data.username);
              //console.log(`logged in as ${this.currentUser}`);
            })
            .catch(function (error) {
              console.log(error);
            });
          },

          ...mapActions(useAccountStore, ['updateCurrentUser']),
    },

    //computed: {
    //  ...mapState(useAccountStore, ['currentUser']),
    //}
}

export default mixinLoggedInAs;
*/