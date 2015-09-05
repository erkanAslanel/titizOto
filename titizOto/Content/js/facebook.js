window.fbAsyncInit = function () {
    FB.init({
        appId: '253592151498110', // App ID
        status: false, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        xfbml: true,
        version: 'v2.0'// parse XFBML
    });

    FB.Event.subscribe('auth.authResponseChange', function (response) {

        console.log("authResponseChange");
        console.log(response);

        if (response.status === 'connected') {

            var uid = response.authResponse.userID;
            var accessToken = response.authResponse.accessToken;

            var form = document.createElement("form");
            form.setAttribute("method", 'post');
            form.setAttribute("action", '/tr/LoginRegister/Facebook');

            var field = document.createElement("input");
            field.setAttribute("type", "hidden");
            field.setAttribute("name", 'accessToken');
            field.setAttribute("value", accessToken);
            form.appendChild(field);

            document.body.appendChild(form);
            form.submit()

       

        } else if (response.status === 'not_authorized') {

            console.log("not_authorized");

        } else { 

            console.log("cancel");
        }
    });

  
};

 


(function(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/tr_TR/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'))