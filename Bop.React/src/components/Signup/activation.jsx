/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useTranslation } from "react-i18next";
import { StickyButton } from "../Share/button";
import { InputWithError } from "../Share/input";
import { useDispatch } from "react-redux";
import { useFormikContext } from "formik";
import { activate, resend } from "../../actions/auth";
import { utilService } from "./../../services/utils";
import { useEffect, useState } from "react";
import { setStep } from "./../../actions/auth";
import { signup_steps } from "./../../models/steps/signup";
import history from "./../../services/history";
import { FaChevronRight } from "react-icons/fa";
import Countdown, { zeroPad } from "react-countdown";
import { TextDirection, SecondaryText, Title } from "./../Share/text";
import { setTimer } from "./../../actions/counter";
import { useSelector } from "react-redux";

const styles = css({
  "input:-moz-placeholder": {
    textAlign: "center !important",
  },
  "input:-ms-input-placeholder": {
    textAlign: "center !important",
  },
  "input::-webkit-input-placeholder": {
    textAlign: "center !important",
  },
  ".font-smaller": {
    fontSize: "0.8rem",
  },
});

const Activation = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const [showResendSms, setShowResendSms] = useState();
  const { values, submitForm, validateForm } = useFormikContext();
  const { timer } = useSelector((state) => state.counter);
  const renderer = ({ seconds }) => {
    return (
      <span className="text-left font-smaller">
        {`${t("signup.title.activation.resend_code")} (${zeroPad(seconds)} ${t(
          "signup.title.activation.second"
        )})`}
      </span>
    );
  };

  useEffect(() => {
    dispatch(setTimer(utilService.getActivationSmsTimer()));
    dispatch(setStep(signup_steps.activation));
    // utilService.autofill();
  }, [history.location.pathname]);

  return (
    <div className="d-flex flex-column flex-grow-1" css={css(styles)}>
      <div className="container flex-grow-1">
        <TextDirection center>
          <Title text={t("signup.title.activation.activation_code")} />
        </TextDirection>
        <SecondaryText className="text-center p-2">
          {t("signup.title.activation.enter_activation_code")}
        </SecondaryText>

        <InputWithError
          className="text-center"
          name="code"
          placeholder={t("share.text.activation_code")}
          autoComplete="one-time-code"
          inputMode="numeric"
        />
        <div className="row text-secondary font-smaller">
          <TextDirection className="col-6">{t("signup.activation.sms.not_received")}</TextDirection>
          <TextDirection className="col-6" reverse>
            {showResendSms ? (
              <div
                className="text-info"
                onClick={() => {
                  dispatch(resend(values));
                  setShowResendSms(false);
                  dispatch(setTimer(utilService.getActivationSmsTimer()));
                }}
              >
                {t("signup.title.activation.resend_code_again")}
              </div>
            ) : (
              <Countdown
                autoStart={true}
                date={timer}
                onStart={() => setShowResendSms(false)}
                onComplete={() => setShowResendSms(true)}
                renderer={renderer}
              ></Countdown>
            )}
          </TextDirection>
        </div>
      </div>

      <StickyButton
        type="button"
        onClick={() => {
          validateForm().then((errors) => {
            submitForm();
            if (utilService.isEmpty(errors)) {
              dispatch(activate(values));
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

export default Activation;
