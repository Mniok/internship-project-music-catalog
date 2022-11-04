import { defineStore } from 'pinia'
import axios from 'axios';
//import router from '.././router'; //`router: router` w options wywala bledy
//import router from '@/router';
import { vueApp } from '../main';   //// dla routera 


export const useSongStore = defineStore('songStore', {

  state: () => ({
    searchUploadedByMe: false,
    searchByTitle: "none",
    searchByArtist: "none",
    searchByGenre: "none",
    searchSongFlags: "none",

    currentPage: 0,
  }),

  /*getters: {
    searchSongFlags: state => !!state.searchByTitle ? "t" : ""
      + !!state.searchByArtist ? "a" : ""
      + !!state.searchByGenre ? "g" : ""
      + (!!state.searchByTitle || !!state.searchByArtist || !!state.searchByGenre) ? "" : "none",
      /// eg. search by title and genre -> "tg",
      /// search by artist only -> "a",
      /// search by none of these three -> "none"
  },*/

  actions: {
    saveSearchCriteria(searchByMe: boolean, searchByTitle: string, searchByArtist: string, searchByGenre: string){
      this.searchUploadedByMe = searchByMe;
      this.searchByTitle = searchByTitle;
      this.searchByArtist = searchByArtist;
      this.searchByGenre = searchByGenre;

      var flags = "none";

      if(!!searchByTitle || !!searchByArtist || !!searchByGenre)
        flags = "";
      if(!!searchByTitle)
        flags += "t";
      if(!!searchByArtist)
        flags += "a";
      if(!!searchByGenre)
        flags += "g";
      this.searchSongFlags = flags;
    }

  },


})
