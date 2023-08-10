export const environment = {
  production: true,
   API_URL:"https://localhost:7179/api/",
   API_FILE_URL:"https://localhost:7179",

  //  API_URL:"https://loftyrooms.cyberasol.com/api/api/",
  //  API_FILE_URL:"https://loftyrooms.cyberasol.com/api",
  GET_TOKEN(){
    var token = sessionStorage.getItem('token');
    if(token == '' || token == null)
    {
      token == '';
    }
    return token;
  },
  GET_USERID(){
    var userid = parseInt(sessionStorage.getItem('userid') || '{}');
    if(userid == null || isNaN(userid))
    {
      userid = 0;
    }
    return userid;
  },
  GET_USERNAME(){
    var username = sessionStorage.getItem('username');
    if(username == '' || username == null)
    {
      username == '';
    }
    return username;
  },
  GET_CLAIMS(){
    var claims = sessionStorage.getItem('claims');
    if(claims == '' || claims == null)
    {
      claims == '';
    }
    return claims;
  }
};
