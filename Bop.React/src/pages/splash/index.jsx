/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useEffect } from "react";
import styles from "./index-style";
import { routes } from "./../../constants/constant";
import { localeService } from "./../../services/locale";
import history from "./../../services/history";
import Zoom from "react-reveal/Zoom";
import { config } from "../../config";
import { Header } from "./../../components/Share/header";
import { Logo } from "./../../components/Share/logo";
import { userService } from "./../../services/user";

const navigate = () => {
  setTimeout(() => {
    if (userService.isAuthUserLoggedIn()) {
      history.push(routes.home);
      return;
    }
    if (localeService.getLanguageInitialized()) {
      history.push(routes.login.base);
      return;
    }
    history.push(routes.language);
  }, 700);
};

const Splash = () => {
  useEffect(() => navigate());

  return (
    <div className="h-100 d-flex flex-column" css={css(styles)}>
      <Header height="220" />

      <Zoom duration={config.animationDuration}>
        <div className="d-flex flex-grow-1 w-100">
          <div className="d-flex flex-column flex-grow-1">
            <div className="container">
              <Logo className="logo" />
            </div>
          </div>
        </div>
      </Zoom>
    </div>
  );
};

export default Splash;
