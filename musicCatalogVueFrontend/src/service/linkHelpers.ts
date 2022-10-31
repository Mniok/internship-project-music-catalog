  interface Link {
    toSite: string,
    linkBody: string
  }


  function cleanupLink(linkEnd:string) : string | undefined {
    //console.log(linkEnd);   ////
    //console.log(linkEnd?.split('/').at(0)?.split('?').at(0)?.split('&').at(0));   ////
    return linkEnd?.split('/')[0]?.split('?')[0]?.split('&')[0];
  }

  var youtubeLinkBase : string = "www.youtube.com/watch?v=";      //ex. https://www.youtube.com/watch?v=ORnvO1VyYMk   //Man On The Silver Mountain btw
  var youtubeLinkShare : string = "youtu.be/";                    //ex. https://youtu.be/ORnvO1VyYMk
  var youtubeLinkShorts : string = "www.youtube.com/shorts/";     //ex. https://www.youtube.com/shorts/BZ_276VGGv4
  function trimYoutubeLink(link : string) : string | boolean | undefined {
    if (!link)
      return false;

    if (link.startsWith("https://"))
      link = link.substring("https://".length);

    if (link.startsWith("http://"))
      link = link.substring("http://".length);

    if (link.substring(0, youtubeLinkBase.length) == youtubeLinkBase)
      return cleanupLink( link.substring(youtubeLinkBase.length) );

    if (link.substring(0, youtubeLinkShare.length) == youtubeLinkShare)
      return cleanupLink( link.substring(youtubeLinkShare.length) );

    if (link.substring(0, youtubeLinkShorts.length) == youtubeLinkShorts)
      return cleanupLink( link.substring(youtubeLinkShorts.length) );

    return false;
  }

  function trimSpotifyLink(link : string) : string | boolean | undefined {
    return false;
  }

  function trimItunesLink(link : string) : string | boolean | undefined {
    return false;
  }

  function trimBandcampLink(link : string) : string | boolean | undefined {
    return false;
  }

  var soundcloudLinkBase : string = "soundcloud.com/";           //ex. https://soundcloud.com/jimihendrix/3b-wav , https://soundcloud.com/creedence-clearwater-revival/fortunate-son-at-the-royal //ten 2gi not available in poland
  function trimSoundcloudLink(link : string) : string | boolean | undefined {
    if (!link)
      return false;

    if (link.startsWith("https://"))
      link = link.substring("https://".length);

    if (link.startsWith("http://"))
      link = link.substring("http://".length);

    if (link.substring(0, soundcloudLinkBase.length) == soundcloudLinkBase){
      link = link.substring(soundcloudLinkBase.length);
      var linkArtist : string | undefined, linkSong : string | undefined;
      [linkArtist, linkSong] = link.split("/");
      linkArtist = cleanupLink(linkArtist);
      linkSong = cleanupLink(linkSong);
      //console.log(link + " ==> " + linkArtist + " / " + linkSong); ////
      if (!!linkArtist && !!linkSong)
        return linkArtist + '/' + linkSong;
    }

    return false;
  }

  export { Link, cleanupLink, trimYoutubeLink, trimSpotifyLink, trimItunesLink, trimBandcampLink, trimSoundcloudLink };