/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./index-style";
import { NavLink } from "react-router-dom";
import Fade from "react-reveal/Fade";
import { Header } from "./../../components/Share/header";
import { PrimaryText } from "../../components/Share/text";
import { ThemeBackgroundColor } from "../../components/Share/color";
import { colors } from "../../constants/colors";
import { config } from "../../config";

const NotFound = () => {
  return (
    <div css={css(styles)}>
      
        <Header back logo burger  />
        <div className="container">
        <div className="error-holder">
          <ThemeBackgroundColor color={colors.white} className="box">
            <p className="error-type">
              <Fade bottom cascade duration={config.animationDuration}>
                <span>4</span>
                <span>0</span>
                <span>4</span>
              </Fade>
            </p>
          </ThemeBackgroundColor>

          <Fade bottom duration={config.animationDuration}>
            <PrimaryText className="error-text">یافت نشد!</PrimaryText>
          </Fade>
          <Fade bottom duration={config.animationDuration}>
            <p className="error-recommendation">
              تمایلی به بازگشت به{" "}
              <NavLink to="/home">
                <PrimaryText>صفحه اصلی</PrimaryText>
              </NavLink>{" "}
              دارید ؟
            </p>
          </Fade>
        </div>
      </div>
    </div>
  );
};

export default NotFound;
