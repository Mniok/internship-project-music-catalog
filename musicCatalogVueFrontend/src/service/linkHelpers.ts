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

  var spotifyLinkBase : string = "open.spotify.com/track/";
  function trimSpotifyLink(link : string) : string | boolean | undefined {      //ex. https://open.spotify.com/track/03sEzk1VyrUZSgyhoQR0LZ?si=0eb2a7047aec4363 //pneuma, działa bez ?si
    if (!link)
      return false;

    if (link.startsWith("https://"))
      link = link.substring("https://".length);

    if (link.startsWith("http://"))
      link = link.substring("http://".length);

    if (link.substring(0, spotifyLinkBase.length) == spotifyLinkBase)
      return cleanupLink( link.substring(spotifyLinkBase.length) );
    
    return false;
  }

  var applemusicLinkBase : string = "music.apple.com/pl/album/";
  function trimApplemusicLink(link : string) : string | boolean | undefined {       //ex. https://music.apple.com/pl/album/pneuma/1475686696?i=1475686700
    if (!link)
      return false;

    if (link.startsWith("https://"))
      link = link.substring("https://".length);

    if (link.startsWith("http://"))
      link = link.substring("http://".length);

    if (link.substring(0, applemusicLinkBase.length) == applemusicLinkBase)
      return link.substring(applemusicLinkBase.length);
    
    return false;
  }

  var bandcampLinkMiddle : string = "bandcamp.com/track/";
  function trimBandcampLink(link : string) : string | boolean | undefined {         //ex. https://blindguardian.bandcamp.com/track/noldor-dead-winter-reigns
                                                                                    //ex. https://blindguardian.bandcamp.com/track/mirror-mirror-2                       
    if (!link)                                                                      //ex. https://blindguardian.bandcamp.com/track/battlefield
        return false;                                                               //ex. https://fellowshipmetal.bandcamp.com/track/glory-days

    if (link.startsWith("https://"))
        link = link.substring("https://".length);

    if (link.startsWith("http://"))
        link = link.substring("http://".length);

    var subdomain : string;
    var dotPos : number = link.indexOf('.');
    subdomain = link.substring(0, dotPos);
    link = link.substring(dotPos+1);

    if(subdomain.includes("/") || subdomain.includes(":"))
        return false;

    //console.log(subdomain + "." + link.substring(bandcampLinkMiddle.length)) ////
    if (link.substring(0, bandcampLinkMiddle.length) == bandcampLinkMiddle)
        return cleanupLink(subdomain) + "." + cleanupLink( link.substring(bandcampLinkMiddle.length) );
  
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

    function restoreYoutubeLink(linkBody : string) : string {
        return "https://" + youtubeLinkBase + linkBody;
    }

    function restoreSpotifyLink(linkBody : string) : string {
        return "https://" + spotifyLinkBase + linkBody;
    }

    function restoreApplemusicLink(linkBody : string) : string {
        return "https://" + applemusicLinkBase + linkBody;
    }

    function restoreBandcampLink(link : string) : string {
        var subdomain : string; var linkbody : string;
        [subdomain, linkbody] = link.split(".");
        return "https://" + subdomain + "." + bandcampLinkMiddle + linkbody;
    }

    function restoreSoundcloudLink(linkBody : string) : string {
        return "https://" + soundcloudLinkBase + linkBody;
    }

export { Link, cleanupLink, trimYoutubeLink, trimSpotifyLink, trimApplemusicLink, 
        trimBandcampLink, trimSoundcloudLink, restoreYoutubeLink , restoreSpotifyLink,
        restoreApplemusicLink, restoreBandcampLink, restoreSoundcloudLink
};