/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import { StickyButton } from "../../components/Share/button";
import { MaskedInputWithError } from "../../components/Share/input";
import { useDispatch } from "react-redux";
import { register, setStep } from "./../../actions/auth";
import { useFormikContext } from "formik";
import { utilService } from "./../../services/utils";
import { useEffect } from "react";
import { signup_steps } from "./../../models/steps/signup";
import history from "./../../services/history";
import { FiPhone } from "react-icons/fi";
import { BsLock, BsShieldLock } from "react-icons/bs";
import { mask } from "./../../constants/masks";
import { Title, SecondaryText, TextDirection, PrimaryText } from "./../Share/text";
import { FaChevronRight } from "react-icons/fa";
import { colors } from "../../constants/colors";
import { InputWithError } from "./../Share/input";
import { routes } from "../../constants/constant";

const styles = css({});

const BasicSignUp = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const { values, submitForm, validateForm } = useFormikContext();

  useEffect(() => {
    dispatch(setStep(signup_steps.basic));
  }, [history.location.pathname]);

  return (
    <div className="d-flex flex-column flex-grow-1" css={css(styles)}>
      <div className="container flex-grow-1">
        <TextDirection center>
          <Title text={t("share.message.welcome")} />
        </TextDirection>
        <SecondaryText className="text-center p-2">{t("signup.title.enter_mobile_number")}</SecondaryText>

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
        <InputWithError
          className="text-left"
          name="confirmPassword"
          placeholder={t("share.text.confirm_password")}
          autoComplete="off"
          icon={<BsShieldLock className="left-addon" color={colors.gray_light_2} />}
          type="password"
        />
        <div className="text-center mt-1 font-small-1" onClick={() => history.push(routes.login.base)}>
          <PrimaryText>{t("signup.button.text.login")}</PrimaryText>
        </div>
      </div>

      <StickyButton
        type="button"
        bottom="0"
        onClick={() => {
          validateForm().then((errors) => {
            submitForm();
            if (utilService.isEmpty(errors)) {
              dispatch(register(values));
            }
          });
        }}
      >
        <span>{t("share.button.continue")}</span>
        <i>{<FaChevronRight />}</i>
      </StickyButton>
    </div>
  );
};

export default BasicSignUp;
