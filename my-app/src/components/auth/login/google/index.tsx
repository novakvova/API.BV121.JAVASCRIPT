import { useEffect } from "react";
import { APP_ENV } from "../../../../env";
import './style.css';

const GoogleAuth=() => {

    const handleLogin = (resp: any) => {
        const {credential} = resp;
        console.log("login google ", credential);
    }

    useEffect(() => {
        window.google.accounts!.id.initialize({
            client_id: APP_ENV.GOOGLE_CLIENT_ID,
            callback: handleLogin
        });

        window.google.accounts.id.renderButton(
            document.getElementById("customBtn"),
        {
            theme: "outline",
            size: 'large',
            type: 'icon',
            width: "40"
            //text: "signin",
            //locale: "uk-ua"
        });
    },[]);

    return (
      <>
        <div>
          <div id="customBtn"></div>
          {/* <span className="h2"> Вхід через гугл</span> */}
        </div>
        {/* <a className="btn btn-outline-primary text-uppercase mb-4" href="#">
          <img src="https://img.icons8.com/color/16/000000/google-logo.png" />{" "}
          Log In with Google
        </a> */}
      </>
    );
}

export default GoogleAuth;