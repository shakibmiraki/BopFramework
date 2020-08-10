/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./index-style";
import { LanguageSelector } from "../../components/LanguageSelector";
import { ThemeSelector } from "./../../components/ThemeSelector/index";
import { useTranslation } from "react-i18next";
import { StickyButton } from "./../../components/Share/button";
import { Header } from "./../../components/Share/header";
import Zoom from "react-reveal/Zoom";
import { config } from "../../config";
import { routes } from "./../../constants/constant";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import { localeService } from "./../../services/locale";
import { themeService } from "../../services/theme";
import history from "./../../services/history";
import { FaChevronRight } from "react-icons/fa";
import { Title, TextDirection } from "./../../components/Share/text";

const Schema = Yup.object().shape({});

const Language = () => {
  const { t } = useTranslation();

  return (
    <div className="h-100 d-flex flex-column" css={css(styles)}>
      <Header logo />

      <Zoom duration={config.animationDuration}>
        <div className="d-flex flex-grow-1">
          <Formik
            initialValues={{}}
            validationSchema={Schema}
            onSubmit={(values) => {
              //set init language
              localeService.setLanguageInitialized();
              //set init theme
              themeService.setThemeInitialized();
              history.push(routes.sign_up.base);
            }}
          >
            {() => (
              <Form className="d-flex flex-column flex-grow-1">
                <div className="container flex-grow-1 d-flex flex-column">
                  <TextDirection center>
                    <Title text={t("language.title.choose_theme_and_language")} />
                  </TextDirection>
                  <LanguageSelector className="flex-grow-1" />
                  <ThemeSelector className="flex-grow-1" />
                </div>
                <StickyButton type="submit" bottom="0">
                  <span>{t("share.button.continue")}</span>
                  <i>{<FaChevronRight />}</i>
                </StickyButton>
              </Form>
            )}
          </Formik>
        </div>
      </Zoom>
    </div>
  );
};

export default Language;
