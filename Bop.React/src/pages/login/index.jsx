/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./index-style";
import { Header } from "../../components/Share/header";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import Zoom from "react-reveal/Zoom";
import { config } from "../../config";
import { mask } from "./../../constants/masks";
import { login } from "./../../actions/auth";
import { StickyButton } from "../../components/Share/button";
import { SecondaryText, TextDirection, Title, PrimaryText } from "../../components/Share/text";
import { MaskedInputWithError, InputWithError } from "./../../components/Share/input";
import { BsLock } from "react-icons/bs";
import { FiPhone } from "react-icons/fi";
import { FaChevronRight } from "react-icons/fa";
import { colors } from "./../../constants/colors";
import { useDispatch } from "react-redux";
import { useTranslation } from "react-i18next";
import { SwitchButton } from "./../../components/Share/switch-button";
import { routes } from "../../constants/constant";
import history from "./../../services/history";

const Schema = Yup.object().shape({
  mobile: Yup.string()
    .matches(mask.validation.mobile, "share.validation.bad_format")
    .required("share.mobile.validation.required"),
  password: Yup.string().required("share.password.validation.required").min(6, "share.password.validation.min"),
});

const Login = () => {
  const dispatch = useDispatch();
  const { t } = useTranslation();

  return (
    <div className="h-100 d-flex flex-column" css={css(styles)}>
      <Header back logo height="180" />

      <Zoom duration={config.animationDuration}>
        <div className="d-flex flex-grow-1">
          <Formik
            initialValues={{
              mobile: "",
              password: "",
              rememberMe: false,
            }}
            validationSchema={Schema}
            onSubmit={(values, { resetForm }) => {
              console.log(values);
              dispatch(login(values));
            }}
          >
            {() => (
              <Form className="d-flex flex-column flex-grow-1">
                <div className="d-flex flex-column flex-grow-1" css={css(styles)}>
                  <div className="container flex-grow-1">
                    <TextDirection center>
                      <Title text={t("share.message.welcome")} />
                    </TextDirection>
                    <SecondaryText className="text-center p-2">
                      {t("signup.title.enter_mobile_number")}
                    </SecondaryText>

                    <MaskedInputWithError
                      className="text-left"
                      name="mobile"
                      placeholder={t("share.text.mobile")}
                      autoComplete="off"
                      mask={mask.mobile}
                      icon={<FiPhone className="left-addon" color={colors.gray_light_2} />}
                      inputMode="numeric"
                    />

                    <InputWithError
                      className="text-left"
                      name="password"
                      placeholder={t("share.text.password")}
                      autoComplete="off"
                      icon={<BsLock className="left-addon" color={colors.gray_light_2} />}
                      type="password"
                    />

                    <div className="mt-2 mb-4">
                      <div className="row align-items-center col-12 m-0 p-0">
                        <label className="flex-grow-1 m-0">{t("share.text.remember_me")}</label>
                        <SwitchButton fieldname="rememberMe" />
                      </div>
                    </div>

                    <div
                      className="text-center mt-1 font-small-1"
                      onClick={() => history.push(routes.sign_up.base)}
                    >
                      <PrimaryText>{t("signup.button.text.register")}</PrimaryText>
                    </div>
                  </div>

                  <StickyButton type="submit">
                    <span>{t("share.button.continue")}</span>
                    <i>{<FaChevronRight />}</i>
                  </StickyButton>
                </div>
              </Form>
            )}
          </Formik>
        </div>
      </Zoom>
    </div>
  );
};

export default Login;
