/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./index-style";
import { useTranslation } from "react-i18next";
import { Paragraph } from "./../../components/Share/text";
import { Header } from "./../../components/Share/header";
import Zoom from "react-reveal/Zoom";
import { config } from "../../config";
import { StyledLogo } from "../../components/Share/logo";
import { IoLogoInstagram, IoMdPaperPlane } from "react-icons/io";

const AboutUs = () => {
  const { t } = useTranslation();

  return (
    <div css={css(styles)}>
      <Header back text={t("about_us.title.about_us")} burger />
      <div className="container">
        <Zoom duration={config.animationDuration}>
          <div className="about-us mt-3 mb-3">
            <div className="row mb-3">
              <StyledLogo />
            </div>
            <div className="row">
              <Paragraph text={t("about_us.copyright.paragraph")} />
            </div>

            <div className="row justify-content-center call-info p-2">
              <span className="text-secondary">تماس با ما</span>
              <span className="text-secondary"> : </span>
              <span>
                <a href="tel:1688">1688</a>
              </span>
            </div>

            <div className="row justify-content-center social-icon p-2">
              <IoLogoInstagram className="text-secondary" css={css({ fontSize: "28px" })} />

              <IoMdPaperPlane className="text-secondary" css={css({ fontSize: "28px" })} />
            </div>
          </div>
        </Zoom>
      </div>
    </div>
  );
};

export default AboutUs;
